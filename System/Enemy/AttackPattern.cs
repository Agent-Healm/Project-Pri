using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPattern : MonoBehaviour
{
    public Bullet bullet;
    public float range = -1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool Attempt(float range){
        if (range <= this.range || this.range == -1.0f){
            // Debug.Log("Attack with "+ this.name);
            return true;
        }
        return false;
    }
    public void ShootBullet(Vector2 distance, Vector2 position){
        bullet.setDirection(distance.normalized);
        Instantiate(bullet, position + distance.normalized * 0.5f, Quaternion.identity);
    }
}
