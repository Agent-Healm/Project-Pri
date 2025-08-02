using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "SO/Misc/Bullets")]
public class BulletDataSO : ScriptableObject
{
    [SerializeField] private int m_damage;
    public int Damage
    {
        get => m_damage;
    }
    [SerializeField] private float m_speed;
    public float Speed
    {
        get => m_speed;
    }

    [SerializeField] private List<ScriptableObject> m_bulletBehaviours;
    public List<ScriptableObject> Behaviours
    {
        get => m_bulletBehaviours;
    }

}