// using System.Collections;
// using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public class WeaponStatsSO : ScriptableObject
{
    [SerializeField, BoxGroup("Weapon Info")]
    protected WeaponTypeSO m_weaponType;
    public WeaponTypeSO WeaponType
    {
        get => m_weaponType;
    }
    [SerializeField, BoxGroup("Weapon Info")]
    protected WeaponRaritySO m_weaponRarity;
    public WeaponRaritySO Rarity
    {
        get => m_weaponRarity;
    }
    [SerializeField]
    protected int m_deviation;
    public int Deviation
    {
        get => m_deviation;
    }
    [SerializeField]
    protected int m_speedModPct;
    public int SpeedModifier
    {
        get => m_speedModPct;
    }
    [SerializeField, Label("Weapon Attack Pattern")]
    protected PlayerWeaponAttackPattern[] m_pwap;
    public PlayerWeaponAttackPattern[] AttackPatterns
    {
        get => m_pwap;
    }
    // public virtual void InitializeWeapon(Weapon weapon)
    // {
        
    // }
}
