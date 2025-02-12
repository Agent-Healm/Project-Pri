using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    public Bullet bullet;
    // public float range = -1.0f;
    // void Start()
    // {
        
    // }

    // void Update()
    // {
        
    // }
    public void ShootBullet(Vector2 distance, Vector2 position){
        bullet.setDirection(distance.normalized);
        Instantiate(bullet, position + distance.normalized * 0.5f, Quaternion.identity);
    }
}

[System.Serializable]
public class EnemyAttackPattern 
{
    public AttackPattern attackPattern;
    public int range;
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
    public AttackPattern attackPattern;
    // public int damage = 0;
    public int energyCost = 0;
    public int critChance = 0;
    public float roundsPerSec = 1f;

    public bool AttemptAttack(int manaPoint){
        if (manaPoint >= energyCost){
            // pwap[0].Attack();
            // attackPattern.ShootBullet();
            // manaPoint -= energyCost;
            return true;
        }
        return false;
    }
    public void Attack(Vector2 direction, Vector2 position){
        attackPattern.ShootBullet(direction, position);
        // return true;
    }
}