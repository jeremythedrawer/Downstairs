using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float currentHealth { get; private set; }
    private bool canLooseHealth = true;
    public bool isHit => PlayerBrain.Instance.playerMaterialController.hit > 0f;
    private void Start()
    {
        currentHealth = PlayerBrain.Instance.characterStats.health;
    }

    public void LooseHealth(Bounds playerBounds, LayerMask enemyLayer)
    {
        if (!canLooseHealth) return;

        Collider[] hits = Physics.OverlapBox(playerBounds.center, playerBounds.extents, Quaternion.identity, enemyLayer);

        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                EnemyBase enemy = hit.GetComponent<EnemyBase>();
                Projectile enemyProjectile = hit.GetComponent<Projectile>();
                if (enemy != null)
                {
                    LooseHealthToEnemy(enemy);
                    canLooseHealth = false;                
                }
                else if (enemyProjectile != null)
                {
                    LooseHealthToProjectile(enemyProjectile);
                    canLooseHealth = false;
                }
                else
                {
                    Debug.LogWarning("Enemy tagged but doesnt have Enemy Script");

                }
            }
        }
    }
    private void LooseHealthToEnemy(EnemyBase enemy)
    {
        currentHealth -= enemy.hitStrength;
        StartCoroutine(LoosingHealth(enemy.transform));    
    }

    private void LooseHealthToProjectile(Projectile projectile)
    {
        currentHealth -= projectile.hitPoints;
        StartCoroutine(LoosingHealth(projectile.transform));
    }
    private IEnumerator LoosingHealth(Transform enemyTransform)
    {
        Vector3 knockBackDir = (transform.position - enemyTransform.position).normalized;
        float knockBackForce = 5f;
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
