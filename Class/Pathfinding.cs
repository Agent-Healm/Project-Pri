using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GameObject target;
    private int frame;
    private Vector2 distance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        distance = target.transform.position - transform.position;
        if(distance.magnitude > 1.5){
            Walk();
        }
        else {
            // Roam();
            if (frame > 30){
                Attack();
                frame = 0;
            }
            // Attack();
            frame += 1;
        }
        // if (frame >30){
        //     // Roam(); 
        //     Walk();
        //     frame = 0;
        // }
        // frame +=1;
        
    }

    public void Walk(){
        // pos = target.transform.position - transform.position;
        // Debug.Log(pos.normalized);
        transform.Translate(distance.normalized * 0.01f);
        // transform.Translate(pos.normalized * 0.1f);

    }

    public void Roam(){
        Vector2 randomPos = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        transform.Translate(randomPos * 0.5f);
    }

    public void Attack(){
        // Bullet bulletA = bullet;
        // bulletA.direction = pos;
        Instantiate(TextureTheme.instance.bulletTile, (Vector2)transform.position + distance.normalized * 0.4f, Quaternion.identity);
        // distance.normalized * 0.1f
    }
}
