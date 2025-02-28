using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [Tooltip("Furthest distance bullet will look for target")]
    public float maxDistance = 1000000;
    RaycastHit hit;
    
    public GameObject bloodEffect;
	
    public LayerMask ignoreLayer;

    /*
    * Upon bullet creation with this script attached,
    * the bullet creates a raycast which searches for colliders with corresponding materials.
    * If the raycast hits something with a matching material, it will create the corresponding effect.
    */
    void Update()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance, ~ignoreLayer))
        {
            EnemyAI enemy = hit.collider.GetComponent< EnemyAI > ();

            if (enemy != null)
            {
                MaterialType materialType = enemy.materialType;

                switch (materialType)
                {
                    case MaterialType.Enemy1:
                        SpawnDecal(hit, bloodEffect);
                        break;
                  
                    case MaterialType.Enemy2:
                        SpawnDecal(hit, bloodEffect);
                        break;
                    case MaterialType.Enemy3:
                        SpawnDecal(hit, bloodEffect);
                        break;

                    default:
                        break;
                }
            }

            Destroy(gameObject);
        }

        Destroy(gameObject, 0.1f);
    }

    void SpawnDecal(RaycastHit hit, GameObject prefab)
    {
        GameObject spawnedDecal = Instantiate(prefab, hit.point + hit.normal * 0.01f, Quaternion.LookRotation(hit.normal));
        spawnedDecal.transform.SetParent(hit.collider.transform);
    }
}