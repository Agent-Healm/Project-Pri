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
        WeaponActionHandler();
    }
    private void WeaponActionHandler(){
        if (Input.GetKeyDown(KeyCode.T)){
            PlayerSwitchWeapon();
        }
        if (Input.GetKeyDown(KeyCode.Y)){
            // Debug.Log("Player is toggling another mode");
            _currentWeapon.SwitchWeaponMode();
        }
        // if (Input.GetKey(KeyCode.R)){
        //     PlayerAction();
        // }
        if (Input.GetKeyDown(KeyCode.G)){
            // Debug.Log("Player is dropping weapon");
            if (weaponInv.Length == 0){
                // Debug.Log("Player has no weapons");
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
    // private void OnTriggerEnter2D(Collider2D other){
    //     if ((other.gameObject.layer == 12) || (other.gameObject.layer == 13)){
    //         // Debug.Log("Weapons nearby player");
    //         _itemNearby = true;
    //         _other = other;
    //     }
    // }
    // private void OnTriggerExit2D(Collider2D other){
    //     if ((other.gameObject.layer == 12) || (other.gameObject.layer == 13)){
    //         // Debug.Log("Weapons are no longer nearby player");
    //         _itemNearby = false;
    //         _other = null;
    //     }
    // }

    private void XPlayerAction(){
        if (_itemNearby){
            // Debug.Log("Player could pickup an item");
            if(_other.TryGetComponent<Potion>(out Potion potion)){
                // potion.OnInteract();
                // OnInteract(potion);
            }
            // Weapon weapon = _other.GetComponent<Weapon>();
            else if (_other.TryGetComponent<Weapon>(out Weapon weapon)){
                AddToWeaponSlots(weapon);
            }
        }
        else {
            // if (weaponInv.Length == 0){
            //     Debug.Log("Player has no weapons");
            //     return;
            // }

            // if (_currentWeapon.AttemptAction(ref _playerMana._energyPoint)){
            //     _currentWeapon.Action(_facing, transform.position);
            // }
            WeaponAction();
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
            // Debug.Log("Player picked up " + weapon.name);
            _currentWeaponSlot += 1;
            ArrayUtility.Insert(ref weaponInv, _currentWeaponSlot, weapon);
        }
        else {
            // Debug.Log("Player has too many weapons");
            // Debug.Log("Dropping " + weaponInv[_currentWeaponSlot].gameObject.name);
            // PlayerDropWeapon();
            // weaponInv[_currentWeaponSlot].gameObject.SetActive(true);
            // weaponInv[_currentWeaponSlot].transform.position = transform.position;
            PlayerDropCurrentWeapon();
            weaponInv[_currentWeaponSlot] = weapon;
        }
        UpdateCurrentWeapon();
        // weapon.gameObject.SetActive(false);
        // Destroy(weapon.gameObject);
    }

    private void PlayerSwitchWeapon(){
        // Debug.Log("Player is switching weapons");
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
