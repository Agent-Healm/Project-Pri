using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Default;

[CreateAssetMenu(fileName = "AttackPatternSO", menuName = "SO/AttackPattern")]
public class AttackPatternSO : ScriptableObject
{
    [SerializeField] private Bullet m_Bullets;
    public Bullet Bullets1 { get => m_Bullets; }
    [SerializeField] private ScriptableObject m_AttackModifier;
    public ScriptableObject AttackModifier1 { get => m_AttackModifier; }

    public void ShootBullet(Vector2 direction, Vector2 position, int critChance)
    {
        m_Bullets?.SpawnBullet(direction, position, critChance);
    }

    // public void ApplyAttackPattern(Bullet bullet)
    // {
    //     if (m_Bullets != null)
    //     {
    //         bullet.SetDamage = m_Bullets.SetDamage;
    //         bullet.SetSpeed = m_Bullets.SetSpeed;
    //         bullet.SetUptime = m_Bullets.SetUptime;
    //         bullet.Direction = m_Bullets.Direction;
    //     }

    //     if (m_AttackModifier is IAttackModifier attackModifier)
    //     {
    //         attackModifier.ModifyAttack();
    //     }
    // }
}
