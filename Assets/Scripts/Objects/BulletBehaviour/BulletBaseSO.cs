using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletBaseSO", menuName = "SO/BulletBase", order = 0)]
public class BulletBaseSO : ScriptableObject, IBulletBehaviour
{
    [SerializeField, Label("Color")]
    private Color m_bulletColor = Color.white;
    // public Color BulletColor
    // {
    //     get => m_bulletColor;
    // }
    [SerializeField, ShowAssetPreview(50,50), Label("Sprite")]
    private Sprite m_bulletSprite;
    // public Sprite BulletSprite
    // {
    //     get => m_bulletSprite;
    // }

    public void Apply(Bullet bullet)
    {
        SpriteRenderer spriteRenderer = bullet.GetComponent<SpriteRenderer>();

        spriteRenderer.sprite = m_bulletSprite;
        spriteRenderer.color = m_bulletColor;
    }
}
