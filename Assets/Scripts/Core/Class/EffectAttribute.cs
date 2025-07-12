using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EffectAttribute {
    [SerializeField] private EffectsSO effects;
    [SerializeField] private int effectValue;
    
    public void ApplyEffect(PlayerAction l_playerAction){
        if (effects == null) {
            Debug.Log("No effect applied");
            return;
        }
        switch (effects?.name){
            case "Heal HP":
                Debug.Log("Player is healing");
                PlayerHitpoint _playerHitpoint = l_playerAction.GetComponent<PlayerHitpoint>();
                _playerHitpoint.HealHealth(effectValue);
                break;
            case "Restore Mana":
                Debug.Log("Player is restoring mana");
                PlayerMana _playerMana = l_playerAction.GetComponent<PlayerMana>();
                _playerMana.RestoreMana(effectValue);
                break;
            case "" :
                Debug.Log("unknown effect");
                break;
        }
    }
}