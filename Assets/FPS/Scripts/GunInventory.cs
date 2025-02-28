using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GunInventory : MonoBehaviour
{
    [Tooltip("Current weapon gameObject.")]
    public GameObject currentGun;
    private Animator currentHandsAnimator;

    [Tooltip("Weapon prefab from Resources folder.")]
    public string gunName = "MyGun";

    private void Awake()
    {
        SpawnWeapon();
    }

    private void SpawnWeapon()
    {
        if (currentGun) Destroy(currentGun);

        GameObject resource = Resources.Load<GameObject>(gunName);
        if (resource)
        {
            currentGun = Instantiate(resource, transform.position, Quaternion.identity);
            AssignHandsAnimator(currentGun);
        }
        else
        {
            Debug.LogError("Gun prefab not found in Resources: " + gunName);
        }
    }

    private void AssignHandsAnimator(GameObject _currentGun)
    {
        if (_currentGun.TryGetComponent(out GunScript gunScript))
        {
            currentHandsAnimator = gunScript.handsAnimator;
        }
    }

    public void DeadMethod()
    {
        if (currentGun) Destroy(currentGun);
        Destroy(this);
    }
}
