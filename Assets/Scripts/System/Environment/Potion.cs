using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Potion : LootAbleItem, IInteractAble
{
    public PotionSO potion;

    public void OnPickup(PlayerAction playerAction){
        foreach (EffectAttribute ea in potion.effectAttributes){
            ea.ApplyEffect(playerAction);
        }
        Destroy(this.gameObject);
    }
}

// [System.Serializable]
// public class PotionAttribute {
//     public Potion.Effects effect;
//     public EffectsSO effects;
//     public int effectValue;
// }