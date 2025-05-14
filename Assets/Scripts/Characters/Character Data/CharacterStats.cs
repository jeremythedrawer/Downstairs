using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public float runSpeed;
    public float walkSpeed;
    public float linearDamp;
    public float rotationDamp;
    public float health;

    [Header("Power Ups")]
    public float burstPower;


    [Header("Power Up Checks")]
    public bool singleTorpedo;
    public bool burst;
    public bool lightPing;
    public bool doubleTorpedo;
    public bool bomb;
}
