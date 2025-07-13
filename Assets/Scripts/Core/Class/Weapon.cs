using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [System.Serializable]
public class Weapon : LootItem, IInteractAble
{
    [SerializeField] protected WeaponStatsSO baseWeaponStats;
    protected WeaponBaseAttributes _weaponAttr;
    protected int _currentWeaponMode = 0;
    protected PlayerWeaponAttackPattern _currentPwap;

    public int CurrentManaCost {
        get {
            return _currentPwap.EnergyCost;
        }
    }
    protected virtual void Awake()
    {
        _weaponAttr = baseWeaponStats.GetWeaponBaseAttributes;
        _currentPwap = _weaponAttr.pwap[_currentWeaponMode];
    }

    public void SwitchWeaponMode(){
        if (_weaponAttr.pwap.Length == 1){return;}

        _currentWeaponMode = (_currentWeaponMode + _weaponAttr.pwap.Length + 1) % _weaponAttr.pwap.Length;
        _currentPwap = _weaponAttr.pwap[_currentWeaponMode];
    }

    public virtual void Action(Vector2 direction, Vector2 position){
        
        float deg = Vector2.SignedAngle(Vector2.right, direction);
        deg += Random.Range(- _weaponAttr.inaccuracy, _weaponAttr.inaccuracy + 1) / 2f;
        Attack(deg, position);
    }

    protected void Attack(float deg, Vector2 position){
        float rad = deg * Mathf.Deg2Rad;
        Vector2 new_direction = new (Mathf.Cos(rad), Mathf.Sin(rad));
        
        _currentPwap.GetPattern.ShootBullet(new_direction, position, _currentPwap.CritChance);
    }

    public virtual void OnPickup(PlayerAction playerAction){
        PlayerWeaponSlot _playerWeaponSlot = playerAction.GetComponent<PlayerWeaponSlot>();
        // Debug.Log("Player picked up " + this);
        _playerWeaponSlot.AddToWeaponSlots(this);
        this.gameObject.SetActive(false);
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

    // public enum xWeaponType {
    //     Pistol,
    //     Rifle,
    //     Shotgun,
    //     Railgun,
    //     Launcher,
    //     Bow,
    //     Staff,
    //     Melee,
    //     Throwables,
    //     Misc
    // }
    // public enum xSubTypes {
    //     None,
    //     Shotgun
    // }

}