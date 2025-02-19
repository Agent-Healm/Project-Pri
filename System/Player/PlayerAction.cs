using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private bool _itemNearby;
    private Collider2D _other;

    private PlayerWeaponSlot _playerWeaponSlot;
    // Start is called before the first frame update
    void Start()
    {
        _playerWeaponSlot = this.GetComponent<PlayerWeaponSlot>();
    }

    // Update is called once per frame
    void Update()
    {
        ActionHandler();
    }
    
    private void OnTriggerEnter2D(Collider2D other){
        // if ((other.gameObject.layer == 12) || (other.gameObject.layer == 13)){
        //     // Debug.Log("Weapons nearby player");
        //     _itemNearby = true;
        //     _other = other;
        // }
        if (other.gameObject.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            _itemNearby = true;
            _other = other;
        };
    }
    private void OnTriggerExit2D(Collider2D other){
        // if ((other.gameObject.layer == 12) || (other.gameObject.layer == 13)){
        //     // Debug.Log("Weapons are no longer nearby player");
        //     _itemNearby = false;
        //     _other = null;
        // }
        if (other.gameObject.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            _itemNearby = false;
            _other = null;
        };
    }

    private void ActionHandler(){
        if (Input.GetKey(KeyCode.R)){
            // PlayerActionHandler();
            if (_itemNearby){
                // PlayerPickupWeapon();
                // Debug.Log("Player could pickup an item");
                // if(_other.TryGetComponent<Potion>(out Potion potion)){
                //     potion.OnPickup(this);
                //     // OnInteract(potion);
                // }
                // // Weapon weapon = _other.GetComponent<Weapon>();
                // else if (_other.TryGetComponent<Weapon>(out Weapon weapon)){
                //     // _playerWeaponSlot.AddToWeaponSlots(weapon);
                //     weapon.OnPickup(this);
                // }
                if (_other.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
                    interactable.OnPickup(this);
                }
            }
            else {
                _playerWeaponSlot.WeaponAction();
            }
        }
    }
}
