using UnityEngine;

public abstract class CharacterBrain : MonoBehaviour
{
    public Rigidbody body;
    public BoxCollider boxCollider;
    public MeshFilter meshFilter;

    public CharacterStats characterStats;
    public MovementController movementController;
    public HealthManager healthManager;
    public PlayerMaterialController playerMaterialController;

    public Mesh noneMesh;
    public Mesh singleTorpedoMesh;
    public Mesh burstMesh;
    public Mesh doubleTorpedoMesh;
    public Mesh bombMesh;
    public Mesh lightPingMesh;
}
