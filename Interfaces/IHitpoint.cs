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
    void ArmorRegeneration();
}

public interface IDamageAble
{
    void TakeDamage(int damage = 1);
    // void ArmorDamage(int damage = 1);
    // void HealthDamage(int damage = 1);
}