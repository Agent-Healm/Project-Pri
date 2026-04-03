// using EditorAttributes;
using System;
using System.Collections.Generic;
// using Healm.Inspector;
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
    // [HorizontalLayout("groupZ")] public int z1;
    // [HorizontalLayout("groupZ"), HideInInspector] public int z2;
    // [HorizontalLayout("groupZ"), HideInInspector] public int z3;
    // [HorizontalLayout("groupZ")] public SpriteRenderer z4;
    // [HorizontalLayout("groupZ")] public int z4;
    // public int zEnd;

    // [HorizontalGroup("GroupX")] public int x1;
    // [HorizontalGroup("GroupX")] public int x2;
    // [HorizontalGroup("GroupX")] public int x3;
    // [VerticalGroup("GroupX/Right")] public int x11;
    // [VerticalGroup("GroupX/Right")] public int x12;
    // [VerticalGroup("GroupX/Right")] public int x13;
    // // [VerticalLayout("GroupX/Right")] public int x14;
    // // [VerticalLayout("GroupX/Right")] public int x15;
    // // [VerticalLayout("GroupX/Right")] public int x16;
    // [HorizontalGroup("GroupX")] public int x4;
    // public int xEnd;

    // H(x1, V(x2, H(x3, V(x4, x5)), x7), x6)

    // [HorizontalGroup("GroupR")] public int r1;
    // [VerticalGroup("GroupR/split")] public int r2;
    // [HorizontalGroup("GroupR/split/inner")] public int r3;
    // [VerticalGroup("GroupR/split/inner/inner2")] public int r4;
    // [VerticalGroup("GroupR/split/inner/inner2")] public int r5;
    // [VerticalGroup("GroupR/split/inner/inner2")] public int r51;
    // [VerticalGroup("GroupR/split/inner/inner2")] public int r52;
    // [VerticalGroup("GroupR/split/inner/inner2")] public int r53;
    // [VerticalGroup("GroupR/split/inner/inner2")] public int r54;
    // [VerticalGroup("GroupR/split")] public int r6;
    // [HorizontalGroup("GroupR")] public int r7;
    // public int rEnd;

    // [HorizontalGroup("GroupY")]
    // // public MyData y0;
    // [VerticalGroup("GroupY/left")] public int y1;
    // [VerticalGroup("GroupY/left")] public MyData y2;
    // [VerticalGroup("GroupY/left")] public int y3;
    // // [VerticalGroup("GroupY/left")] public int y4;
    // [VerticalGroup("GroupY/mid")] public int y5;
    // [VerticalGroup("GroupY/mid")] public int y6;
    // [VerticalGroup("GroupY/mid")] public int y7;
    // // [VerticalGroup("GroupY/mid")] public int y8;
    // // [VerticalGroup("GroupY/right")] public int y9;
    // // [VerticalGroup("GroupY/right")] public int y10;
    // // [VerticalGroup("GroupY/right")] public int y11;
    // // [VerticalGroup("GroupY/right")] public int y12;
    // // public int yEnd;

    // // [FoldoutGroup("GroupF")] public MyData f0;
    // // [FoldoutGroup("GroupF")] public int f1;
    // // [FoldoutGroup("GroupF")] public int f2;
    // [FoldoutGroup("GroupF")] public MyData f3;
    // // [FoldoutGroup("GroupF")] public int f4;
    // // [FoldoutGroup("GroupF")] public int f5;
    // // [FoldoutGroup("GroupF")] public List<int> f6;
    // // [FoldoutGroup("GroupF")] public int[] f6;
    // [FoldoutGroup("GroupF")] public MyArray<int> f6;
    // // [FoldoutGroup("GroupF")] public List<MyData> f61;
    // [FoldoutGroup("GroupF")] public int f7;
    // // [FoldoutGroup("GroupF")] public int f8;
    // [FoldoutGroup("GroupF")] public MyArray<MyData> f9;

    public int fEnd;
    // public int[] items;
    public int itemEnd;


    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
    }

}
