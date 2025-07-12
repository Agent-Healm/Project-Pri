using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;

public class Crate : MonoBehaviour, IHealth, IDamageAble, ILootPool
{
    [SerializeField] private int maxHealthPoint = 10;
    [SerializeField] private int maxLootCount = 1;
    [SerializeField] private LootTableSO lootTableSO;

    private int _healthPoint;
    private LootAbleItem[] _lootItem = {};

    // Start is called before the first frame update
    void Start()
    {
        _healthPoint = maxHealthPoint;
        lootTableSO.GenerateLoot(ref _lootItem, maxLootCount);
    }

    public bool InflictDamage(int l_damage = 0){
        if (l_damage < 0){return false;}
        if (_healthPoint <= 0){return false;}
        _healthPoint -= l_damage;
        if (_healthPoint <= 0){
            HealthAtZero();
        }
        return true;
    }

    public void HealthAtZero(){
        Destroy(this.gameObject);
        LootOnDestroy();
        this.enabled = false;
    }

    public void LootOnDestroy(){
        Debug.Log("Spawn a random loot on crate.");
        foreach(LootAbleItem item in _lootItem){
            item.SpawnLoot(this.transform.position);
        }
    }

}
