using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.InputSystem;
public class PlayerAction : MonoBehaviour
{

    // private bool _itemNearby;
    private Collider2D _interactable;

    // private PlayerWeaponSlot _playerWeaponSlot;

    private delegate void OnPlayerAction(PlayerAction playerAction);
    private event OnPlayerAction onPlayerAction;
    
    // private event EventHandler OnPlayerAction;
    // private OnPlayerAction OnPlayerAction;

    [SerializeField] private bool _isInteracting = false;

    public void OnInteract(InputAction.CallbackContext context){
        switch (context.phase)
        {
            case InputActionPhase.Started:
                _isInteracting = true;
                break;
            case InputActionPhase.Performed:
                break;
            case InputActionPhase.Canceled:
                _isInteracting = false;
                break;
        }

        // Debug.Log($"Testing Interact: {context.phase}");

    }

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
        if (other.gameObject.TryGetComponent(out IInteractAble interactable)){
            _interactable = other;
            PlayerInteractItem();
        };
    }
    private void OnTriggerExit2D(Collider2D other){
        if (other.gameObject.TryGetComponent(out IInteractAble interactable)){
            _interactable = null;
            PlayerInteractWeapon();
        };
    }

    private void ActionHandler(){
        // if (Input.GetKey(KeyCode.R)){
        //     // OnPlayerAction.Invoke(this, EventArgs.Empty);
        //     onPlayerAction?.Invoke(this);
        // }
        if (_isInteracting){
                onPlayerAction?.Invoke(this);
        }
    }

    private void PlayerInteractItem(){
        if (_interactable.TryGetComponent(out IInteractAble interactable)){
            onPlayerAction = interactable.OnPickup;
        }
    }
    private void PlayerInteractWeapon(){
        if (this.TryGetComponent(out PlayerWeaponSlot playerWeaponSlot)){
            onPlayerAction = playerWeaponSlot.WeaponAction;
        }
    }
}
