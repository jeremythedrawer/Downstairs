using UnityEngine;
public class Jellyfish : SchoolFish<Jellyfish>
{
    private bool isMoving;
    private float currentTime;
    private float speed;
    public JellyfishSpawner jellyfishSpawner {  get; set; }

    protected override void OnEnable()
    {
        base.OnEnable();
    }
    private void Update()
    {
        if (distanceFromPlayer > 20f) return;
        UpdatePos();
        shaderController.speed = stats.linearSpeed;
    }


    private void UpdatePos()
    {
        currentTime += Time.deltaTime;
        float speedPattern = Mathf.Sin(currentTime) * 0.75f + 0.25f;
        speed = stats.linearSpeed * speedPattern;

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPos) < 0.2f)
            {
                isMoving = false;
            }
        }
        else
        {
            GetNewPos();
            isMoving = true;
        }
    }
}
