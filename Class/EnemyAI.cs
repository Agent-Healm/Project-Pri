using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public GameObject target;
    public Bullet bullet;
    private int _frame;
    private Vector2 _distance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _distance = target.transform.position - transform.position;
        if(_distance.magnitude > 5.5){
            Chase();
        }
        else {
            if (_frame > 10){
                Attack();
                _frame = 0;
            }
            _frame += 1;
        }
    }

    public void Chase(){
        transform.Translate(_distance.normalized * 0.08f);

    }

    public void Roam(){
        Vector2 randomPos =  new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        transform.Translate(randomPos * 0.5f);
    }

    public void Attack(){
        bullet.setDirection(_distance.normalized);
        Instantiate(bullet, (Vector2)transform.position + _distance.normalized * 0.5f, Quaternion.identity);
    }
    

}
