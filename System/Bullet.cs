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
            Debug.Log("despawned");
        }
        _time += 1;
    }

    // only usable when two collider2D non trigger 
    // private void OnCollisionEnter2D(Collision2D other){
    //     Debug.Log("collision triggered");
    //     Debug.Log(other.gameObject.name);
    //     // if (other.gameObject.CompareTag("wall")){
    //     if (other.gameObject.tag == "wall"){
    //         Debug.Log("I hit a wall");
    //         Destroy(gameObject);}
    // }

    private void OnTriggerEnter2D(Collider2D other){
        // Debug.Log("hit");
        // if (collision.collider){
        if (other.gameObject.tag == "wall"){
            // move to layer for control
            Debug.Log("I hit a wall");
            DestroyBullet();}
        if (other.gameObject.name == "player"){
            Hitpoint hpTarget = other.gameObject.GetComponent<Hitpoint>();
            hpTarget.TakeDamage(damage);
            // Debug.Log("I hit something");
            DestroyBullet();

        }
    }

    public void setDirection(Vector2 direction){
        this.direction = direction;
    }

    public void DestroyBullet(){
        Destroy(this.gameObject);
    }
}
