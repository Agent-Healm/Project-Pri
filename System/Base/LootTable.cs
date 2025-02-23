using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="LootTable", menuName="ScriptableObjects/LootTable")]
public class LootTable : ScriptableObject
{
    public Loot[] lootTable2;
    public float spawnChance;
    public int totalWeight;
    public bool isCalculated = false;

    public void GenerateLoot(Vector2 position){
        if (!isCalculated){
            totalWeight = GetTotalWeight();
            isCalculated = true;
        }
        
        // int randomValue = Random.Range(0, totalWeight);
        int randomValue = Random.Range(0, totalWeight);
        Debug.Log("set random value : " + randomValue);
        foreach(Loot loot in lootTable2){
            randomValue -= loot.weight;
            if (randomValue < 0){
                Debug.Log("spawn loot : " + loot.loot.name);
                loot.loot.SpawnLoot(position);
                return;
            }
        }
    }
    private int GetTotalWeight(){
        int totalWeight = 0;
        foreach(Loot loot in lootTable2){
            totalWeight += loot.weight;
        }
        return totalWeight;
    }
}

[System.Serializable]
public class Loot
{
    public LootAbleItem loot;
    public int weight;
}