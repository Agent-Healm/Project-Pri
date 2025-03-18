// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EffectsSO", menuName = "SO/Effects")]
public class EffectsSO : ScriptableObject
{
    // public string effectName;   
}

[System.Serializable]
public class EffectAttribute {
    public EffectsSO effects;
    public int effectValue;
}