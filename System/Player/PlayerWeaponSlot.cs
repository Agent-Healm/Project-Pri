using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerWeaponSlot : MonoBehaviour
{
    private PlayerAI playerAI;
    private PlayerSimpleMovement psm;
    public Weapon[] weapons;
    private Vector2 _facing = Vector2.up;

    // Start is called before the first frame update
    void Start()
    {
        playerAI = this.GetComponent<PlayerAI>();
        psm = this.GetComponent<PlayerSimpleMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerAI.isAiming()){
            _facing = playerAI.getAimDir();
        }
        else {
            _facing = psm.getMoveDir();
        }
        ActionHandler();        
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
        _facing = _distance.normalized;
        // Vector2 _distance = 
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
