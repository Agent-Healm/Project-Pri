using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PistolScriptableObject", menuName = "ScriptableObjects/Pistol", order = 0)]
public class PistolScriptableObject : WeaponScriptableObject {
    // public WeaponBaseAttributes weaponAttribute;

    private void OnEnable() {
        base.weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Pistol;
    }
}
