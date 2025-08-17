using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using Default;
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float m_range = 0.0f;
    [SerializeField] private LayerMask m_layerMask = GlobalLayerMask.PlayerLayer | GlobalLayerMask.EnvironmentLayer;
    [SerializeField] private LayerMask m_targetMask = GlobalLayerMask.PlayerLayer;
    [SerializeField] private EnemyAttackPattern[] m_enemyAttackPattern;
    
    private GameObject _target;
    private int _time;
    private Vector2 _distance;

    void Update()
    {
        if (_target == null){
            _target = GetTarget();
        }
        else{
            _distance = _target.transform.position - transform.position;
            if (_distance.magnitude > m_range + 3f){
                // Debug.Log("distance : " + _distance.magnitude);
                _target = null;
                return;
            }
            else{
                if (_time > 10){
                    if (LineOfSight()){
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
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _distance.normalized, m_range - 0.5f, m_layerMask);
        // if (raycastHit2D.collider == null){
        //     Debug.Log("nothing in sight");
        // }
        // else if(raycastHit2D.collider.name == _target.name){
        //     Debug.Log("player in sight");
        // }
        return raycastHit2D.collider?.gameObject.layer == GlobalLayer.Player;
    }
    // public void Chase(){
    //     transform.Translate(_distance.normalized * 0.08f);
    // }

    // public void Roam(){
    //     Vector2 randomPos =  new Vector2(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f));
    //     transform.Translate(randomPos * 0.5f);
    // }

    private void Attack(){
        foreach (EnemyAttackPattern enemyPattern in m_enemyAttackPattern){
            if (enemyPattern.Attempt(_distance.magnitude)){
                // enemyPattern.GetPattern.
                enemyPattern.GetAttackPatternSO.
                ShootBullet(_distance, transform.position, 0);
                break;
            }
        }
    }
    
    private Collider2D[] GetSurroundingTargetAll(){
        return Physics2D.OverlapCircleAll(transform.position, m_range - 0.5f, m_targetMask);    
    }
    
    private GameObject GetTarget(){
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
