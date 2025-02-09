using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyAI : MonoBehaviour
{
    public float range = 0.0f;
    public GameObject target;
    public AttackPattern[] attackPattern;
    private int _time;
    [SerializeField] private LayerMask layerMask = ~(1 << 6 | 1 << 7 | 1 << 9 << 1 << 10 | 1 << 11);
    [SerializeField] private LayerMask targetMask = 1 << 8;
    private Vector2 _distance;

    // void Awake(){
        
    // }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (target == null){
            foreach(Collider2D collider in SearchForTargets()){
                // Debug.Log("Enemy looking at" + collider.gameObject.name);
                // if (collider.gameObject.layer == 8){
                target = collider.gameObject;
                // }
                break;
            }
        }
        else{
            _distance = target.transform.position - transform.position;
            if (_distance.magnitude > range){
                // Debug.Log("distance : " + _distance.magnitude);
                target = null;
                return;
            }
            else{
                _time += 1;
                if (LineOfSight()){
                    if (_time > 10){
                        Attack();
                        _time = 0;
                    }
                }
            }
        }
    }
    public bool LineOfSight(){
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _distance.normalized, range - 0.5f, layerMask);
        // if (raycastHit2D.collider == null){
        //     Debug.Log("nothing in sight");
        // }
        // else if(raycastHit2D.collider.name == target.name){
        //     Debug.Log("player in sight");
        // }
        // Debug.Log(raycastHit2D.collider?.gameObject.name);
        // return (raycastHit2D.collider?.name == target.name);
        return (raycastHit2D.collider?.gameObject.layer == 8);
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
    
    public Collider2D[] SearchForTargets(){
        return Physics2D.OverlapCircleAll(transform.position, range - 0.5f, targetMask);    
    }
}
