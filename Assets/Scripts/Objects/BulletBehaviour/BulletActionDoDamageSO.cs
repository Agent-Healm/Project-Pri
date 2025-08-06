using NaughtyAttributes;
using UnityEngine;
using Healm.EditorTools;

[GlobalBackgroundColor(0,0,1)]
[CreateAssetMenu(fileName = "BulletActionDoDamageSO", menuName = "SO/BulletActionDoDamage", order = 0)]
public class BulletActionDoDamageSO : ScriptableObject, IBulletBehaviour
{
    // [FieldColor(255,0,0,1)]
    [SerializeField, ValidateInput("MustNotNegative", "Damage must not be negative value")]
    private int m_damage;
    
    [SerializeField]
    private float m_speed;

    [SerializeField, ValidateInput("MustNotNegative", "Uptime must not be negative value")]
    private int m_uptime = 60;

    [LabelSize(50f)]
    public int a;
    [LabelSize(50f)]
    public int b;
    [LabelSize(50f)]
    public int c;
    // public int Uptime
    // {
    //     get => m_uptime;
    // }
    public int end;
    private bool MustNotNegative(int value) => value >= 0;
    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }
}