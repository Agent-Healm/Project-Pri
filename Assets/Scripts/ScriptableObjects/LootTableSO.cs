using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.ComponentModel;

[CreateAssetMenu(fileName="LootTableSO", menuName="SO/Misc/LootTable")]
public class LootTableSO : ScriptableObject
{
    [SerializeField] private Loot[] lootTable;
    [SerializeField] private float spawnChance;
    [SerializeField] private int totalWeight;

    private void OnValidate(){
        totalWeight = GetTotalWeight();
    }

    public void GenerateLoot(ref LootItem[] l_lootAbleItems, int l_maxLootCount){
        int l_randomValue;
        for (int i = 0; i < l_maxLootCount; i++){
            if (spawnChance < Random.Range(0, 100)){break;}
            l_randomValue = Random.Range(0, totalWeight);
            DetermineLoot(l_randomValue, ref l_lootAbleItems);
        }
    }
    private void DetermineLoot(int l_randomValue, ref LootItem[] l_lootAbleItems){
        foreach(Loot loot in lootTable){
            l_randomValue -= loot.weight;
            if (l_randomValue < 0){
                ArrayUtility.Add(ref l_lootAbleItems, loot.lootItem);
                break;
            }
        }
    }
    private int GetTotalWeight(){
        int l_totalWeight = 0;
        foreach(Loot loot in lootTable){
            l_totalWeight += loot.weight;
        }
        return l_totalWeight;
    }

}

[System.Serializable]
public class Loot
{
    public LootItem lootItem;
    public int weight;
}