using System;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTypeSO", menuName = "SO/Weapon/Type")]
public class WeaponTypeSO : ScriptableObject
{
    [SerializeField] private Sprite weaponTypeIcon;

    public Sprite GetWeaponType {get => weaponTypeIcon;}
}
