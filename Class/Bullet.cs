using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;
    public GameObject texture;
    public float uptime = 2.4f;

    private float _time;

    void Awake(){
    }
    void Start(){

    }
    void Update(){
        transform.Translate(direction * 0.08f);
        if ( _time >= uptime){
            Destroy(gameObject);
            Debug.Log("despawned");
        }
        _time += 0.04f;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        // Debug.Log("hit");
        if (collision.gameObject.name == "home"){
            Debug.Log("I hit something");
            Destroy(gameObject);
        }
    }
}
