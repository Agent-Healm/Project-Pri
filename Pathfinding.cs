using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public GameObject target;
    private int sec;
    private Vector2 pos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        pos = target.transform.position - transform.position;
        if(pos.magnitude > 1.5){
            Walk();
        }
        // else {
        //     Roam();
        // }
        // if (sec >60){
        //     Roam(); 
        //     sec = 0;
        // }
        // sec +=1;
        
    }

    public void Walk(){
        // pos = target.transform.position - transform.position;
        // Debug.Log(pos.normalized);
        transform.Translate(pos.normalized * 0.01f);
    }

    public void Roam(){
        Vector2 randomPos = new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        transform.Translate(randomPos * 0.5f);
    }
}
