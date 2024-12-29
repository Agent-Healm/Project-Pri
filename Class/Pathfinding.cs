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
        if(distance.magnitude > 5.5){
            Walk();
        }
        else {
            if (frame > 10){
                Attack();
                frame = 0;
            }
            frame += 1;
        }
        
    }

    public void Walk(){
        transform.Translate(distance.normalized * 0.08f);

    }

    public void Roam(){
        Vector2 randomPos = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        transform.Translate(randomPos * 0.5f);
    }

    public void Attack(){
        GameObject bulletA = TextureTheme.instance.bulletTile;
        Bullet bullet = bulletA.GetComponent<Bullet>();
        bullet.direction = distance.normalized;

        Instantiate(bulletA, (Vector2)transform.position + distance.normalized * 0.5f, Quaternion.identity);        
        
    }
}
