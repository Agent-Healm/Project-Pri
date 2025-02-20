using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [System.Serializable]
public class Pistol : Weapon
{
    // public PlayerWeaponAttackPattern[] pwap;
    // public WeaponBaseAttributes weaponBaseAttributes;

    // private int _currentWeaponMode = 0;
    // private PlayerWeaponAttackPattern _currentPwap;

    // void Awake() {
        // weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Pistol;
        // this.weaponType = WeaponBaseAttributes.WeaponType.Pistol;
        // _currentWeaponMode = 0;
        // _currentPwap = pwap[_currentWeaponMode];
        // base._currentPwap = pwap[_currentWeaponMode];
        // base.Awake();
    // }
    // Start is called before the first frame update
    // void Start()
    // {
        // if (weaponBaseAttributes.weaponType != WeaponBaseAttributes.WeaponType.Pistol){
        //     weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Pistol;
        // }

    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    // public int GetWeaponInaccuracy(){
    //     return this.inaccuracy;
        // return weaponBaseAttributes.inaccuracy
    // }

    // public void AttemptAttack(ref int manaPoint){
    //     if (manaPoint >= pwap[0].energyCost){
    //         // pwap[0].Attack();
    //     }
    //     manaPoint -= pwap[0].energyCost;
    // }
    // public void Attack(Vector2 direction, Vector2 position){
    //     pwap[0].Attack(direction, position);
    // }

    // public void SwitchWeaponMode(){
    //     if (pwap.Length == 1){return;}

    //     _currentWeaponMode = (_currentWeaponMode + pwap.Length + 1) % pwap.Length;
    //     _currentPwap = pwap[_currentWeaponMode];
    // }

    // public PlayerWeaponAttackPattern GetCurrentPwap(){
    //     return _currentPwap;
    // }

    // public void Action(){

    // }

}
