using System.Collections;
using UnityEngine;

public class SeaUrchinSpike : Projectile
{
    public static float spikeSpeed = 2f;
    public SeaUrchin seaUrchin {  get; set; }
    public void ReleaseSpikes()
    {
        StartCoroutine(UpdateSpikes());
    }
    public void ResizeSpikes()
    {
        StartCoroutine(ResizingSpikes());
    }
    private IEnumerator UpdateSpikes()
    {
        Vector2 baseDir = transform.up;
        Vector2 direction = Quaternion.Euler(0, 0, 0) * baseDir;
        yield return new WaitUntil(() => seaUrchin != null);

        while (seaUrchin.shaderController.time < 1f && !hasHit)
        {
            transform.position += (Vector3)(direction * spikeSpeed * Time.deltaTime);
            yield return null;
        }
        transform.localPosition = new Vector3(0, 0, 0.1f);
        transform.localScale = Vector3.zero;
        hasHit = false;
    }
    private IEnumerator ResizingSpikes()
    {
        Vector3 targetScale = Vector3.one;
        while (transform.localScale.magnitude < targetScale.magnitude - 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
