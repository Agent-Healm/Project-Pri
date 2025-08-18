// using EditorAttributes;
using Healm.EditorTools;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletActionDoDamageSO", menuName = "SO/Bullet/BulletActionDoDamage", order = 0)]
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
    [HorizontalLayout2("groupZ")]
    public int z1;
    [HorizontalLayout2("groupZ"), HideInInspector] public int z2;
    [HorizontalLayout2("groupZ"), HideInInspector] public int z3;
    // [HorizontalLayout2("groupZ"), HideInInspector] public SpriteRenderer z4;
    // [HorizontalLayout2("groupZ")] public int z4;
    public int zEnd;

    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }
}
