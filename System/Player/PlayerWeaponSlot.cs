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
    private int _currentSlot = 0;
    private int _weaponSlotCount;
    private Pistol _currentWeapon;
    // Start is called before the first frame update
    void Start()
    {
        _playerAim = this.GetComponent<PlayerAim>();
        _playerMana = this.GetComponent<PlayerMana>();

        _weaponSlotCount = weapons.Length;
        _currentWeapon = weapons[0];
    }

    // Update is called once per frame
    void Update()
    {
        _facing = _playerAim.GetCurrentFaceDir();
        ActionHandler();
    }
    private void ActionHandler(){
        if (Input.GetKeyDown(KeyCode.T)){
            Debug.Log("Player is switching weapons");
            _currentSlot = (_currentSlot + _weaponSlotCount + 1) % _weaponSlotCount;
            _currentWeapon = weapons[_currentSlot];
        }
        if (Input.GetKey(KeyCode.R)){
            if (_currentWeapon.GetCurrentPwap().AttemptAttack(ref _playerMana._energyPoint)){
                _currentWeapon.GetCurrentPwap().Attack(_facing, transform.position, 
                _currentWeapon.GetWeaponInaccuracy());
                // _playerMana.ManaConsume(_currentWeapon.GetCurrentPwap().energyCost);
            }
        }
        if (Input.GetKeyDown(KeyCode.Y)){
            Debug.Log("Player is toggling another mode");
            _currentWeapon.SwitchWeaponMode();
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
