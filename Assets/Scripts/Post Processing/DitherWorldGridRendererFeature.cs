using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using System;
using UnityEngine.Experimental.Rendering;

public class DitherWorldGridRendererFeature : ScriptableRendererFeature
{
    [SerializeField, Space] private Shader shader;
    [SerializeField] private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    [SerializeField] int passEventOrder = 0;
    [SerializeField, Space] internal DefaultDitherWorldGridSettings settings;
    [SerializeField] internal ComputeShader computeShader;
    [SerializeField] internal Texture2D noiseTexture;

    internal Material material;
    private DitherWorldGridPass ditherWorldGridPass;
    private RTHandle ditherWorldGridFeatureRTHandle;
    private bool isInitialized = false;

    public override void Create()
    {
        material = CoreUtils.CreateEngineMaterial(shader);

        ditherWorldGridPass = new DitherWorldGridPass(this);
        ditherWorldGridPass.renderPassEvent = (RenderPassEvent)((int)renderPassEvent + passEventOrder);

    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Ensure only game cameras get the render pass;
        if (renderingData.cameraData.cameraType != CameraType.Game)
            return;

        // Initialize feature RTHandle if needed;
        if (ditherWorldGridFeatureRTHandle == null || !isInitialized)
        {
            // Clean up any existing handle first;
            ReleaseDitherWorldGridFeatureRTHandle();

            // Create a new RTHandle with valid dimensions;
            ditherWorldGridFeatureRTHandle = RTHandles.Alloc(
                renderingData.cameraData.cameraTargetDescriptor.width,
                renderingData.cameraData.cameraTargetDescriptor.height,
                slices: 1,
                depthBufferBits: DepthBits.None,
                colorFormat: GraphicsFormat.R16G16B16A16_SFloat,
                filterMode: FilterMode.Point,
                wrapMode: TextureWrapMode.Clamp,
                dimension: TextureDimension.Tex2D,
                enableRandomWrite: false,
                useMipMap: false,
                autoGenerateMips: false,
                isShadowMap: false,
                anisoLevel: 1,
                useDynamicScale: true,
                name: "DitherWorldGridFeatureRTHandle"
            );

            isInitialized = true;
        }

        // Configure and enqueue the pass with color texture request;
        ditherWorldGridPass.ConfigureInput(ScriptableRenderPassInput.Color);
        renderer.EnqueuePass(ditherWorldGridPass);
    }

    private void ReleaseDitherWorldGridFeatureRTHandle()
    {
        if (ditherWorldGridFeatureRTHandle != null)
        {
            ditherWorldGridFeatureRTHandle.Release();
            ditherWorldGridFeatureRTHandle = null;
        }
    }
}
public class DitherWorldGridPass : ScriptableRenderPass
{
    private readonly DitherWorldGridRendererFeature rendererFeature;

    private static readonly int gridScaleID = Shader.PropertyToID("_gridScale");
    private static readonly int gridThicknessID = Shader.PropertyToID("_gridThickness");

    private static readonly int gridComputeTexID = Shader.PropertyToID("_GridComputeTex");

    public DitherWorldGridPass(DitherWorldGridRendererFeature rendererFeature) => this.rendererFeature = rendererFeature;


    private class DitherWorldGridPassData
    { 
        internal Material material;
        internal TextureHandle sourceColor;
        internal DitherWorldGridVolumeComponent vc;
        internal TextureHandle targetColor;
        internal DefaultDitherWorldGridSettings defaultSettings;
    }

    private class DitherWorldGridComputePassData
    {
        internal Material material;
        internal ComputeShader computeShader;
        internal DitherWorldGridVolumeComponent vc;
        internal int kernalHandle;
        internal TextureHandle noiseTexture;
        internal TextureDesc texDesc;
        internal TextureHandle outputTexture;

        internal float time;
        internal Vector2 resolution;
        internal float gridScale;
        internal float gridThickness;
        internal Vector2 playerPos;

        internal float sonarPingTime;
        internal float flareTime;
        internal Vector2 flarePos;
        internal float radialScanTime;
        internal float radialScanRotation;
    }


    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

        TextureHandle sourceCamColor = resourceData.cameraColor;
        TextureDesc textDesc = resourceData.cameraColor.GetDescriptor(renderGraph); //getting exact same texture descriptor from the render graphs frame data
        textDesc.name = "DitherWorldGridTexture";
        textDesc.clearBuffer = false;
        TextureHandle dest = renderGraph.CreateTexture(textDesc); //copy of the source camera color for the shader to alter and replace the original camera color

        var computeResultDesc = new TextureDesc(textDesc.width, textDesc.height)
        {
            colorFormat = GraphicsFormat.R32G32B32A32_SFloat,
            enableRandomWrite = true,
            name = "ComputeResultTexture",
            clearBuffer = false,
            msaaSamples = MSAASamples.None,
            filterMode = FilterMode.Bilinear,
            wrapMode = TextureWrapMode.Repeat
        };
        TextureHandle computeResultTHandle = renderGraph.CreateTexture(computeResultDesc);

        using(var builder = renderGraph.AddComputePass<DitherWorldGridComputePassData>("Dither World Grid Compute Pass", out var computePassData))
        {
            computePassData.material = rendererFeature.material;
            computePassData.computeShader = rendererFeature.computeShader;
            computePassData.vc = VolumeManager.instance.stack.GetComponent<DitherWorldGridVolumeComponent>();
            computePassData.kernalHandle = rendererFeature.computeShader.FindKernel("CSMain");
            computePassData.noiseTexture = renderGraph.ImportTexture(RTHandles.Alloc(rendererFeature.noiseTexture));
            computePassData.texDesc = textDesc;
            computePassData.outputTexture = computeResultTHandle;

            computePassData.time = Time.time;
            computePassData.resolution = new Vector2(textDesc.width, textDesc.height);
            computePassData.gridScale = computePassData.vc.gridScale.value;
            computePassData.gridThickness = computePassData.vc.gridThickness.value;
            computePassData.playerPos = computePassData.vc.playerPos.value;

            computePassData.sonarPingTime = computePassData.vc.sonarPingTime.value;
            computePassData.flareTime = computePassData.vc.flareTime.value;
            computePassData.flarePos = computePassData.vc.flarePos.value;
            computePassData.radialScanTime = computePassData.vc.radialScanTime.value;
            computePassData.radialScanRotation = computePassData.vc.radialScanRotation.value;

            builder.UseTexture(computePassData.noiseTexture, AccessFlags.Read);
            builder.UseTexture(computeResultTHandle, AccessFlags.WriteAll);
            builder.AllowPassCulling(false);

            builder.SetRenderFunc((DitherWorldGridComputePassData data, ComputeGraphContext ctx) => ExecuteComputeShader(data, ctx));

        }



        DitherWorldGridPassData passData;
        using(var builder = renderGraph.AddRasterRenderPass<DitherWorldGridPassData>("Dither World Grid Render Pass", out passData)) //passData is a copy of DitherWorldGridPassData
        {
            passData.targetColor = dest;
            passData.sourceColor = resourceData.cameraColor;
            passData.material = rendererFeature.material;
            passData.vc = VolumeManager.instance.stack.GetComponent<DitherWorldGridVolumeComponent>();
            passData.defaultSettings = rendererFeature.settings;

            builder.UseTexture(passData.sourceColor); // getting souce color
            builder.SetRenderAttachment(passData.targetColor, 0, AccessFlags.WriteAll); // setting source color via the target that is a copy

            builder.SetRenderFunc((DitherWorldGridPassData data, RasterGraphContext ctx) => ExecuteDitherWorldGridPass(passData, ctx));
        }
        resourceData.cameraColor = passData.targetColor;
    }

    private static void ExecuteDitherWorldGridPass(DitherWorldGridPassData passData, RasterGraphContext ctx)
    {
        UpdateDitherWorldGridSettings(passData); // update settings before execution
        Blitter.BlitTexture(ctx.cmd, passData.sourceColor, Vector2.one, passData.material, 0); // execute shader pass

    }

    private void ExecuteComputeShader(DitherWorldGridComputePassData passData, ComputeGraphContext ctx)
    {
        ctx.cmd.SetComputeTextureParam(passData.computeShader, passData.kernalHandle, "_NoiseTex", passData.noiseTexture);
        ctx.cmd.SetComputeTextureParam(passData.computeShader, passData.kernalHandle, "Result", passData.outputTexture);
        ctx.cmd.SetComputeFloatParam(passData.computeShader, "_gridScale", passData.gridScale);
        ctx.cmd.SetComputeFloatParam(passData.computeShader, "_gridThickness", passData.gridThickness);
        ctx.cmd.SetComputeFloatParam(passData.computeShader, "_TimeY", passData.time);
        ctx.cmd.SetComputeFloatParam(passData.computeShader, "_sonarPingTime", passData.sonarPingTime);
        ctx.cmd.SetComputeFloatParam(passData.computeShader, "_flareTime", passData.flareTime);
        ctx.cmd.SetComputeFloatParam(passData.computeShader, "_radialScanTime", passData.radialScanTime);
        ctx.cmd.SetComputeFloatParam(passData.computeShader, "_radialScanRotation", passData.radialScanRotation);

        ctx.cmd.SetComputeVectorParam(passData.computeShader, "_playerPos", passData.playerPos);
        ctx.cmd.SetComputeVectorParam(passData.computeShader, "_flarePos", passData.flarePos);
        ctx.cmd.SetComputeVectorParam(passData.computeShader, "_Resolution", passData.resolution);
        
        int threadGroupX = Mathf.CeilToInt(passData.texDesc.width / 8f);
        int threadGroupY = Mathf.CeilToInt(passData.texDesc.height / 8f);

        ctx.cmd.DispatchCompute(passData.computeShader, passData.kernalHandle, threadGroupX, threadGroupY, 1);

        passData.material.SetTexture(gridComputeTexID, passData.outputTexture);
    }
    private static void UpdateDitherWorldGridSettings(DitherWorldGridPassData passData)
    {        
        float gridScale = passData.vc.active && passData.vc.gridScale.overrideState ? passData.vc.gridScale.value : passData.defaultSettings.gridScale;
        float gridThickness = passData.vc.active && passData.vc.gridThickness.overrideState ? passData.vc.gridThickness.value :  passData.defaultSettings.gridThickness;

        passData.material.SetFloat(gridScaleID, gridScale);
        passData.material.SetFloat(gridThicknessID, gridThickness);
    }
}

[Serializable]
public class DefaultDitherWorldGridSettings
{
    public float gridScale = 10f;
    public float gridThickness = 0.1f;
}


