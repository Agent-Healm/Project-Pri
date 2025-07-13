using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAim : MonoBehaviour
{
    // [SerializeField] private UnityEvent<Vector2> unityEvent;

    private PlayerAI _playerAI;
    private PlayerSimpleMovement _psm;
    private Vector2 _faceDir = Vector2.right;
    
    // Start is called before the first frame update
    void Start(){
        _playerAI = this.GetComponent<PlayerAI>();
        _psm = this.GetComponent<PlayerSimpleMovement>();
        // unityEvent = new UnityEvent();
    }

    // Update is called once per frame
    void Update(){
        if (_playerAI.AimingAtTarget){
            _faceDir = _playerAI.AutoaimDir;
        }
        else {
            _faceDir = _psm.MoveDirection;
        }
    }

    public Vector2 GetCurrentFaceDir(){
        return _faceDir;
    }

}