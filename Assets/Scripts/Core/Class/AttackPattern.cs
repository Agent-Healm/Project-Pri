using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    [SerializeField] private Bullet bullet;
    public void ShootBullet(Vector2 l_direction, Vector2 position, int critChance)
    {
        bullet.SpawnBullet(l_direction, position, critChance);
    }
}

[System.Serializable]
public class EnemyAttackPattern
{
    [SerializeField] private AttackPattern attackPattern;
    public AttackPattern GetPattern {
        get {
            return attackPattern;
        }
    }
    [SerializeField] private int range;
    // public BulletSO bullet;
    public bool Attempt(float range){
        if (range <= this.range || this.range == -1.0f){
            // Debug.Log("Attack with "+ this.name);
            return true;
        }
        return false;
    }
}

[System.Serializable]
public class PlayerWeaponAttackPattern
{
    [SerializeField] private AttackPattern attackPattern;
    public AttackPattern GetPattern {
        get {
            return attackPattern;
        }
    }
    [SerializeField] private int energyCost = 0;
    public int EnergyCost
    {
        get { return energyCost; }
    }
    [SerializeField] private int critChance = 0;
    public int CritChance
    {
        get { return critChance; }
    }
    // public float foreswing = 1f;
    // public float backswing = 0f;
    // public float cooldown = 1f;
    // public float TBA {
    //     get => foreswing + cooldown;
    // } 
}