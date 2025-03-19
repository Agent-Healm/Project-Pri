// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbSO", menuName = "SO/Decor/Orb")]
public class OrbSO : ScriptableObject {
    [field:SerializeField] public EffectAttribute effectAttributes {get; private set;}
    [field:SerializeField] public Sprite orbSprite {get; private set;}
    [field:SerializeField] public Color color {get; private set;} = Color.white;
}
