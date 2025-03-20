using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IHealth
{
    int maxHealthPoint { get; set; }
    void HealthAtZero();
    // void HealthDamage(int damage = 1);
}

public interface IHealAble 
{
    void HealHealth(int heal = 1);
}

public interface IArmor
{
    float armorRegenStart { get; set; }
    float armorRegenInterval { get; set; }
    int maxArmorPoint { get; set; }
    // void ArmorDamage(int damage = 1);
    void ArmorAtZero();
    
}

public interface IDamageAble
{
    bool InflictDamage(int damage = 1);
}

public interface ILootPool
{
    void LootOnDestroy();
}
