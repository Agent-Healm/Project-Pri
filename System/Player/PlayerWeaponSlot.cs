using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerWeaponSlot : MonoBehaviour
{
    // [SerializeReference]
    // public List<Weapon> weaponAhh = new List<Weapon>();
    public Weapon[] weaponInv;
    public int weaponSlotCountMax = 3;
    private int _currentWeaponSlot = -1;
    private bool _itemNearby;
    // [SerializeReference]
    private Weapon _currentWeapon;

    private Vector2 _facing;
    private Collider2D _other;
    private PlayerAim _playerAim;
    private PlayerMana _playerMana;
    
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
        WeaponActionHandler();
    }
    private void WeaponActionHandler(){
        if (Input.GetKeyDown(KeyCode.T)){
            PlayerSwitchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Y)){
            _currentWeapon.SwitchWeaponMode();
        }
        if (Input.GetKeyDown(KeyCode.G)){
            if (weaponInv.Length == 0){
                return;
            }
            PlayerDropCurrentWeapon();

            ArrayUtility.RemoveAt(ref weaponInv, _currentWeaponSlot);
            if (_currentWeaponSlot >= weaponInv.Length){
                _currentWeaponSlot -= 1;
            }

            UpdateCurrentWeapon();
        }
    }
    public void WeaponAction(){
        if (weaponInv.Length == 0){
            Debug.Log("Player has no weapons");
            return;
        }

        if (_currentWeapon.AttemptAction(ref _playerMana._energyPoint)){
            _currentWeapon.Action(_facing, transform.position);
        }
    }
    public void AddToWeaponSlots(Weapon weapon){
        if (weaponInv.Length < weaponSlotCountMax){
            _currentWeaponSlot += 1;
            ArrayUtility.Insert(ref weaponInv, _currentWeaponSlot, weapon);
        }
        else {
            PlayerDropCurrentWeapon();
            weaponInv[_currentWeaponSlot] = weapon;
        }
        UpdateCurrentWeapon();
    }

    private void PlayerSwitchWeapon(){
        if (weaponInv.Length <= 1){return;}
        _currentWeaponSlot = (_currentWeaponSlot + weaponSlotCountMax + 1) % weaponInv.Length;
        UpdateCurrentWeapon();
    }

    private void UpdateCurrentWeapon(){
        if (weaponInv.Length == 0){
            _currentWeapon = null;
            return;
        }
        _currentWeapon = weaponInv[_currentWeaponSlot];
    }

    private void PlayerDropCurrentWeapon(){
        weaponInv[_currentWeaponSlot].gameObject.SetActive(true);
        weaponInv[_currentWeaponSlot].transform.position = transform.position;
        
    }
}
