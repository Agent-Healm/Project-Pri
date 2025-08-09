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
    public int text;
    // between text and "a" field on line 27, theres a weird spacing

    [HorizontalLayout("texta")]
    // [LabelSize(60f)]
    // [FieldColor(0, 1, 0, 1)]
    public int weirdField;
    [HorizontalLayout("texta")] public int b;
    // public int za;
    // public int zb;
    // [VerticalLayout("text/v")] public int zc;
    // [VerticalLayout("text/v")] public int zd;
    // [VerticalLayout("text/v")] public int zz;
    // [VerticalLayout("text/v")] public int e;
    [HorizontalLayout("texta", true)] public int e;

    public int batasTexta;

    [HorizontalLayout("text")] public int end;
    [HorizontalLayout("text")] public int d;
    [VerticalLayout("text/v")] public int z;
    [VerticalLayout("text/v")] public int w;
    [VerticalLayout("text/v")] public int w2;
    [VerticalLayout("text/v")] public int w3;
    [VerticalLayout("text/v")] public int w4;
    [VerticalLayout("text/v")] public int w5;
    [VerticalLayout("text/v")] public int w6;
    [HorizontalLayout("text", true)] public int w7;



    public int batas1;




    // [HorizontalLayout("wtf")] public int zw1;
    // [HorizontalLayout("wtf")] public int zw2;
    // [HorizontalLayout("wtf", true)] public int zw3;


    public int batas2;
    public int batas2_v2;

    [HorizontalLayout("text")] public int end2;
    [HorizontalLayout("text")] public int d2;
    [VerticalLayout("text/v")] public int z2;
    [VerticalLayout("text/v")] public int ww1;
    [VerticalLayout("text/v")] public int ww2;
    [VerticalLayout("text/v")] public int ww3;
    [VerticalLayout("text/v")] public int ww4;
    [VerticalLayout("text/v")] public int ww5;
    [VerticalLayout("text/v")] public int ww6;
    [VerticalLayout("text/v", true)] public int ww7;
    // [HorizontalLayout("text", true)] public int ww7;

    /*
        something weird about eol in vertical layout
    */


    public int batas3;

    private bool MustNotNegative(int value) => value >= 0;
    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }
}
