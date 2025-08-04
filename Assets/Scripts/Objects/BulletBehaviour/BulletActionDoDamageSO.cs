using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletActionDoDamageSO", menuName = "SO/BulletActionDoDamage", order = 0)]
public class BulletActionDoDamageSO : ScriptableObject, IBulletBehaviour
{
    [SerializeField, ValidateInput("MustNotNegative", "Damage must not be negative value")]
    private int m_damage;

    [SerializeField]
    private float m_speed;
    [SerializeField, ValidateInput("MustNotNegative", "Uptime must not be negative value")]
    private int m_uptime = 60;
    // public int Uptime
    // {
    //     get => m_uptime;
    // }
    private bool MustNotNegative(int value) => value >= 0;
    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }
}