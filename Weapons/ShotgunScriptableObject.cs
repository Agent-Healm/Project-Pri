using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "ShotgunScriptableObject", menuName = "ScriptableObjects/Shotgun", order = 0)]
public class ShotgunScriptableObject : WeaponScriptableObject {

    // [Header("Weapon Base Stats")]
    // public WeaponBaseAttributes weaponBaseAttributes;
    // [Header("Shotgun properties")]
    [field:SerializeField] public int shotgunCount {get; set;} = 1;
    // public int shotgunSpread {get; set;}= 1;
    [field:SerializeField] public Shotgun.ShotgunType shotgunType {get; set;} = Shotgun.ShotgunType.Wide;
    // public enum ShotgunType {
    //     Wide,
    //     Scatter,
    //     Conical
    // }
    
    private void OnEnable(){
        base.weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Shotgun;
    }
    // public void getShotgunCount(){
    //     Debug.Log("Shotgun Count = " + shotgunCount);
    // }
}
