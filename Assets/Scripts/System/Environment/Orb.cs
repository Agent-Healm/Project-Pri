using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : LootAbleItem
{
    // public Potion.Effects effect;
    // public int effectValue;

    public OrbSO orb;

    private EffectAttribute effectAttr;

    private void Awake()
    {
        effectAttr = orb.effectAttributes;
    }
    private Vector2 _velocity = Vector2.zero;
    private void OnTriggerStay2D(Collider2D other){
        // print("This orb is colliding with " + other.name);
        // if (other.gameObject.TryGetComponent<PlayerHitpoint>(out PlayerHitpoint playerHitpoint))
        if (other.gameObject.layer == 8)
        {
            this.transform.position = Vector2.SmoothDamp(this.transform.position, 
                                                        other.transform.position,
                                                        ref _velocity,
                                                        0.2f);  
            if (Vector2.Distance(this.transform.position, other.transform.position) < 1f){
                // switch(effect){
                //     case Potion.Effects.None :{
                //         break;
                //     }
                //     case Potion.Effects.Poison :{
                //         break;
                //     }
                //     case Potion.Effects.Heal :{
                //         // Debug.Log("Player collected HP orb");
                //         playerHitpoint.HealHealth(effectValue);
                //         break;
                //     }
                //     case Potion.Effects.Buff :{
                //         break;
                //     }
                //     case Potion.Effects.Debuff :{
                //         break;
                //     }
                // }
                switch (effectAttr.effects.name){
                    case "Heal HP":
                        Debug.Log("Player is healing with orb");
                        PlayerHitpoint _playerHitpoint = other.GetComponent<PlayerHitpoint>();
                        _playerHitpoint.HealHealth(effectAttr.effectValue);
                        break;
                    case "Restore Mana":
                        print("Player is restoring mana with orb");
                        PlayerMana _playerMana = other.GetComponent<PlayerMana>();
                        _playerMana.RestoreMana(effectAttr.effectValue);
                        break;
                }
                Destroy(this.gameObject);
            }
        }
    }
}