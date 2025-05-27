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
}
