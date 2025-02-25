using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerAction : MonoBehaviour
{

    // private bool _itemNearby;
    private Collider2D _other;

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
            // _itemNearby = true;
            _other = other;
            PlayerInteract();
        };
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            // _itemNearby = false;
            _other = null;
            PlayerDefaultAction();
        };
    }

    private void ActionHandler(){
        if (Input.GetKeyDown(KeyCode.R)){
            // if (_itemNearby){
            //     if (_other.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            //         interactable.OnPickup(this);
            //     }
            // }
            // else {
            //     if (this.TryGetComponent<PlayerWeaponSlot>(out PlayerWeaponSlot playerWeaponSlot)){}
            //     playerWeaponSlot.WeaponAction();
            //     // PlayerDefaultAction();
            // }
            // OnPlayerAction.Invoke(this, EventArgs.Empty);
            onPlayerAction?.Invoke(this);
        }
    }
    private void PlayerInteract(){
        // if (_itemNearby){
        if (_other.TryGetComponent<IInteractAble>(out IInteractAble interactable)){
            onPlayerAction = interactable.OnPickup;
        }
        // }
    }
    private void PlayerDefaultAction(){
        if (this.TryGetComponent<PlayerWeaponSlot>(out PlayerWeaponSlot playerWeaponSlot)){
            // playerWeaponSlot.WeaponAction();
            onPlayerAction = playerWeaponSlot.WeaponAction;
        }
    }
}
