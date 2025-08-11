using System.Collections;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletBaseSO", menuName = "SO/BulletBase", order = 0)]
public class BulletBaseSO : ScriptableObject, IBulletBehaviour
{
    [SerializeField]
    private Color m_bulletColor = Color.white;
    // public Color BulletColor
    // {
    //     get => m_bulletColor;
    // }
    [SerializeField] [AssetPreview(60f, 60f)]
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
