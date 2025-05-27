using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Basic Stats")]
    public float linearSpeed;
    public float rotationSpeed;
    public float linearDamp;
    public float rotationDamp;
    public float health;


    [Header("Power Up Checks")]
    public bool canSonarPing;
    public bool canFlare;
    public bool canRadialScan;
}
