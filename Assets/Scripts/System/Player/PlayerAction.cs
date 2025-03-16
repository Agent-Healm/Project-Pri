using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAction : MonoBehaviour
{

    // private bool _itemNearby;
    private Collider2D _interactable;

    // private PlayerWeaponSlot _playerWeaponSlot;

    private delegate void OnPlayerAction(PlayerAction playerAction);
    private event OnPlayerAction onPlayerAction;
    
    // private event EventHandler OnPlayerAction;
    // private OnPlayerAction OnPlayerAction;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ActionHandler();
    }
    
    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            _interactable = other;
            PlayerInteractItem();
        };
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            _interactable = null;
            PlayerInteractWeapon();
        };
    }

    private void ActionHandler(){
        if (Input.GetKeyDown(KeyCode.R)){
            // OnPlayerAction.Invoke(this, EventArgs.Empty);
            onPlayerAction?.Invoke(this);
        }
    }
    private void PlayerInteractItem(){
        if (_interactable.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            onPlayerAction = interactable.OnPickup;
        }
    }
    private void PlayerInteractWeapon(){
        if (this.TryGetComponent<PlayerWeaponSlot>(out PlayerWeaponSlot playerWeaponSlot)){
            onPlayerAction = playerWeaponSlot.WeaponAction;
        }
    }
}
