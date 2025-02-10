using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAim : MonoBehaviour
{
    
    private PlayerAI _playerAI;
    private PlayerSimpleMovement _psm;
    private Vector2 _faceDir = Vector2.right;
    
    // Start is called before the first frame update
    void Start(){
        _playerAI = this.GetComponent<PlayerAI>();
        _psm = this.GetComponent<PlayerSimpleMovement>();
    }

    // Update is called once per frame
    void Update(){
        if (_playerAI.isAiming()){
            _faceDir = _playerAI.getAimDir();
        }
        else {
            _faceDir = _psm.getMoveDir();
        }
    }

    public Vector2 GetCurrenetFaceDir(){
        return _faceDir;
    }
}
