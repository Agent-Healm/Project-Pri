using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PistolSO", menuName = "ScriptableObjects/Pistol", order = 0)]
public class PistolSO : WeaponSO {
    // public WeaponBaseAttributes weaponAttribute;

    private void OnEnable() {
        base.weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Pistol;
    }
}
