using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

[CreateAssetMenu(fileName = "BulletDataSO", menuName = "SO/Misc/Bullets")]
public class BulletDataSO : ScriptableObject
{
    [SerializeField, Expandable]
    private List<ScriptableObject> m_bulletBehaviours;
    public List<ScriptableObject> Behaviours
    {
        get => m_bulletBehaviours;
    }

}