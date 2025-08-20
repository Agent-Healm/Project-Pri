// using EditorAttributes;
using System;
using System.Collections.Generic;
using Healm.EditorTools;
using UnityEngine;
using UnityEngine.UI;

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
    // [HorizontalLayout2("groupZ")] public int z1;
    // [HorizontalLayout2("groupZ"), HideInInspector] public int z2;
    // [HorizontalLayout2("groupZ"), HideInInspector] public int z3;
    // [HorizontalLayout2("groupZ")] public SpriteRenderer z4;
    // [HorizontalLayout2("groupZ")] public int z4;
    // public int zEnd;

    [HorizontalLayout2("GroupX")] public int x1;
    [HorizontalLayout2("GroupX")] public int x2;
    [HorizontalLayout2("GroupX")] public int x3;
    // [VerticalLayout2("GroupX/Right")] public int x11;
    // [VerticalLayout2("GroupX/Right")] public int x12;
    // [VerticalLayout2("GroupX/Right")] public int x13;
    // [VerticalLayout2("GroupX/Right")] public int x14;
    [HorizontalLayout2("GroupX")] public int x4;
    // [HorizontalLayout2("GroupX")]
    // [VerticalLayout2("GroupX")] public int x4;
    // [VerticalLayout2("GroupX")] public int x5;
    public int xEnd;

    /*
        H(x1, x2, V(x11, x12, x13))

       x1 [] x2[] x11[]
                  x12[]     
                  x13[]     
    */

    /*
        H group1 x1

        H group1 
        V group1/split x2

        v group1/split
        H group1/split/inner x3

        v group1/split/inner/inner2 x4
        v group1/split/inner/inner2 x5

        =>
        H(x1, V(x2, H(x3, V(x4, x5)), x7), x6)

        x1[] | (    x2[]     ) | x6
               (x3[] | (x4[]))
               (       (x5[]))
               (    x7[]     )
    */
    
    // public void OnEnable()
    // {
    // }
    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }

}
