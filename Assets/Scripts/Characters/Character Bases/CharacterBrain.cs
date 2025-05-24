using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    public Rigidbody body;
    public SphereCollider sphereCollider;

    public CharacterStats characterStats;
    public MovementController movementController;
    public HealthManager healthManager;
    public PlayerMaterialController playerMaterialController;
    public PlayerLightController lightController;
    public AudioSource audioSource;
}
