using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName="LootTableSO", menuName="SO/Misc/LootTable")]
public class LootTableSO : ScriptableObject
{
    public Loot[] lootTable2;
    public float spawnChance;
    public int totalWeight;

    private void OnValidate(){
        totalWeight = GetTotalWeight();
    }

    public void GenerateLoot(ref LootAbleItem[] lootAbleItems, int maxLootCount){
        int randomValue;
        for (int i = 0; i < maxLootCount; i++){
            if (spawnChance < Random.Range(0, 100)){break;}
            randomValue = Random.Range(0, totalWeight);
            DetermineLoot(randomValue, ref lootAbleItems);
        }
    }
    private void DetermineLoot(int randomValue, ref LootAbleItem[] lootAbleItems){
        foreach(Loot loot in lootTable2){
            randomValue -= loot.weight;
            if (randomValue < 0){
                ArrayUtility.Add(ref lootAbleItems, loot.lootItem);
                break;
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
    public LootAbleItem lootItem;
    public int weight;
}