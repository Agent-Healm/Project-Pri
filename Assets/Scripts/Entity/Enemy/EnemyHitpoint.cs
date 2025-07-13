using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitpoint : MonoBehaviour, IHealth, IDamageAble, ILootPool
{
    [SerializeField] private int maxHealthPoint = 8;
    private int _healthPoint;
    void Awake(){
        _healthPoint = maxHealthPoint;
    }

    public bool InflictDamage(int damage = 0){
        if (damage < 0){return false;}
        _healthPoint -= damage;
        if (_healthPoint <=0) {
            HealthAtZero();
        }
        return true;
    }

    public void HealthAtZero(){
        _healthPoint = 0;
        Destroy(this.gameObject);
        LootOnDestroy();
    }

    public void LootOnDestroy(){
        Debug.Log("Spawn coins and orbs");
    }
}