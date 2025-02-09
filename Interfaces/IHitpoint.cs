using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    void HealthAtZero(){
    }
}

public interface IArmor
{
    // int armorRegenStart { get; set; }
    // int armorRegenInterval { get; set; }
    // int maxArmorPoint { get; set; }
    void ArmorRegeneration();
    void ArmorAtZero();
    
}

public interface IDamageAble
{
    void InflictDamage(int damage = 1);
    // void ArmorDamage(int damage = 1);
    // void HealthDamage(int damage = 1);
}