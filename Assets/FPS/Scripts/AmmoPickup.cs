using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoPickup : MonoBehaviour
{
    [Tooltip("Amount of bullets this pickup provides.")]
    public float ammoAmount = 10f;

    private void OnTriggerEnter(Collider other)
    {
        GunInventory gunInventory = other.GetComponentInChildren<GunInventory>();
        if (gunInventory != null && gunInventory.currentGun != null)
        {
            GunScript gunScript = gunInventory.currentGun.GetComponent<GunScript>();
            if (gunScript != null)
            {
                gunScript.AddBullets(ammoAmount);
                Destroy(gameObject);
            }
        }
    }
}
