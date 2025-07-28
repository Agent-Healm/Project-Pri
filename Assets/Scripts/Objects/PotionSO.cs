using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PotionSO", menuName = "SO/Decor/Potion")]
public class PotionSO : ScriptableObject
{
    [SerializeField] private EffectAttribute[] effectAttributes;
    public EffectAttribute[] GetEffectAttributes { get => effectAttributes; }
    [SerializeField] private Sprite potionSprite;
    [SerializeField] private Color color = Color.white;
    
}