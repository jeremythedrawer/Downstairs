using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float currentHealth { get; private set; }
    private bool canLooseHealth = true;

    private void Start()
    {
        currentHealth = PlayerBrain.Instance.characterStats.health;
    }

    public void LooseHealth(Bounds playerBounds, LayerMask enemyLayer)
    {
        if (!canLooseHealth) return;

        Collider[] hits = Physics.OverlapBox(playerBounds.center, playerBounds.extents, Quaternion.identity, enemyLayer);

                Debug.Log(hits.Length);
        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy != null)
                {
                    StartCoroutine(LoosingHealth(enemy, hit.transform));
                    canLooseHealth = false;                
                }
                else
                {
                    Debug.LogWarning("Enemy tagged but doesnt have Enemy Script");
                }
            }
        }
    }
    private IEnumerator LoosingHealth(Enemy enemy, Transform enemyTransform)
    {
        currentHealth -= enemy.hitStrength;

        Vector3 knockBackDir = (transform.position - enemyTransform.position).normalized;
        float knockBackForce = 10f;
        float torqueForce = 50f;

        float randomDirection = Random.value > 0.5f ? 1f : -1f;
        Vector3 knockBackTorque = Vector3.forward * torqueForce * randomDirection; ;

        PlayerBrain.Instance.body.AddTorque(knockBackTorque, ForceMode.Impulse);
        PlayerBrain.Instance.body.AddForce(knockBackDir * knockBackForce, ForceMode.Impulse);
        PlayerBrain.Instance.playerMaterialController.hit = 1f;

        while (PlayerBrain.Instance.playerMaterialController.hit > 0)
        {
            PlayerBrain.Instance.playerMaterialController.hit -= Time.deltaTime;
            yield return null;
        }
        PlayerBrain.Instance.playerMaterialController.hit = 0f;
        canLooseHealth = true;
    }
}
