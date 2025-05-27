using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    public Rigidbody body;
    public SphereCollider sphereCollider;

    public CharacterStats characterStats;
    public MovementController movementController;
    public PlayerMaterialController playerMaterialController;
    public PlayerLightController lightController;
    public AudioManager audioManager;
}
