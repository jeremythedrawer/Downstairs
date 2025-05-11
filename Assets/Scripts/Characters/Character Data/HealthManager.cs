using System.Collections;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private float currentHealth;
    private bool canLooseHealth = true;

    private void Start()
    {
        currentHealth = PlayerBrain.Instance.characterStats.health;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy") && canLooseHealth)
        {
        Debug.Log("loosing health");
            Enemy enemy = other.gameObject.GetComponent<Enemy>();

            if (enemy != null)
            {
                StartCoroutine(LoosingHealth(enemy, other.transform));
                canLooseHealth = false;
            }
            else
            {
                Debug.LogWarning("Enemy tagged but doesnt have Enemy Script");
            }
        }
    }

    private IEnumerator LoosingHealth(Enemy enemy, Transform enemyTransform)
    {
        currentHealth -= enemy.hitStrength;

        Vector2 knockBackDir = (transform.position - enemyTransform.position).normalized;
        float knockBackForce = 10f;

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
