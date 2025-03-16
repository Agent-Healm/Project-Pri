using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [System.Serializable]
public abstract class Weapon : LootAbleItem, IInteractAble
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
    // public AttackPattern attackPattern;
    // public WeaponType weaponType;
    // public SubTypes subtypes;
    // public WeaponRarity weaponRarity;
    // public Effects effects;
    // public int inaccuracy;
    // public int speedModPct;

    // public PlayerWeaponAttackPattern[] pwap;
    // public WeaponBaseAttributes weaponBaseAttributes;
    public WeaponSO weaponSO;

    protected WeaponBaseAttributes _weaponAttr;
    protected int _currentWeaponMode = 0;
    protected PlayerWeaponAttackPattern _currentPwap ;
    // public enum xWeaponType {
        // Pistol,
        // Shotgun,
        // Rifle,
        // Railgun,
        // Launcher,
        // Bow,
        // Staff,
        // Melee,
        // Throwables,
        // Misc
    // }
    // public enum xWeaponRarity {
    //     Common,
    //     Uncommon,
    //     Rare,
    //     VeryRare,
    //     Epic,
    //     Legendary
    // }
    // public enum xEffects {
    //     None,
    //     Poison,
    //     Burn,
    //     Freeze
    // }
    // public enum SubTypes {
    //     None,
    //     Shotgun
    // }

    protected virtual void Awake(){
        // this.gameObject.AddComponent<BoxCollider2D>();
        _weaponAttr = weaponSO.weaponBaseAttributes;
        _currentPwap = _weaponAttr.pwap[_currentWeaponMode];
    }

    // void Start(){

    // }

    public void SwitchWeaponMode(){
        if (_weaponAttr.pwap.Length == 1){return;}

        _currentWeaponMode = (_currentWeaponMode + _weaponAttr.pwap.Length + 1) % _weaponAttr.pwap.Length;
        _currentPwap = _weaponAttr.pwap[_currentWeaponMode];
    }

    public virtual void Action(Vector2 direction, Vector2 position){
        
        float deg = Vector2.SignedAngle(Vector2.right, direction);
        deg += (Random.Range(- _weaponAttr.inaccuracy, _weaponAttr.inaccuracy + 1) / 2f);
        this.Attack(deg, position);
    }

    protected void Attack(float deg, Vector2 position){
        float rad = deg * Mathf.Deg2Rad;
        Vector2 new_direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        
        _currentPwap.attackPattern.ShootBullet(new_direction, position, _currentPwap.critChance);
    }

    public virtual void OnPickup(PlayerAction playerAction){
        PlayerWeaponSlot _playerWeaponSlot = playerAction.GetComponent<PlayerWeaponSlot>();
        // Debug.Log("Player picked up " + this);
        _playerWeaponSlot.AddToWeaponSlots(this);
        this.gameObject.SetActive(false);
    }

    public int getManaCost(){
        return _currentPwap.energyCost;
    }
}

[System.Serializable]
public class WeaponBaseAttributes {
    public WeaponType weaponType;
    // public SubTypes subtypes;
    // public WeaponRarity weaponRarity;
    public WeaponRaritySO weaponRarity;
    // public Effects effects;
    public int inaccuracy;
    public int speedModPct;
    public PlayerWeaponAttackPattern[] pwap;

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
    public enum xSubTypes {
        None,
        Shotgun
    }

}