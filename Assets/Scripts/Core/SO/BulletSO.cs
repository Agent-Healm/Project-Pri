using UnityEngine;

[CreateAssetMenu(fileName = "BulletSO", menuName = "SO/Misc/Bullets")]
public class BulletSO : ScriptableObject
{
    public Sprite bulletSprite;
    public Color bulletColor = Color.white;
    public int uptime;
    public int damage;
    public float speed;
}