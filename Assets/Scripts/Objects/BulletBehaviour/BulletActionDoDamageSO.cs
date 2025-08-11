using EditorAttributes;
// using Healm.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletActionDoDamageSO", menuName = "SO/BulletActionDoDamage", order = 0)]
public class BulletActionDoDamageSO : ScriptableObject, IBulletBehaviour
{
    [SerializeField]
    private int m_damage;
    [SerializeField]
    private float m_speed;
    [SerializeField] 
    private int m_uptime = 60;

    // public int Uptime
    // {
    //     get => m_uptime;
    // }
    public int text;

    [HorizontalGroup(true, nameof(vertGroup1), nameof(vertGroup2), nameof(vertGroup3))]
    [SerializeField] private Void horiGroup1;

    [VerticalGroup(true, nameof(q1), nameof(q2), nameof(q3), nameof(q4))]
    [SerializeField, HideInInspector] private Void vertGroup1;
    [VerticalGroup(true, nameof(q5), nameof(q6), nameof(q7), nameof(q8))]
    [SerializeField, HideInInspector] private Void vertGroup2;
    [VerticalGroup(true, nameof(q9), nameof(q10), nameof(q11), nameof(q12), nameof(q13))]
    [SerializeField, HideInInspector] private Void vertGroup3;

    [SerializeField, HideInInspector]
    // [FieldColor(0,0,1)]
    public int q1;
    [SerializeField, HideInInspector] public int q2;
    [SerializeField, HideInInspector] public int q3;
    [SerializeField, HideInInspector] public int q4;
    [SerializeField, HideInInspector] public int q5;
    [SerializeField, HideInInspector] public int q6;
    [SerializeField, HideInInspector] public int q7;
    [SerializeField, HideInInspector] public int q8;
    [SerializeField, HideInInspector] public Sprite q9;
    [SerializeField, HideInInspector] public SpriteRenderer q10;
    [SerializeField, HideInInspector] public AudioClip q11;
    [SerializeField, HideInInspector] public GameObject q12;
    [SerializeField, HideInInspector] public Vector3 q13;

    // [SerializeField, HideInInspector] public int qEnd;

    private bool MustNotNegative(int value) => value >= 0;
    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }
}
