// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OrbSO", menuName = "SO/Orb")]
public class OrbSO : ScriptableObject {
    public EffectAttribute effectAttributes;
    public Sprite orbSprite;
    public Color color = Color.white;
}
