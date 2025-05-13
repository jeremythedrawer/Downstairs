using UnityEditor.Rendering.LookDev;
using UnityEngine;

public class Anglerfish : Enemy<Anglerfish>
{
    private Vector3 target;

    private bool isMoving;
    private float currentTime;

    protected override void Update()
    {
        base.Update();
        UpdatePos();
        Death(this, () => AnglerfishSpawner.anglerfishes.Remove(this));
    }

    private void UpdatePos()
    {
        currentTime += Time.deltaTime;
        float speedPattern = SpeedPattern(currentTime, 1f);
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
        Vector3 direction = PlayerBrain.Instance.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        target = PlayerBrain.Instance.transform.position;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }


    private float SpeedPattern(float time, float duration)
    {
        float t = Mathf.Clamp01(time / duration);
        return t * t * t;
    }
}
