// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponRaritySO", menuName = "SO/Weapon/Rarity")]
public class WeaponRaritySO : ScriptableObject
{
    [SerializeField] private Color rarityColor = Color.white;
    
    public Color GetRarityColor { get => rarityColor; }
}
