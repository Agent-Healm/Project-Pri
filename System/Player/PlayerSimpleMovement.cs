using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PlayerSimpleMovement : MonoBehaviour
{
    public float speed = 10;
    [SerializeField] private LayerMask layerMask;
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
                transform.position += _moveDir.normalized * speed * Time.deltaTime;
            }
            else if (raycastHit.collider != null){
                Debug.Log("Player is stuck against " + raycastHit.collider.name);
                if (raycastHit.normal.x == 0){
                    // transform.position += Vector3.right * moveX * speed * Time.deltaTime;
                    transform.position += Vector3.right * _moveDir.normalized.x * speed * Time.deltaTime;
                }
                else if (raycastHit.normal.y == 0){
                    // transform.position += Vector3.up * moveY * speed * Time.deltaTime;
                    transform.position += Vector3.up * _moveDir.normalized.y * speed * Time.deltaTime;
                }
            }
        }
    }
    public Vector2 getMoveDir(){
        return _moveDir;
    }
}
