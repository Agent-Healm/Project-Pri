using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Potion : LootAbleItem, IInteractAble
{
    public enum Effects{
        None,
        Poison,
        Heal,
        Buff,
        Debuff
    }
    public PotionSO potion;
    // public PotionAttribute[] potionAttribute;
    // Start is called before the first frame update
    // void Start()
    // {
        
    // }

    // Update is called once per frame
    // void Update()
    // {
        
    // }

    public void OnPickup(PlayerAction playerAction){
        PlayerHitpoint _playerHitpoint = playerAction.GetComponent<PlayerHitpoint>();
        foreach (EffectAttribute ea in potion.effectAttributes){
            // switch (pa.effect){
            //     case Effects.None:
            //         break;
            //     case Effects.Poison:
            //         break;
            //     case Effects.Heal:
            //         Debug.Log("Player is healing");
            //         _playerHitpoint.HealHealth(pa.effectValue);
            //         break;
            //     case Effects.Buff:
            //         break;
            //     case Effects.Debuff:
            //         break;
            // }
            // switch (ea.effects){
                
            // }
            print(ea.effects.name);
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