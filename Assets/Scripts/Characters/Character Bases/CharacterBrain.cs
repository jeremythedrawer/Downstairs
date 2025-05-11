using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    public Rigidbody body;
    public BoxCollider boxCollider;

    public MovementController movementController;
    public CharacterStats characterStats;
    public PlayerMaterialController playerMaterialController;
}
