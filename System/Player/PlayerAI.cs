using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask = ~(1 << 8 | 1 << 7 | 1 << 9 << 1 << 10 | 1 << 11);
    [SerializeField] private LayerMask targetMask = 1 << 6;

    private GameObject _target;
    private Vector2 _distance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if (_target == null){
        Collider2D[] collider2Ds = SearchForTargets();
        if (collider2Ds.Length > 0){
            foreach(Collider2D collider in collider2Ds){
                // ??
                // if (collider.gameObject.layer == 6){
                    // Debug.Log(collider.gameObject.name);
                    if(LineOfSight(collider.gameObject)){
                        _target = collider.gameObject;
                        Debug.Log("now targeting "  + _target.name);
                        break;
                    }
                // }
            }
        }
        // }
        else{
            _target = null;
        }
    }
    private Collider2D[] SearchForTargets(){
        return Physics2D.OverlapCircleAll(transform.position, 5f, targetMask);
    }

    private bool LineOfSight(GameObject potentialTarget){
        _distance = potentialTarget.transform.position - transform.position;
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _distance.normalized, 5f - 0.5f, layerMask);
        // if (raycastHit2D.collider == null){
        //     Debug.Log("nothing in sight");
        // }
        // else if(raycastHit2D.collider.name == potentialTarget.name){
        //     Debug.Log("player in sight");
        // }
        return raycastHit2D.collider?.gameObject.layer == 6;
    }
}
