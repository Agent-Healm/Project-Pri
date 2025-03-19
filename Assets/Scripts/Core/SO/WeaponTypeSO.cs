using UnityEngine;

[CreateAssetMenu(fileName = "WeaponTypeSO", menuName ="SO/Weapon/Type")]
public class WeaponTypeSO : ScriptableObject
{
    [field:SerializeField] public Sprite weaponTypeIcon {get; private set;}
}
