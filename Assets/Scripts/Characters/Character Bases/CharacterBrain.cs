using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    public Rigidbody body;
    public CapsuleCollider capsuleCollider;
    public MeshFilter meshFilter;

    public CharacterStats characterStats;
    public MovementController movementController;
    public HealthManager healthManager;
    public PlayerMaterialController playerMaterialController;

    public MeshRenderer canonMesh;
    public MeshRenderer burstMesh;
    public MeshRenderer sonarPingMesh;
}
