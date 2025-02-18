using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    // public PlayerWeaponAttackPattern[] pwap;
    // public WeaponBaseAttributes weaponBaseAttributes;
    [field:SerializeField] public int shotgunCount {get; set;} = 1;
    // public int shotgunSpread {get; set;}= 1;
    [field:SerializeField] public ShotgunType shotgunType {get; set;} = ShotgunType.Wide;
    public enum ShotgunType {
        Wide,
        Scatter,
        Conical
    }
    
    // void Awake(){
    //     base._currentPwap = pwap[_currentWeaponMode];
    // }

    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    public override void Action(Vector2 direction, Vector2 position){
        if (shotgunType == ShotgunType.Wide){
            float shotgunSpread = (inaccuracy * 2f) / (shotgunCount - 1);
            float deg = Vector2.SignedAngle(Vector2.right, direction);

            if (shotgunCount % 2 == 1){
                this.Attack(deg, position);
            }

            for (int i = 1 + shotgunCount % 2; i <= shotgunCount; i+=2){
                this.Attack(deg + shotgunSpread * i / 2f, position);
                this.Attack(deg - shotgunSpread * i / 2f, position);
            }

        } 
        else if (shotgunType == ShotgunType.Scatter){
            float deg = Vector2.SignedAngle(Vector2.right, direction);

            for (int i = 0; i < shotgunCount; i++){
                // deg += (Random.Range(-inaccuracy, inaccuracy + 1) / 2f);
                this.Attack(deg + (Random.Range(-inaccuracy, inaccuracy + 1) / 2f), position);
            }
        } 
        // else if (shotgunType == ShotgunType.Conical){

        // }
    }
}
