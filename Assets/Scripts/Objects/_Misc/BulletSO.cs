using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "SO/Bullet/Bullets", order = 0)]
public class BulletDataSO : ScriptableObject
{
    [SerializeField]
    private List<ScriptableObject> m_bulletBehaviours;
    public List<ScriptableObject> Behaviours
    {
        get => m_bulletBehaviours;
    }

}