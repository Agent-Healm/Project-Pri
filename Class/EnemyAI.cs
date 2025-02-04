using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    // public Bullet bullet;
    public float range = 0.0f;
    public GameObject target;
    public AttackPattern[] attackPattern;
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
        // if (raycastHit2D.collider == null){
        //     Debug.Log("nothing in sight");
        //     Chase();
        // }
        // else {
        //     Debug.Log("something in sight");
        //     if (_frame > 10){
        //         Attack();
        //         _frame = 0;
        //     }
        //     _frame += 1;
        // }

        // if(_distance.magnitude > range){
        //      Chase();
        // }
        // else {
        //     if (_frame > 10){
        //         Attack();
        //         _frame = 0;
        //     }
        //     _frame += 1;
        // }
        
        // Debug.Log("Direction : " + _distance.normalized);
        // Debug.Log("Distance : " + _distance.magnitude);
        // the raycast overlap itself

           
        _frame += 1;
        // RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _distance.normalized, range - 0.5f);
        // if (raycastHit2D.collider == null){
        //     Debug.Log("nothing in sight");
        // }
        // else if(raycastHit2D.collider.name == target.name){
        //     Debug.Log("player in sight");
        //     if (_frame > 10){
        //         Attack();
        //         _frame = 0;
        //     }
        // }
        // else {
        //     // raycast hit the target position instead of the center of the object.
        //     Debug.Log("something in " + raycastHit2D.point + raycastHit2D.distance);
        //     Debug.Log(raycastHit2D.collider.name);
        // }
        if (Seek()){
            if (_frame > 10){
                Attack();
                _frame = 0;
            }
        }
    }
    public bool Seek(){
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _distance.normalized, range - 0.5f);
        // if (raycastHit2D.collider == null){
        //     Debug.Log("nothing in sight");
        // }
        // else if(raycastHit2D.collider.name == target.name){
        //     Debug.Log("player in sight");
        // }
        // return (raycastHit2D.collider != null) && (raycastHit2D.collider.name == target.name);
        return (raycastHit2D.collider?.name == target.name);
    }
    public void Chase(){
        transform.Translate(_distance.normalized * 0.08f);
    }

    public void Roam(){
        Vector2 randomPos =  new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
        transform.Translate(randomPos * 0.5f);
    }

    public void Attack(){
        // bullet.setDirection(_distance.normalized);
        // Instantiate(bullet, (Vector2)transform.position + _distance.normalized * 0.5f, Quaternion.identity);
        
        foreach (AttackPattern pattern in attackPattern){
            if (pattern.Attempt(_distance.magnitude)){
                pattern.ShootBullet(_distance, transform.position);
                break;
            }
        }
    }
    

}
