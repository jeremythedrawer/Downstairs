using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public BoxCollider boxCollider;
    public EnemyShaderController enemyShaderController;
    public float health;
    public float hitStrength;
    public float moveSpeed;

    [SerializeField] float materialFlickerTime = 0.05f;
    protected float currentHealth;

    private bool materialReadyToUpdate = true;

    private void OnEnable()
    {
        currentHealth = health;
        enemyShaderController.hit = false;
        materialReadyToUpdate = true;
    }
    protected virtual void Update()
    {
        Collider[] hits = Physics.OverlapBox(boxCollider.bounds.center, boxCollider.bounds.extents, Quaternion.identity, GameManager.instance.bulletLayer);

        if (hits.Length > 0)
        {
            foreach (Collider hit in hits)
            {
                Torpedo torpedo = hit.GetComponent<Torpedo>();

                if (materialReadyToUpdate)
                {
                    materialReadyToUpdate = false;
                    StartCoroutine(UpdatingMaterial());
                }
                currentHealth -= torpedo.hitPoints;
            }
        }
    }

    private IEnumerator UpdatingMaterial()
    {
        enemyShaderController.hit = true;
        yield return new WaitForSeconds(materialFlickerTime);
        enemyShaderController.hit = false;
        yield return new WaitForSeconds(materialFlickerTime);
        materialReadyToUpdate = true;
    }
}
