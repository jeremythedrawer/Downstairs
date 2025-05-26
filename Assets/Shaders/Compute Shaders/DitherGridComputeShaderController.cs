using UnityEngine;

public class DitherGridComputeShaderController : MonoBehaviour
{
    public ComputeShader ditherGridCompute;
    public RenderTexture resultTex;

    public Vector2 playerPos;
    public float gridScale = 8f;
    public float sonarPingTime = 0;

    private int kernelHandle;

    private void Start()
    {
        kernelHandle = ditherGridCompute.FindKernel("CSMain");

        resultTex = new RenderTexture(Screen.width, Screen.height, 0);
        resultTex.enableRandomWrite = true;
        resultTex.Create();
    }


    private void Update()
    {
        ditherGridCompute.SetFloat("_GameTime", Time.time);
        ditherGridCompute.SetFloats("_PlayerPos", playerPos.x, playerPos.y);
        ditherGridCompute.SetFloat("_GridScale", gridScale);
        ditherGridCompute.SetFloat("_SonarPingTime", sonarPingTime);

        ditherGridCompute.SetTexture(kernelHandle, "ResultTexture", resultTex);

        ditherGridCompute.Dispatch(kernelHandle, Mathf.CeilToInt(Screen.width / 8f), Mathf.CeilToInt(Screen.height / 8f), 1);
    }
}
