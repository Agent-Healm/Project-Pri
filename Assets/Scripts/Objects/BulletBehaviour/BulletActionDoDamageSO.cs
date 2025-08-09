using NaughtyAttributes;
using UnityEngine;
using Healm.EditorTools;

// [GlobalBackgroundColor(0,0.3f,1)]
[CreateAssetMenu(fileName = "BulletActionDoDamageSO", menuName = "SO/BulletActionDoDamage", order = 0)]
public class BulletActionDoDamageSO : ScriptableObject, IBulletBehaviour
{
    [SerializeField, HorizontalLayout("basic_stats")]
    [LabelSize(60f), FieldColor(1, 0, 0, 1)]
    [ValidateInput("MustNotNegative", "Damage must not be negative value")]
    private int m_damage;

    [SerializeField, HorizontalLayout("basic_stats")]
    [LabelSize(60f), FieldColor(1, 1, 0, 1)]
    private float m_speed;

    [SerializeField, HorizontalLayout("basic_stats", true)]
    [LabelSize(60f), FieldColor(0, 1, 0, 1)]
    [ValidateInput("MustNotNegative", "Uptime must not be negative value")]
    private int m_uptime = 60;

    // public int Uptime
    // {
    //     get => m_uptime;
    // }
    [HorizontalLayout("text")]
    public int a;
    [HorizontalLayout("text")]
    public int b;
    // public int za;
    // public int zb;
    // [VerticalLayout("text/v")]
    // [HorizontalLayout("text")]
    // public int c;
    // [VerticalLayout("text/v")]
    // public int d;
    // [VerticalLayout("text/v")]
    // public int z;
    // [VerticalLayout("text/v", true)]
    // public int e;


    [HorizontalLayout("text")]
    public int end;
    [VerticalLayout("text/v")]
    public int d;
    [VerticalLayout("text/v")]
    public int z;
    [VerticalLayout("text/v")]
    public int w;
    [VerticalLayout("text/v")]
    public int w2;
    [VerticalLayout("text/v", true)]
    public int e;


    public int batas;
    private bool MustNotNegative(int value) => value >= 0;
    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }
}
