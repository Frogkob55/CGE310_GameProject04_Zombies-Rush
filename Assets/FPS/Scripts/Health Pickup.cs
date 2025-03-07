using System.Collections;
using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    public int healAmount = 25;
    public float respawnTime = 10f;

    private Vector3 initialPosition;
    private Collider pickupCollider;
    private Renderer pickupRenderer;

    private void Start()
    {
        initialPosition = transform.position;
        pickupCollider = GetComponent<Collider>();
        pickupRenderer = GetComponent<Renderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Heal(healAmount);
                StartCoroutine(RespawnHealth()); 

                
                pickupCollider.enabled = false;
                pickupRenderer.enabled = false;
            }
        }
    }

    private IEnumerator RespawnHealth()
    {
        yield return new WaitForSeconds(respawnTime); 

        
        transform.position = initialPosition;
        pickupCollider.enabled = true;
        pickupRenderer.enabled = true;
    }
}
