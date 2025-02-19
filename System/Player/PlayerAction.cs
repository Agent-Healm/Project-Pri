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
        if (other.gameObject.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            _itemNearby = true;
            _other = other;
        };
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            _itemNearby = false;
            _other = null;
        };
    }

    private void ActionHandler(){
        if (Input.GetKey(KeyCode.R)){
            if (_itemNearby){
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
