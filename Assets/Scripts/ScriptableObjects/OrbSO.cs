// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbSO", menuName = "SO/Decor/Orb")]
public class OrbSO : ScriptableObject
{
    [SerializeField] private EffectAttribute effectAttributes;
    public EffectAttribute GetEffectAttributes { get => effectAttributes; }
    [SerializeField] private Sprite orbSprite;
    // public Sprite GetOrbSprite { get => orbSprite; }

    [SerializeField] private Color color = Color.white;
    public Color GetColor { get => color; }
}
