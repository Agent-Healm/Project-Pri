using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public PlayerWeaponAttackPattern[] pwap;
    public WeaponBaseAttributes weaponBaseAttributes;
    private void Awake(){
        weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Shotgun;
    }
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    
    public int getWeaponInaccuracy(){
        return weaponBaseAttributes.inaccuracy;
    }
}
