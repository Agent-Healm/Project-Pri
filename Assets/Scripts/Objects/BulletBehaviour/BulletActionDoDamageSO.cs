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

    // [LabelSize(60f)]
    // [FieldColor(0, 1, 0, 1)]
    // [HorizontalLayout("texta")] public int textaA;
    // [HorizontalLayout("texta")] public int textaB;
    // [HorizontalLayout("texta", true)] public int textaEnd;
    // public int batasTexta;

    // [HorizontalLayout("text")] public int z1;
    // [HorizontalLayout("text")] public int z2;
    // [VerticalLayout("text/v")] public int z3;
    // [VerticalLayout("text/v")] public int z4;
    // // [HorizontalLayout("text")] public int z9;
    // [VerticalLayout("text/v")] public int z5;
    // [VerticalLayout("text/v")] public int z6;
    // // [VerticalLayout("text/v")] public int z7;
    // // [VerticalLayout("text/v")] public int z8;
    // [HorizontalLayout("text", true)] public int zEnd;
    // public int batasZ1ToZ9;




    // [HorizontalLayout("wtf")] public int zw1;
    // [HorizontalLayout("wtf")] public int zw2;
    // [HorizontalLayout("wtf", true)] public int zw3;

    // public int batasZ2;
    // public int batasZ3;


    // [HorizontalLayout("text")] public int x1;
    // [HorizontalLayout("text")] public int x2;
    // [VerticalLayout("text/v")] public int x3;
    // // [VerticalLayout("text/v")] public int x4;
    // // [VerticalLayout("text/v")] public int x5;
    // // [VerticalLayout("text/v")] public int x6;
    // public int batasX1toXEnd;
    // [VerticalLayout("text/v", true)] public int xEnd;

    /*
        something weird about eol in vertical layout
    */

    [HorizontalLayout("zet")] public int q1;
    [HorizontalLayout("zet")] public int q2;
    [VerticalLayout("zet")] public int q3;
    [VerticalLayout("zet")] public int q4;
    [VerticalLayout("zet")] public int q5;
    [VerticalLayout("zet")] public int q6;
    [VerticalLayout("zet")] public int q7;
    [VerticalLayout("zet")] public int q8;
    [VerticalLayout("zet")] public SpriteRenderer q10;
    [VerticalLayout("zet")] public Sprite q9;
    // [VerticalLayout("zet")] public AudioClip q11;
    // [VerticalLayout("zet")] public GameObject q12;
    // [VerticalLayout("zet")] public Vector3 q11;

    [VerticalLayout("zet", true)] public int qEnd;
    public int batasQ1toQEnd;

    // [HorizontalLayout("")] public int f1;
    // [HorizontalLayout("")] public int f2;
    // [HorizontalLayout("")] public int f3;
    // [HorizontalLayout("")] public int f4;
    // [HorizontalLayout("", true)] public int f5;

    // public int batasF1;

    // [HorizontalLayout("")] public int vert1;
    // [VerticalLayout("")] public int vert2;
    // // [VerticalLayout("")] public int vert3;
    // // [VerticalLayout("")] public int vert4;
    // // [VerticalLayout("")] public int vert5;
    // // [VerticalLayout("")] public int vert6;
    // [VerticalLayout("",true)] public int vertEnd;
    // public int batasVert;

    private bool MustNotNegative(int value) => value >= 0;
    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }
}
