using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.InputSystem.Interactions;

[CreateAssetMenu(fileName = "ShotgunSO", menuName = "SO/Weapon/Shotgun", order = 0)]
public class ShotgunSO : WeaponStatsSO {

    // [Header("Weapon Base Stats")]
    // public WeaponBaseAttributes weaponBaseAttributes;
    // [Header("Shotgun properties")]
    [SerializeField] private int shotgunCount = 1;
    public int GetShotgunCount { get => shotgunCount; }
    [SerializeField] private Shotgun.Category shotgunType = Shotgun.Category.Wide;
    public Shotgun.Category GetShotgunType { get => shotgunType; }
    
    private void OnEnable(){
        // base.weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Shotgun;
    }
    // public void getShotgunCount(){
    //     Debug.Log("Shotgun Count = " + shotgunCount);
    // }

}
