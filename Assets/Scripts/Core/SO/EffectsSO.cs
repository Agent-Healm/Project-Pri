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
}