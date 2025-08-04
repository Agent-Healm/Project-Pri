using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// [System.Serializable]
public class Pistol : Weapon
{
    protected override void Awake() {
        base.Awake();
        PistolSO pistol = base.m_baseWeaponStats as PistolSO;
    }
}
