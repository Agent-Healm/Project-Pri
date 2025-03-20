using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : LootAbleItem
{

    public OrbSO orb;

    // private EffectAttribute effectAttr;

    // private void Awake()
    // {
    //     effectAttr = orb.effectAttributes;
    // }
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
                PlayerAction playerAction = other.GetComponent<PlayerAction>();
                orb.effectAttributes.ApplyEffect(playerAction);
                Destroy(this.gameObject);
            }
        }
    }
}