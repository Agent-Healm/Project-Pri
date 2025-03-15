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
    public void ShootBullet(Vector2 distance, Vector2 position, int critChance){
        bullet.setDirection(distance.normalized);
        bullet.setCritChance(critChance);
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
    public int energyCost = 0;
    public int critChance = 0;
    // public float foreswing = 1f;
    // public float backswing = 0f;
    // public float cooldown = 1f;
    // public float TBA {
    //     get => foreswing + cooldown;
    // } 
}