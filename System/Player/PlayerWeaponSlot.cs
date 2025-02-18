using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerWeaponSlot : MonoBehaviour
{
    // [SerializeReference]
    // public List<Weapon> weaponAhh = new List<Weapon>();
    public Weapon[] weaponInv;
    // public List<Weapon> weapons = new List<Weapon>();
    public int weaponSlotCountMax = 3;
    private bool _itemNearby;
    private PlayerAim _playerAim;
    private PlayerMana _playerMana;
    private Vector2 _facing;
    private int _currentWeaponSlot = -1;
    private Collider2D _other;


    // [SerializeReference]
    private Weapon _currentWeapon;
    // Start is called before the first frame update
    void Start()
    {
        _playerAim = this.GetComponent<PlayerAim>();
        _playerMana = this.GetComponent<PlayerMana>();

        // WeaponSlotCountMax = weaponInv.Length;
        // _currentWeapon = weaponInv[0];
        // foreach(Weapon weapon in weaponAhh){
        //     Debug.Log(weapon.weaponType);
        // }
    }

    // Update is called once per frame
    void Update()
    {
        _facing = _playerAim.GetCurrentFaceDir();
        ActionHandler();
    }
    private void ActionHandler(){
        if (Input.GetKeyDown(KeyCode.T)){
            PlayerSwitchWeapon();
        }
        if (Input.GetKey(KeyCode.R)){
            PlayerAction();
        }
        if (Input.GetKeyDown(KeyCode.Y)){
            Debug.Log("Player is toggling another mode");
            _currentWeapon.SwitchWeaponMode();
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        if ((other.gameObject.layer == 12) || (other.gameObject.layer == 13)){
            // Debug.Log("Weapons nearby player");
            _itemNearby = true;
            _other = other;
        }
    }
    private void OnTriggerExit2D(Collider2D other){
        if ((other.gameObject.layer == 12) || (other.gameObject.layer == 13)){
            // Debug.Log("Weapons are no longer nearby player");
            _itemNearby = false;
            _other = null;
        }
    }

    private void PlayerAction(){
        if (_itemNearby){
            // Debug.Log("Player could pickup an item");
            if(_other.TryGetComponent<Potion>(out Potion potion)){
                potion.OnInteract();
            }
            // Weapon weapon = _other.GetComponent<Weapon>();
            else if (_other.TryGetComponent<Weapon>(out Weapon weapon)){
                AddToWeaponSlots(weapon);
            }
        }
        else {
            if (weaponInv.Length == 0){
                Debug.Log("Player has no weapons");
                return;
            }

            if (_currentWeapon.AttemptAction(ref _playerMana._energyPoint)){
                _currentWeapon.Action(_facing, transform.position);
            }
        }

    }

    private void AddToWeaponSlots(Weapon weapon){
        if (weaponInv.Length < weaponSlotCountMax){
            // Debug.Log("Player picked up " + weapon.name);
            _currentWeaponSlot += 1;
            ArrayUtility.Insert(ref weaponInv, _currentWeaponSlot, weapon);
        }
        else {
            // Debug.Log("Player has too many weapons");
            // Debug.Log("Dropping " + weaponInv[_currentWeaponSlot].gameObject.name);
            weaponInv[_currentWeaponSlot].gameObject.SetActive(true);
            weaponInv[_currentWeaponSlot].transform.position = transform.position;
            weaponInv[_currentWeaponSlot] = weapon;
        }
        UpdateCurrentWeapon();
        weapon.gameObject.SetActive(false);
        // Destroy(weapon.gameObject);
    }

    private void PlayerSwitchWeapon(){
        Debug.Log("Player is switching weapons");
        if (weaponInv.Length <= 1){return;}
        _currentWeaponSlot = (_currentWeaponSlot + weaponSlotCountMax + 1) % weaponSlotCountMax;
        UpdateCurrentWeapon();
    }

    private void UpdateCurrentWeapon(){
        _currentWeapon = weaponInv[_currentWeaponSlot];
    }
}
