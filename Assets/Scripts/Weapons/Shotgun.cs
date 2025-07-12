using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public enum Category {
        Wide,
        Scatter,
        Conical
    }
    
    private int _shotgunCount;
    private Category _shotgunType;

    protected override void Awake(){
        base.Awake();
        ShotgunSO _shotgunSO = base.weaponSO as ShotgunSO;
        _shotgunCount = _shotgunSO.GetShotgunCount;
        _shotgunType = _shotgunSO.GetShotgunType;
    }

    public override void Action(Vector2 l_direction, Vector2 l_position){
        if (_shotgunType == Category.Wide){
            float l_shotgunSpread = _weaponAttr.inaccuracy * 2f / (_shotgunCount - 1);
            float l_deg = Vector2.SignedAngle(Vector2.right, l_direction);

            if (_shotgunCount % 2 == 1){
                this.Attack(l_deg, l_position);
            }

            for (int i = 1 + _shotgunCount % 2; i <= _shotgunCount; i+=2){
                this.Attack(l_deg + l_shotgunSpread * i / 2f, l_position);
                this.Attack(l_deg - l_shotgunSpread * i / 2f, l_position);
            }

        } 
        else if (_shotgunType == Category.Scatter){
            float deg = Vector2.SignedAngle(Vector2.right, l_direction);

            for (int i = 0; i < _shotgunCount; i++){
                // deg += (Random.Range(-inaccuracy, inaccuracy + 1) / 2f);
                this.Attack(deg + (Random.Range(- _weaponAttr.inaccuracy, _weaponAttr.inaccuracy + 1) / 2f), l_position);
            }
        }
        // else if (shotgunType == ShotgunType.Conical){

        // }
    }
}
