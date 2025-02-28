using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WeaponDamage : MonoBehaviour
{
    public Dictionary<WeaponType, Dictionary<MaterialType, int>> damageTable = new Dictionary<WeaponType, Dictionary<MaterialType, int>>();

    private void Start()
    {
        damageTable[WeaponType.Auto] = new Dictionary<MaterialType, int>
        {
            { MaterialType.Enemy1, 15 },
            { MaterialType.Enemy2, 10 },
            { MaterialType.Enemy3, 5 }
        };
    }

    public int GetDamage(WeaponType weaponType, MaterialType materialType)
    {
        if (damageTable.ContainsKey(weaponType) && damageTable[weaponType].ContainsKey(materialType))
        {
            return damageTable[weaponType][materialType];
        }
        return 0;
    }
}
