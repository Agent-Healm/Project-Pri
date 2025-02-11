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