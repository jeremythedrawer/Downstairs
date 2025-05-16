using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public float linearSpeed;
    public float rotationSpeed;
    public float linearDamp;
    public float rotationDamp;
    public float health;

    [Header("Power Ups")]
    public float burstPower;


    [Header("Power Up Checks")]
    public bool canon;
    public bool burst;
    public bool sonarPing;
}
