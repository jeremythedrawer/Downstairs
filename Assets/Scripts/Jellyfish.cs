using System.Collections;
using UnityEngine;

public class Jellyfish : Enemy
{
    public float moveSpeed = 1f;
    private Vector3 target;
    private Camera cam;

    private bool isMoving;
    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, target) < 0.1f)
            {
                isMoving = false;
            }
        }
        else
        {
            GetNewPosition();
            isMoving = true;
        }
    }

    private void GetNewPosition()
    {
        float randomX = Random.Range(CameraController.minX, CameraController.maxX);
        float randomY = Random.Range(CameraController.minY, CameraController.maxY);

        target = new Vector3(randomX, randomX, transform.position.z);
    }
}
