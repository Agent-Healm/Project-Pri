using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public interface IHealth
{
    void HealthAtZero();
    // void HealthDamage(int damage = 1);
}

public interface IHealAble 
{
    void HealHealth(int heal = 1);
}

public interface IArmor
{
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
