using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "SO/Bullet/Bullets", order = 0)]
public class BulletDataSO : ScriptableObject
{
    // [SerializeField]
    // private List<ScriptableObject> m_bulletBehaviours;
    // public List<ScriptableObject> Behaviours
    // {
    //     get => m_bulletBehaviours;
    // }

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


    [SerializeField]
    private Color m_bulletColor = Color.white;
    // public Color BulletColor
    // {
    //     get => m_bulletColor;
    // }
    [SerializeField]
    private Sprite m_bulletSprite;
    // public Sprite BulletSprite
    // {
    //     get => m_bulletSprite;
    // }

    public void Apply(Bullet bullet)
    {
        bullet.SetDamage = m_damage;
        bullet.SetSpeed = m_speed;
        bullet.SetUptime = m_uptime;
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = m_bulletSprite;
        spriteRenderer.color = m_bulletColor;
    }

}