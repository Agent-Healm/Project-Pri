using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Pistol : MonoBehaviour
{
    // [SerializeField] private 
    /* attackPattern[]
    public int damage;
    public int energyCost;
    public int critChance;
    public float roundsPerSec;
    */
    public PlayerWeaponAttackPattern[] pwap;

    /* weapon
    public enum weaponType;
    public enum subtypes;
    public enum rarity;
    public int inaccuracy;
    public int speedMod;
    public enum effects;
    */

    public WeaponBaseAttributes weaponBaseAttributes;

    private void Awake() {
        weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Pistol;
    }
    // Start is called before the first frame update
    void Start()
    {
        // if (weaponBaseAttributes.weaponType != WeaponBaseAttributes.WeaponType.Pistol)
        // {
        //     weaponBaseAttributes.weaponType = WeaponBaseAttributes.WeaponType.Pistol;
        // }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getWeaponInaccuracy(){
        return weaponBaseAttributes.inaccuracy;
    }

    // public void AttemptAttack(ref int manaPoint){
    //     if (manaPoint >= pwap[0].energyCost){
    //         // pwap[0].Attack();
    //     }
    //     manaPoint -= pwap[0].energyCost;
    // }
    // public void Attack(Vector2 direction, Vector2 position){
    //     pwap[0].Attack(direction, position);
    // }

}
