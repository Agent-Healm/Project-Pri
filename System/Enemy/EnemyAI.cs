using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnemyAI : MonoBehaviour
{
    public float range = 0.0f;
    public AttackPattern[] attackPattern;
    private GameObject _target;
    private int _time;
    [SerializeField] private LayerMask layerMask = DLayer.PlayerLayer() | DLayer.EnvironmentLayer();
    [SerializeField] private LayerMask targetMask = DLayer.PlayerLayer();
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
        if (_target == null){
            _target = GetTarget();
        }
        else{
            _distance = _target.transform.position - transform.position;
            if (_distance.magnitude > range + 3f){
                // Debug.Log("distance : " + _distance.magnitude);
                _target = null;
                return;
            }
            else{
                if (LineOfSight()){
                    if (_time > 10){
                        Attack();
                        _time = 0;
                    }
                }
                // else {
                //     Chase();
                // }
            }
        }
        _time += 1;
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
    
    public Collider2D[] GetSurroundingTargetAll(){
        return Physics2D.OverlapCircleAll(transform.position, range - 0.5f, targetMask);    
    }
    
    public GameObject GetTarget(){
        // foreach(Collider2D collider in GetSurroundingTargetAll()){
        //     // Debug.Log("Enemy looking at" + collider.gameObject.name);
        //     // if (collider.gameObject.layer == 8){
        //     return collider.gameObject;
        //     // }
        //     // break;
        // }
        Collider2D[] collider2D = GetSurroundingTargetAll();
        if (collider2D.Length > 0){return collider2D[0].gameObject;}
        return null;
    }
}
