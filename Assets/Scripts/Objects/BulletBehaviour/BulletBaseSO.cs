using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletBaseSO", menuName = "SO/BulletBase", order = 0)]
public class BulletBaseSO : ScriptableObject, IBulletBehaviour
{
    [SerializeField] private Sprite m_bulletSprite;
    public Sprite BulletSprite
    {
        get => m_bulletSprite;
    }
    [SerializeField] private Color m_bulletColor = Color.white;
    public Color BulletColor
    {
        get => m_bulletColor;
    }
    [SerializeField] private int m_uptime = 60;
    public int Uptime
    {
        get => m_uptime;
    }

    public void Apply(Bullet bullet)
    {
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = m_bulletSprite;
        spriteRenderer.color = m_bulletColor;
        bullet.SetUptime = m_uptime;
    }
}
