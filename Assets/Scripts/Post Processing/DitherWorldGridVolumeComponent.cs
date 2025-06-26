using System;
using UnityEngine;
using UnityEngine.Rendering;


[Serializable]
public class DitherWorldGridVolumeComponent : VolumeComponent
{
    public FloatParameter gridScale = new FloatParameter(10f);
    public FloatParameter gridThickness = new FloatParameter(0.1f);
    public FloatParameter gridFallOff = new FloatParameter(0.1f);
    public Vector2Parameter playerPos = new Vector2Parameter(Vector2.zero);

    public ClampedFloatParameter sonarPingTime = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter flareTime = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public Vector2Parameter flarePos = new Vector2Parameter(Vector2.zero);
    public ClampedFloatParameter radialScanTime = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public ClampedFloatParameter radialScanRotation = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);

}