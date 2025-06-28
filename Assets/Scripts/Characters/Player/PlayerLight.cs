using UnityEngine;

public class PlayerLight : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    void Update()
    {
        Vector2 playerPos = playerTransform.position;
        transform.position = new Vector3(playerPos.x, playerPos.y, transform.position.z);
    }
}
