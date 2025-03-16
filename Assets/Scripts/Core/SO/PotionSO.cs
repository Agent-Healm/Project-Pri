using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionSO", menuName = "SO/Potion")]
public class PotionSO : ScriptableObject {
    public EffectAttribute[] effectAttributes;
    public Sprite potionSprite;
    public Color color = Color.white;
}

[System.Serializable]
public class EffectAttribute {
    public EffectsSO effects;
    public int effectValue;
}