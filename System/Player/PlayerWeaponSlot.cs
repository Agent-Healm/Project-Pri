using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerWeaponSlot : MonoBehaviour
{
    public Pistol[] weapons;
    private PlayerAim _playerAim;
    private PlayerMana _playerMana;
    private Vector2 _facing;

    // Start is called before the first frame update
    void Start()
    {
        _playerAim = this.GetComponent<PlayerAim>();
        _playerMana = this.GetComponent<PlayerMana>();
    }

    // Update is called once per frame
    void Update()
    {
        _facing = _playerAim.GetCurrentFaceDir();
        ActionHandler();
    }
    private void ActionHandler(){
        if (Input.GetKeyDown(KeyCode.R)){
            // Debug.Log("Player is interacting");
            if (weapons[0].pwap[0].AttemptAttack(_playerMana._energyPoint)){
                weapons[0].pwap[0].Attack(_facing, transform.position);
                _playerMana.ManaConsume(weapons[0].pwap[0].energyCost);
            }
        }
    }

    private void FacingHandler(){
        // Vector2 _distance = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        // _facing = _distance.normalized;
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
