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
    public PotionAttribute[] potionAttribute;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPickup(PlayerAction playerAction){
        PlayerHitpoint _playerHitpoint = playerAction.GetComponent<PlayerHitpoint>();
        foreach (PotionAttribute pa in potionAttribute){
            switch (pa.effect){
                case Effects.None:
                    break;
                case Effects.Poison:
                    break;
                case Effects.Heal:
                    Debug.Log("Player is healing");
                    _playerHitpoint.HealHealth(pa.effectValue);
                    break;
                case Effects.Buff:
                    break;
                case Effects.Debuff:
                    break;
            }
        }
        Destroy(this.gameObject);
    }
}

[System.Serializable]
public class PotionAttribute {
    public Potion.Effects effect;
    public int effectValue;
}