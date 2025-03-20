// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "EffectsSO", menuName = "SO/Misc/Effects")]
public class EffectsSO : ScriptableObject
{
    // public string effectName;
    // public UnityEvent _effect;
    public Sprite effectIcon;
    
}

[System.Serializable]
public class EffectAttribute {
    public EffectsSO effects;
    public int effectValue;
    
    public void ApplyEffect(PlayerAction playerAction){
        if (effects == null) {
            Debug.Log("No effect applied");
            return;
        }
        switch (effects?.name){
            case "Heal HP":
                Debug.Log("Player is healing");
                PlayerHitpoint _playerHitpoint = playerAction.GetComponent<PlayerHitpoint>();
                _playerHitpoint.HealHealth(effectValue);
                break;
            case "Restore Mana":
                Debug.Log("Player is restoring mana");
                PlayerMana _playerMana = playerAction.GetComponent<PlayerMana>();
                _playerMana.RestoreMana(effectValue);
                break;
            case "" :
                Debug.Log("unknown effect");
                break;
        }
    }
}