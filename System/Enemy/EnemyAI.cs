using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyAI : MonoBehaviour
{
    public float range = 0.0f;
    public GameObject target;
    public AttackPattern[] attackPattern;
    private int _frame;
    [SerializeField] private LayerMask layerMask;
    private Vector2 _distance;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _distance = target.transform.position - transform.position;
        _frame += 1;
        // if (_frame > 10){
        //     Attack();
        //     _frame = 0;
        // }
        if (Seek()){
            if (_frame > 10){
                Attack();
                _frame = 0;
            }
        }
    }
    public bool Seek(){
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _distance.normalized, range - 0.5f, layerMask);
        // if (raycastHit2D.collider == null){
        //     Debug.Log("nothing in sight");
        // }
        // else if(raycastHit2D.collider.name == target.name){
        //     Debug.Log("player in sight");
        // }
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
        foreach (AttackPattern pattern in attackPattern){
            if (pattern.Attempt(_distance.magnitude)){
                pattern.ShootBullet(_distance, transform.position);
                break;
            }
        }
    }
    

}
