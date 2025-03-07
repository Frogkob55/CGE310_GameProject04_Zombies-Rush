using System.Collections;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Tooltip("Amount of bullets this pickup provides.")]
    public float ammoAmount = 10f;
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
        GunInventory gunInventory = other.GetComponentInChildren<GunInventory>();
        if (gunInventory != null && gunInventory.currentGun != null)
        {
            GunScript gunScript = gunInventory.currentGun.GetComponent<GunScript>();
            if (gunScript != null)
            {
                gunScript.AddBullets(ammoAmount);
                StartCoroutine(RespawnAmmo()); 

                
                pickupCollider.enabled = false;
                pickupRenderer.enabled = false;
            }
        }
    }

    private IEnumerator RespawnAmmo()
    {
        yield return new WaitForSeconds(respawnTime); 

        
        transform.position = initialPosition;
        pickupCollider.enabled = true;
        pickupRenderer.enabled = true;
    }
}
