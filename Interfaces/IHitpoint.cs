using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    int maxHealthPoint { get; set; }
    void HealthAtZero();
    // void HealthDamage(int damage = 1);
}

public interface IArmor
{
    int armorRegenStart { get; set; }
    int armorRegenInterval { get; set; }
    int maxArmorPoint { get; set; }
    void ArmorRegeneration();
    // void ArmorDamage(int damage = 1);
    void ArmorAtZero();
    
}

public interface IDamageAble
{
    void InflictDamage(int damage = 1);
}

public interface ILootPool
{
    void LootOnDeath();
}