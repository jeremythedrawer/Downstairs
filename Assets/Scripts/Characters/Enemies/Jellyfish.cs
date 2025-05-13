using UnityEngine;
public class Jellyfish : Enemy<Jellyfish>
{
    private Vector3 target;

    private bool isMoving;
    private float currentTime;

    protected override void Update()
    {
        base.Update();
        UpdatePos();
        Death(this, () => JellyfishSpawner.jellyfishes.Remove(this));
    }


    private void UpdatePos()
    {
        currentTime += Time.deltaTime;
        float speedPattern = SpeedPattern(currentTime, 0.1f);
        float speed = moveSpeed * speedPattern;

        if (isMoving)
        {

            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

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

        target = new Vector3(randomX, randomY, transform.position.z);
    }

    private float SpeedPattern(float t, float frequency)
    {
        float phase = (t * frequency) % 1f;

        if (phase < 0.75f)
        {
            return Mathf.Sin(Mathf.PI * (phase / 0.75f));
        }
        else
        {
            return Mathf.Sin(Mathf.PI + Mathf.PI * ((phase - 0.75f) / 0.25f));
        }
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Debug.DrawLine(transform.position, target, Color.magenta);
    }
}
