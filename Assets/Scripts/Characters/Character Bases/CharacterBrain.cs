using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    public Rigidbody body;
    public BoxCollider boxCollider;

    public CharacterStats characterStats;
    public MovementController movementController;
    public HealthManager healthManager;
    public PlayerMaterialController playerMaterialController;
}
