using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    public Rigidbody2D body;
    public BoxCollider2D boxCollider;
    public SpriteRenderer spriteRenderer;

    public MovementController movementController;
    public CollisionChecker collisionChecker;
    public AnimationManager animationManager;
    public CharacterStats characterStats;
}
