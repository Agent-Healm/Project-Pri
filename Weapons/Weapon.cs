using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [System.Serializable]
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
    // public AttackPattern attackPattern;
    public WeaponType weaponType;
    public SubTypes subtypes;
    public WeaponRarity weaponRarity;
    public Effects effects;
    public int inaccuracy;
    public int speedModPct;

    public PlayerWeaponAttackPattern[] pwap;
    // public WeaponBaseAttributes weaponBaseAttributes;

    protected int _currentWeaponMode = 0;
    protected PlayerWeaponAttackPattern _currentPwap;

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

    protected void Awake(){
        // this.gameObject.AddComponent<BoxCollider2D>();
        _currentPwap = pwap[_currentWeaponMode];
    }

    public void SwitchWeaponMode(){
        if (pwap.Length == 1){return;}

        _currentWeaponMode = (_currentWeaponMode + pwap.Length + 1) % pwap.Length;
        _currentPwap = pwap[_currentWeaponMode];
    }

    public virtual bool AttemptAction(ref int manaPoint){
        int energyCost = _currentPwap.energyCost;
        if (manaPoint >= energyCost){
            manaPoint -= energyCost;
            return true;
        }
        return false;
    }

    public virtual void Action(Vector2 direction, Vector2 position){
        // _currentPwap.ap_Attack(direction, position, this.inaccuracy);
        
        float deg = Vector2.SignedAngle(Vector2.right, direction);
        deg += (Random.Range(-inaccuracy, inaccuracy + 1) / 2f);
        this.Attack(deg, position);
    }

    // public void VAttack(Vector2 direction, Vector2 position){
    //     _currentPwap.attackPattern.ShootBullet(direction, position, _currentPwap.critChance);
    // }

    public void Attack(float deg, Vector2 position){
        float rad = deg * Mathf.Deg2Rad;
        Vector2 new_direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        
        _currentPwap.attackPattern.ShootBullet(new_direction, position, _currentPwap.critChance);
    }
    
    // public void OnInteract(){
    //     Debug.Log("Player picked up " + this.name);
        
    //     Destroy(this.gameObject);
    // }
}

// [System.Serializable]
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