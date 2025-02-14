using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IHealth, IDamageAble, ILootPool
{
    [field: SerializeField] public int maxHealthPoint {get; set;} = 10;
    private int _healthPoint;
    // Start is called before the first frame update
    void Start()
    {
        _healthPoint = maxHealthPoint;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void InflictDamage(int damage = 1){
        if (damage < 0){return;}
        _healthPoint -= damage;
        // Debug.Log("Crate durability : " + _healthPoint);
        if (_healthPoint <= 0){
            HealthAtZero();
        }
    }

    public void HealthAtZero(){
        // Debug.Log("Crate is destroyed");
        Destroy(this.gameObject);
        LootOnDeath();
    }

    public void LootOnDeath(){
        Debug.Log("Spawn a random potion on crate.");
    }
}
