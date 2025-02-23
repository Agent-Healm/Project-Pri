using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : LootAbleItem
{
    public Potion.Effects effect;
    public int effectValue;

    private Vector2 _velocity = Vector2.zero;
    private void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.TryGetComponent<PlayerHitpoint>(out PlayerHitpoint playerHitpoint)){
            this.transform.position = Vector2.SmoothDamp(this.transform.position, 
                                                        other.transform.position,
                                                        ref _velocity,
                                                        0.2f);  
            if (Vector2.Distance(this.transform.position, other.transform.position) < 1f){
                switch(effect){
                    case Potion.Effects.None :{
                        break;
                    }
                    case Potion.Effects.Poison :{
                        break;
                    }
                    case Potion.Effects.Heal :{
                        // Debug.Log("Player collected HP orb");
                        playerHitpoint.HealHealth(effectValue);
                        break;
                    }
                    case Potion.Effects.Buff :{
                        break;
                    }
                    case Potion.Effects.Debuff :{
                        break;
                    }
                }
                Destroy(this.gameObject);
            }
        }
    }
}