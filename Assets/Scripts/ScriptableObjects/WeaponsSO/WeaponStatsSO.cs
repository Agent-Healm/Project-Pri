// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

public class WeaponStatsSO : ScriptableObject
{
    [SerializeField] private WeaponBaseAttributes weaponBaseAttributes;
    public WeaponBaseAttributes GetWeaponBaseAttributes { get => weaponBaseAttributes; }
}
