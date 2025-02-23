using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour, IHealth, IDamageAble, ILootPool
{
    [field: SerializeField] public int maxHealthPoint {get; set;} = 10;

    [field: SerializeField] public int maxLootCount {get; set;} = 1;
    public LootTable lootTable;
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
        if (_healthPoint <= 0){
            HealthAtZero();
        }
    }

    public void HealthAtZero(){
        Destroy(this.gameObject);
        LootOnDeath();
    }

    public void LootOnDeath(){
        Debug.Log("Spawn a random loot on crate.");
        // Instantiate(potion[Random.Range(0, potion.Length)], this.transform.position, Quaternion.identity);
        lootTable.GenerateLoot(this.transform.position);
    }
}
