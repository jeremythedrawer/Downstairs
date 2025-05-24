using UnityEngine;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule.Util;
using System;
using UnityEngine.Experimental.Rendering;

public class DitherWorldGridRendererFeature : ScriptableRendererFeature
{
    [SerializeField, Space] private Shader shader;
    [SerializeField] private RenderPassEvent renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing;
    [SerializeField] int passEventOrder = 0;
    [SerializeField, Space] private DefaultDitherWorldGridSettings settings;

    private Material material;
    private DitherWorldGridPass ditherWorldGridPass;
    private RTHandle ditherWorldGridFeatureRTHandle;
    private bool isInitialized = false;

    public override void Create()
    {

        if (shader == null)
        {
            Debug.LogError("Dither World Grid shade is missing.");
        }

        material = CoreUtils.CreateEngineMaterial(shader);

        ditherWorldGridPass = new DitherWorldGridPass(material, settings);
        ditherWorldGridPass.renderPassEvent = (RenderPassEvent)((int)renderPassEvent + passEventOrder);
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        // Ensure only game cameras get the render pass;
        if (renderingData.cameraData.cameraType != CameraType.Game)
            return;

        if (shader == null || material == null)
        {
            Debug.LogWarning("DataMosh shader or material is missing!");
            return;
        }

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

        // Update the render pass with the current ditherFeatureRTHandle;
        ditherWorldGridPass.SetDitherWorldRTHandle(ditherWorldGridFeatureRTHandle);

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
    private DefaultDitherWorldGridSettings defaultSettings;
    private Material material;
    private RenderTextureDescriptor ditherWorldGridTextureDescriptor;
    private RTHandle ditherWorldGridPassRTHandle;
    private TextureHandle ditherWorldGridTextureHandle;

    private static readonly int gridScaleID = Shader.PropertyToID("_gridScale");
    private static readonly int gridFallOffID = Shader.PropertyToID("_gridFallOff");
    private static readonly int gridThicknessID = Shader.PropertyToID("_gridThickness");
    private static readonly int sonarPingTimeID = Shader.PropertyToID("_sonarPingTime");
    private static readonly int playerPosID = Shader.PropertyToID("_playerPos");

    private const string temporaryTextureName = "TempDitherWorldGrid";
    private const string k_CalculateDitherWorldGridPass = "CalculateDitherWorldGridPass";
    private const string k_ApplyDitherWorldGridPass = "ApplyDitherWorldGridPass ";
    public DitherWorldGridPass(Material material, DefaultDitherWorldGridSettings settings)
    {
        this.defaultSettings = settings;
        this.material = material;
        ditherWorldGridTextureDescriptor = new RenderTextureDescriptor(Screen.width, Screen.height, RenderTextureFormat.ARGBFloat, 0);


    }

    public override void RecordRenderGraph(RenderGraph renderGraph, ContextContainer frameData)
    {
        if (material == null)
        {
            Debug.LogWarning("Dither World Grid missing material. Skipping render pass.");
            return;
        }

        if (ditherWorldGridPassRTHandle == null)
        {
            Debug.LogWarning("Dither World Grid missing RT Handle. Skipping render pass.");
            return;
        }

        UniversalResourceData resourceData = frameData.Get<UniversalResourceData>();
        UniversalCameraData cameraData = frameData.Get<UniversalCameraData>();

        if (resourceData.isActiveTargetBackBuffer || !resourceData.activeColorTexture.IsValid())
        {
            Debug.LogError($"Skipping render pass. BlitAndSwapColorRendererFeature requires an intermediate ColorTexture, we can't use the BackBuffer as a texture input.");
            return;
        }

        TextureHandle sourceCamColor = resourceData.activeColorTexture;

        ditherWorldGridTextureDescriptor.width = cameraData.cameraTargetDescriptor.width;
        ditherWorldGridTextureDescriptor.height = cameraData.cameraTargetDescriptor.height;

        if (ditherWorldGridTextureDescriptor.width <= 0 || ditherWorldGridTextureDescriptor.height <= 0)
        {
            Debug.LogWarning("Texture Descriptor width and/or height is zero");
            return;
        }

        TextureHandle dest = UniversalRenderer.CreateRenderGraphTexture
        (
            renderGraph,
            ditherWorldGridTextureDescriptor,
            temporaryTextureName,
            false
        );

        if (!dest.IsValid())
        {
            Debug.LogWarning("Failed to create temporary texture for Dither World Grid Pass");
            return;
        }

        if (!sourceCamColor.IsValid())
        {
            Debug.LogWarning("Required textures are invalid for outline effect!");
            return;
        }

        if (!ditherWorldGridTextureHandle.IsValid())
        {
            RTHandle rtHandle = RTHandles.Alloc(ditherWorldGridPassRTHandle);
            ditherWorldGridTextureHandle = renderGraph.ImportTexture(rtHandle);
        }

        if (!ditherWorldGridTextureHandle.IsValid())
        {
            Debug.LogWarning("Dither World Grid handle is invalid");
        }

        UpdateDitherWorldGridSettings();

        // First pass: Apply jpeg effect
        // Input: sourceCamColor (current frame) and dst (temp texture)
        // Output: tempTexture (jpeg compression result)
        RenderGraphUtils.BlitMaterialParameters ditheringEffectParams = new
        (
            source: sourceCamColor,
            destination: dest,
            material: material,
            shaderPass: 0 // Calculate dither world grid pass
        );
        renderGraph.AddBlitPass(ditheringEffectParams, k_CalculateDitherWorldGridPass);

        // Second pass: Copy jpeg compressed result back to sourceCamColor
        // Input: tempTexture (jpeg result)
        // Output: srcCamColor (camera color target for display)
        RenderGraphUtils.BlitMaterialParameters applyToCameraParams = new(
            source: dest,
            destination: sourceCamColor,
            material: material,
            shaderPass: 1 // Apply dither world grid pass
        );
        renderGraph.AddBlitPass(applyToCameraParams, k_ApplyDitherWorldGridPass);
    }

    public void SetDitherWorldRTHandle(RTHandle rtHandle)
    {
        ditherWorldGridPassRTHandle = rtHandle;
        ditherWorldGridTextureHandle = TextureHandle.nullHandle;
    }
    private void UpdateDitherWorldGridSettings()
    {
        if (material == null) return;

        DitherWorldGridVolumeComponent vc = VolumeManager.instance.stack.GetComponent<DitherWorldGridVolumeComponent>();

        float gridScale = vc != null && vc.gridScale.overrideState ? vc.gridScale.value : defaultSettings.gridScale;
        float gridFallOff = vc != null && vc.gridFallOff.overrideState ? vc.gridFallOff.value : defaultSettings.gridFallOff;
        float gridThickness = vc != null && vc.gridThickness.overrideState ? vc.gridThickness.value : defaultSettings.gridThickness;
        float sonarPingTime = vc != null && vc.sonarPingTime.overrideState ? vc.sonarPingTime.value : 0;
        Vector2 playerPos = vc != null && vc.playerPos.overrideState ? vc.playerPos.value : Vector2.zero;

        material.SetFloat(gridScaleID, gridScale);
        material.SetFloat(gridFallOffID, gridFallOff);
        material.SetFloat(gridThicknessID, gridThickness);
        material.SetFloat(sonarPingTimeID, sonarPingTime);
        material.SetVector(playerPosID, playerPos);
    }
}

[Serializable]
public class DefaultDitherWorldGridSettings
{
    public float gridScale = 10f;
    public float gridFallOff = 1f;
    public float gridThickness = 0.1f;
}


