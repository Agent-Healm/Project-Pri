using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerWeaponSlot : MonoBehaviour
{
    public Weapon[] weapons;

    [SerializeField] private LayerMask layerMask;
    private Vector2 _facing;

    // Start is called before the first frame update
    void Start()
    {
        _facing = Vector2.up;
    }

    // Update is called once per frame
    void Update()
    {
        ActionHandler();
        FacingHandler();
    }
    private void ActionHandler(){
        if (Input.GetKeyDown(KeyCode.R)){
            Debug.Log("Player is interacting");
            weapons[0].Attack(_facing, transform.position);
        }   
        else if (Input.GetKeyDown(KeyCode.T)){
            Debug.Log("Player is interacting");
        }   
    }

    private void FacingHandler(){
        Vector2 _distance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // Vector2 _distance = 
        _facing = _distance.normalized;
        // RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, _distance.normalized, 5.5f, layerMask);
        // if (raycastHit2D.collider == null){
        //     Debug.Log("nothing in sight");
        // }
        // else if(raycastHit2D.collider.name == target.name){
        //     Debug.Log("player in sight");
        // }
        // _facing = 
        
    }
}
