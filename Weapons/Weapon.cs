using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class Weapon : MonoBehaviour
{
    // [SerializeField] private 
    /* attackPattern[]
    public int damage;
    public int energyCost;
    public int critChance;
    public float roundsPerSec;
    */
    /*
    public enum weaponType;
    public enum subtypes;
    public enum rarity;
    public int inaccuracy;
    public int speedModPct;
    public enum effects;
    */
    public AttackPattern attackPattern;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Vector2 direction, Vector2 position){
        attackPattern.ShootBullet(direction, position);
    }

}

[System.Serializable]
public class WeaponBaseAttributes {
    public WeaponType weaponType;
    public SubTypes subtypes;
    public WeaponRarity weaponRarity;
    public Effects effects;
    public int inaccuracy;
    public int speedModPct;

    public enum WeaponType {
        Pistol,
        Rifle,
        Shotgun,
        Railgun,
        Launcher,
        Bow,
        Staff,
        Melee,
        Throwables,
        Misc
    }
    public enum WeaponRarity {
        Common,
        Uncommon,
        Rare,
        VeryRare,
        Epic,
        Legendary
    }
    public enum Effects {
        None,
        Poison,
        Burn,
        Freeze
    }
    public enum SubTypes {
        None,
        Shotgun
    }

}