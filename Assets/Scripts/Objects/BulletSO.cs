using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO", menuName = "SO/Misc/Bullets")]
public class BulletSO : ScriptableObject
{
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private Color bulletColor = Color.white;
    [SerializeField] private int uptime;
    [SerializeField] private int damage;
    [SerializeField] private float speed;

    // TODO
}