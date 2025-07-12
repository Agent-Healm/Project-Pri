using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [System.Serializable]
public class Weapon : LootAbleItem, IInteractAble
{
    public WeaponStatsSO weaponSO;
    protected WeaponBaseAttributes _weaponAttr;
    protected int _currentWeaponMode = 0;
    protected PlayerWeaponAttackPattern _currentPwap ;

    protected virtual void Awake(){
        // this.gameObject.AddComponent<BoxCollider2D>();
        _weaponAttr = weaponSO.GetWeaponBaseAttributes;
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
        // this.
        Attack(deg, position);
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
    // public SubTypes subtypes;
    public WeaponTypeSO weaponType;
    public WeaponRaritySO weaponRarity;
    // public Effects effects;
    public int inaccuracy;
    public int speedModPct;
    public PlayerWeaponAttackPattern[] pwap;

    public enum xWeaponType {
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