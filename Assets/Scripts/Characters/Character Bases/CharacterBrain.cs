using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    public Rigidbody body;
    public SphereCollider sphereCollider;
    public MeshFilter meshFilter;

    public CharacterStats characterStats;
    public MovementController movementController;
    public HealthManager healthManager;
    public PlayerMaterialController playerMaterialController;
    public ArmController armController;

    public MeshRenderer canonMesh;
    public MeshRenderer burstMesh;
    public MeshRenderer sonarPingMesh;
}
