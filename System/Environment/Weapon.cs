using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public AttackPattern attackPattern;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Attack(Vector2 direction, Vector2 position){
        // Debug.Log("Weapon action");
        // attackPattern.ShootBullet(Vector2.up, transform.position);    
        attackPattern.ShootBullet(direction, position);    
    }
}
