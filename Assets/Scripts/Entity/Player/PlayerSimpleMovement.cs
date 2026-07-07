using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEditor;

using Default;
using UnityEngine.InputSystem;
public class PlayerSimpleMovement : MonoBehaviour
{
    [SerializeField] private float speed = 10;
    [SerializeField] private LayerMask layerMask = GlobalLayerMask.EnvironmentLayer;
    
    private Vector2 m_move;

    private Vector3 _moveDir;
    public Vector2 MoveDirection{
        get{return _moveDir;}
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        m_move = context.ReadValue<Vector2>();
        Debug.Log("Move: " + m_move);
    }

    // Update is called once per frame
    void Update()
    {
        MovementHandler();
    }
    
    private void MovementHandler(){
        // float moveX = 0f;
        // float moveY = 0f;
        
        // if (Input.GetKey(KeyCode.W)){
        //     moveY = 1;
        // }
        // if (Input.GetKey(KeyCode.S)){
        //     moveY = -1;
        // }
        // if (Input.GetKey(KeyCode.A)){
        //     moveX = -1;
        // }
        // if (Input.GetKey(KeyCode.D)){
        //     moveX = 1;
        // }

        float moveX = m_move.x;
        float moveY = m_move.y;

        bool isIdle = moveX == 0 && moveY == 0;
        if (isIdle)
        {
            return;
        }
        else
        {
            _moveDir = new Vector2(moveX, moveY);

            Collider2D collider2D = GetComponent<Collider2D>();
            RaycastHit2D[] raycastHits = new RaycastHit2D[3];
            int ObjectHitByRaycast = collider2D.Raycast(_moveDir, raycastHits, _moveDir.magnitude / 2f + speed * Time.deltaTime, layerMask);
            // print("ObjectHitByRaycast: " + ObjectHitByRaycast);

            if (ObjectHitByRaycast == 0)
            {
                transform.position += speed * Time.deltaTime * _moveDir.normalized;
            }
            else if (ObjectHitByRaycast != 0)
            {
                // Debug.Log("Player is stuck against " + raycastHits[0].collider.name);
                if (raycastHits[0].normal.x == 0)
                {
                    transform.position += _moveDir.normalized.x * speed * Time.deltaTime * Vector3.right;
                }
                else if (raycastHits[0].normal.y == 0)
                {
                    transform.position += _moveDir.normalized.y * speed * Time.deltaTime * Vector3.up;
                }
            }

        }
    }
}
