// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponRaritySO", menuName ="SO/Weapon/Rarity")]
public class WeaponRaritySO : ScriptableObject
{
    [field:SerializeField] public Color rarityColor {get; private set;} = Color.white;
}
