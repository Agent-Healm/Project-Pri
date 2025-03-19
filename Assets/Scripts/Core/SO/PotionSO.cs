using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionSO", menuName = "SO/Decor/Potion")]
public class PotionSO : ScriptableObject {
    [field:SerializeField] public EffectAttribute[] effectAttributes {get; private set;}
    [field:SerializeField] public Sprite potionSprite {get; private set;}
    [field:SerializeField] public Color color {get; private set;} = Color.white;
}