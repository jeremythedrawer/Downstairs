using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private float enemyKnockBackForce = 5f;
    [SerializeField] private float torqueForce = 50f;
    public float currentHealth { get; private set; }
    private bool canLooseHealth = true;
    public bool isHit => PlayerBrain.Instance.playerMaterialController.hit > 0f;

    private void Start()
    {
        currentHealth = PlayerBrain.Instance.characterStats.health;
    }

    public void LooseHealth(Bounds playerBounds, LayerMask hitLayer)
    {
        if (!canLooseHealth) return;

        Collider[] hits = Physics.OverlapBox(playerBounds.center, playerBounds.extents, Quaternion.identity, hitLayer);

        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                switch(hit.tag)
                {
                    case "Enemy":
                    {
                        EnemyBase enemy = hit.GetComponent<EnemyBase>();
                        if (enemy != null)
                        {
                            Vector3 knockBackDir = (transform.position - enemy.transform.position).normalized;
                            StartCoroutine(LoosingHealth(enemy.hitPoints, enemyKnockBackForce, knockBackDir));
                                          
                        }
                        else
                        {
                            Debug.LogWarning(hit.gameObject.name + "Doesnt have an EnemyBase Script attached");
                        }
                    }
                    break;

                    case "Projectile":
                    {
                        Projectile enemyProjectile = hit.GetComponent<Projectile>();
                        if (enemyProjectile != null)
                        {
                            Vector3 knockBackDir = (transform.position - enemyProjectile.transform.position).normalized;
                            StartCoroutine(LoosingHealth(enemyProjectile.hitPoints, enemyKnockBackForce, knockBackDir));
                        }
                        else
                        {
                            Debug.LogWarning(hit.gameObject.name + "Doesnt have an Projectile Script attached");
                        }
                    }
                    break;

                    default:
                    {
                        return;
                    }
                }
            }
        }
    }
    private IEnumerator LoosingHealth(float hitPoints, float knockBackForce, Vector3 knockBackDir)
    {
        canLooseHealth = false;
        currentHealth -= hitPoints;

        float randomDirection = Random.value > 0.5f ? 1f : -1f;
        Vector3 knockBackTorque = Vector3.forward * torqueForce * randomDirection;

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
