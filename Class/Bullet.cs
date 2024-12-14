using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public Vector2 direction;
    public GameObject texture;
    public float uptime = 3.0f;

    private float _time;

    void Awake(){
        // set framerate limit
        Application.targetFrameRate = 30;
    }
    void Start(){

    }
    void Update(){
        if ( _time >= uptime){
            Destroy(gameObject);
        }
        _time += 0.03f;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        // Debug.Log("hit");
        if (collision.gameObject.name == "home"){
            Debug.Log("I hit something");
            Destroy(gameObject);
        }
    }
}
