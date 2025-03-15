using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    // public PlayerWeaponAttackPattern[] pwap;
    // public WeaponBaseAttributes weaponBaseAttributes;
    // [field:SerializeField] public int shotgunCount {get; set;} = 1;
    // public int shotgunSpread {get; set;}= 1;
    // [field:SerializeField] public ShotgunType shotgunType {get; set;} = ShotgunType.Wide;
    public enum ShotgunType {
        Wide,
        Scatter,
        Conical
    }
    
    private int _shotgunCount;
    private ShotgunType _shotgunType;

    protected override void Awake(){
        base.Awake();
        ShotgunSO _shotgunSO = (ShotgunSO)base.weaponSO;
        _shotgunCount = _shotgunSO.shotgunCount;
        _shotgunType = _shotgunSO.shotgunType;
    }

    public override void Action(Vector2 direction, Vector2 position){
        if (_shotgunType == ShotgunType.Wide){
            float shotgunSpread = (_weaponAttr.inaccuracy * 2f) / (_shotgunCount - 1);
            float deg = Vector2.SignedAngle(Vector2.right, direction);

            if (_shotgunCount % 2 == 1){
                this.Attack(deg, position);
            }

            for (int i = 1 + _shotgunCount % 2; i <= _shotgunCount; i+=2){
                this.Attack(deg + shotgunSpread * i / 2f, position);
                this.Attack(deg - shotgunSpread * i / 2f, position);
            }

        } 
        else if (_shotgunType == ShotgunType.Scatter){
            float deg = Vector2.SignedAngle(Vector2.right, direction);

            for (int i = 0; i < _shotgunCount; i++){
                // deg += (Random.Range(-inaccuracy, inaccuracy + 1) / 2f);
                this.Attack(deg + (Random.Range(- _weaponAttr.inaccuracy, _weaponAttr.inaccuracy + 1) / 2f), position);
            }
        }
        // else if (shotgunType == ShotgunType.Conical){

        // }
    }
}
