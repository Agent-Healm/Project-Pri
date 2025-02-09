using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int uptime = 60;
    public int damage;
    [SerializeField] private Vector2 direction;
    private float _time;
    void Awake(){
        BoxCollider2D collider = gameObject.GetComponent<BoxCollider2D>();
        if (gameObject.layer == 7){
            collider.excludeLayers = 1 << 6 | 1 << 7;
        }
        else if (gameObject.layer == 9){
            collider.excludeLayers = 1 << 8 | 1 << 9;
        }
    }
    void Start(){
        // if (direction == Vector2.zero){Debug.Log("no direction ??");}
    }
    void FixedUpdate(){

    }
    void Update(){
        transform.Translate(direction * 0.08f);
        if ( _time >= uptime){
            Destroy(gameObject);
            // Debug.Log("despawned");
        }
        _time += 1;
    }

    private void OnTriggerEnter2D(Collider2D other){

        IDamageAble damageable = other.gameObject.GetComponent<IDamageAble>();
        if (damageable != null){
            damageable.InflictDamage(damage);
            // Debug.Log("damageable object found");
            DestroyBullet();
        }
        else {
            if (other.gameObject.layer == 10){
                Debug.Log("I hit a wall");
                DestroyBullet();}
            // Debug.Log(other.gameObject.name);
            // Debug.Log("no damageable interface found, could be a wall");
        }
    }

    public void setDirection(Vector2 direction){
        this.direction = direction;
    }

    public void DestroyBullet(){
        Destroy(this.gameObject);
    }
}
