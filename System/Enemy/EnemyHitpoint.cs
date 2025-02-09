using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitpoint : MonoBehaviour, IHealth, IDamageAble, ILootPool
{
    public int maxHealthPoint = 1;
    private int _healthPoint;
    void Awake(){
        _healthPoint = maxHealthPoint;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InflictDamage(int damage = 0){
        if (damage <0){return;}
        _healthPoint -= damage;
        if (_healthPoint <=0) {
            HealthAtZero();
        }
    }

    public void HealthAtZero(){
        _healthPoint = 0;
        Destroy(this.gameObject);
        LootOnDeath();
    }

    public void LootOnDeath(){
        Debug.Log("Spawn coins and orbs");
    }
}