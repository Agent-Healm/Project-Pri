using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public Potion.Effects effect;
    public int effectValue;

    private Vector2 _velocity = Vector2.zero;
    // Start is called before the first frame update
    // private void OnTriggerEnter2D(Collider2D other){
    //     if (other.gameObject.TryGetComponent<PlayerHitpoint>(out PlayerHitpoint playerHitpoint)){
            // switch(effect){
            //     case Potion.Effects.None :{
            //         break;
            //     }
            //     case Potion.Effects.Poison :{
            //         break;
            //     }
            //     case Potion.Effects.Heal :{
            //         Debug.Log("Player collected HP orb");
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
            // Destroy(this.gameObject);
    //     }
    // }

    private void OnTriggerStay2D(Collider2D other){
        if (other.gameObject.TryGetComponent<PlayerHitpoint>(out PlayerHitpoint playerHitpoint)){
            // Debug.Log("Player collected HP orb");
            // this.transform.position = Vector2.MoveTowards(this.transform.position, other.transform.position, 2f);
            // this.transform.position = Vector2.LerpUnclamped(this.transform.position, other.transform.position, 1f);
            this.transform.position = Vector2.SmoothDamp(this.transform.position, 
                                                        other.transform.position,
                                                        ref _velocity,
                                                        0.2f);  
            if (Vector2.Distance(this.transform.position, other.transform.position) < 1f){
                // Debug.Log("Collected hp orb");
                // Destroy(this.gameObject);
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