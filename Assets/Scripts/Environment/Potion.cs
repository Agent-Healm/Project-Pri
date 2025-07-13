using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Potion : LootItem, IInteractAble
{
    [SerializeField] private PotionSO potion;

    public void OnPickup(PlayerAction l_playerAction){
        foreach (EffectAttribute ea in potion.GetEffectAttributes){
            ea.ApplyEffect(l_playerAction);
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