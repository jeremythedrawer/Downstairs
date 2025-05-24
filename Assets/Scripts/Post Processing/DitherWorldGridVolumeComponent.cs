using System;
using UnityEngine;
using UnityEngine.Rendering;


[Serializable]
public class DitherWorldGridVolumeComponent : VolumeComponent
{
    public FloatParameter gridScale = new FloatParameter(10f);
    public FloatParameter gridFallOff = new FloatParameter(1f);
    public FloatParameter gridThickness = new FloatParameter(0.1f);
    public ClampedFloatParameter sonarPingTime = new ClampedFloatParameter(0.0f, 0.0f, 1.0f);
    public Vector2Parameter playerPos = new Vector2Parameter(Vector2.zero);
}