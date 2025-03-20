using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using DefaultLayer;
public class PlayerSimpleMovement : MonoBehaviour
{
    public float speed = 10;
    [SerializeField] private LayerMask layerMask = BitLayer.EnvironmentLayer();
    private Vector3 _moveDir;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandler();
    }
    
    private void MovementHandler(){
        float moveX = 0f;
        float moveY = 0f;
        if (Input.GetKey(KeyCode.W)){
            moveY = 1;
        }
        if (Input.GetKey(KeyCode.S)){
            moveY = -1;
        }
        if (Input.GetKey(KeyCode.A)){
            moveX = -1;
        }
        if (Input.GetKey(KeyCode.D)){
            moveX = 1;
        }

        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle){
            return;
        }
        else {
            _moveDir = new Vector2(moveX, moveY);
            RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, _moveDir, _moveDir.magnitude / 2f + speed * Time.deltaTime, layerMask);
            if (raycastHit.collider == null){
                transform.position +=  speed * Time.deltaTime * _moveDir.normalized;
            }
            else if (raycastHit.collider != null){
                // Debug.Log("Player is stuck against " + raycastHit.collider.name);
                if (raycastHit.normal.x == 0){
                    // transform.position += Vector3.right * moveX * speed * Time.deltaTime;
                    transform.position += speed * Time.deltaTime * Vector3.right * _moveDir.normalized.x;
                }
                else if (raycastHit.normal.y == 0){
                    // transform.position += Vector3.up * moveY * speed * Time.deltaTime;
                    transform.position += speed * Time.deltaTime * Vector3.up * _moveDir.normalized.y ;
                }
            }
        }
    }
    public Vector2 getMoveDir(){
        return _moveDir;
    }
}
