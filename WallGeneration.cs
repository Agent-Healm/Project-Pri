using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public enum GateOrientation {
//     East = 0,
//     South = 1,
//     West = 2,
//     North = 3
// }

public class WallGeneration : MonoBehaviour
{
    public bool gateEast = false;
    public bool gateSouth = false;
    public bool gateWest = false;
    public bool gateNorth = false;
    
    private GameObject _wallTile;
    private GameObject _gateTile;
    private GameObject _nullTile;
    private float _halfLength;
    private float _halfWidth;
    private int _length;
    private int _width;
    private RoomUtility.RoomType _roomType;
    private Vector2 _center;
    private RoomGeneration _roomGen;
    // private TextureTheme instance;

    // Start is called before the first frame update
    void Start()
    {

        Vector2 pos;

        _roomGen = this.GetComponent<RoomGeneration>();
        _length = _roomGen._length;
        _width = _roomGen._width;
        _roomType = _roomGen.roomType;
        
        _center = transform.position;
        _wallTile = TextureTheme.instance.wallTile;
        _gateTile = TextureTheme.instance.gateTile;
        _nullTile = TextureTheme.instance.nullTile;
        _halfLength = (_length - 1.0f) / 2;
        _halfWidth = (_width - 1.0f) / 2;

        // Debug.Log("room size is : " + _length + " X " + _width);

        if (_roomType == RoomUtility.RoomType.Room){
            pos = new Vector2(- 1 - _halfLength, _halfWidth + 1);
            GenerateWall(_wallTile, pos); // Top Left Corner

            pos = new Vector2(_length - _halfLength, _halfWidth + 1);
            GenerateWall(_wallTile, pos); // Top Right Corner

            pos = new Vector2(_length - _halfLength, _halfWidth - _width);
            GenerateWall(_wallTile, pos); // Bottom Right Corner
                        
            pos = new Vector2(- 1 - _halfLength, _halfWidth - _width);
            GenerateWall(_wallTile, pos); // Bottom Left Corner

        }
        
        if (_roomType == RoomUtility.RoomType.Room){
            GenerateGate(Vector2.right, gateEast);
            GenerateGate(Vector2.left, gateWest);
            GenerateGate(Vector2.down, gateSouth);
            GenerateGate(Vector2.up, gateNorth);
        }
        else if (_roomType == RoomUtility.RoomType.Gate){
            // if both direction is not a gate, generate wall
            if(!gateEast && !gateWest){
                GenerateGate(Vector2.right, gateEast);
                GenerateGate(Vector2.left, gateWest);
            }
            if(!gateSouth && !gateNorth){
                GenerateGate(Vector2.down, gateSouth);
                GenerateGate(Vector2.up, gateNorth);
            }
        }
    }
    
    public void GenerateGate(Vector2 vec2, bool hasGate = false){

        int i;
        Vector2 pos;
        GameObject __tile = hasGate? _gateTile : _wallTile;

        if (__tile == null){
            __tile = _nullTile;
        }
        
        if (vec2.y == 0.0f){
            // float __xPos = - _halfLength - 1.0f;
            // if (vec2.x == 1.0f){
            //     __xPos += (_length + 1.0f);
            // }
            // HORIZONTAL
            // for (i = 0 ; i < _width ; i++){
            //     pos = new Vector2(__xPos, _halfWidth - i);
            //     GenerateWall(__tile, pos);
            // }

            float __posX;
            if(vec2 == Vector2.left){   
                __posX = - _halfLength - 1.0f;
                // for (i = 0 ; i < _width ; i++){
                //     pos = new Vector2(__xPos, _halfWidth - i);
                //     GenerateWall(__tile, pos);
                // }
            }
            else if(vec2 == Vector2.right){
                __posX = _length - _halfLength;
            }
            for (i = 0 ; i < _width ; i++){
                pos = new Vector2(__xPos, _halfWidth - i);
                GenerateWall(__tile, pos);
            }
        }
        else if (vec2.x == 0.0f){
            // VERTICAL

            float __posY;
            if(vec2 == Vector2.up){
                __posY = _halfWidth + 1.0f;
                // for (i = 0 ; i < _length ; i++){
                //     pos = new Vector2(i - _halfLength, __yPos);
                //     GenerateWall(__tile, pos);
                // }
            }
            else if (vec2 == Vector2.down){
                __posY = _halfWidth - _width;
            }
            for (i = 0 ; i < _length ; i++){
                pos = new Vector2(i - _halfLength, __yPos);
                GenerateWall(__tile, pos);
            }
        }

    }
    private void GenerateWall(GameObject tile, Vector2 vec2){
        Instantiate(tile, _center + vec2, 
                    Quaternion.identity, transform);
    }
    public void GateReset(){
        gateEast = false;
        gateSouth = false;
        gateWest = false;
        gateNorth = false;
    }
    public void setGate(Vector2 vec2, bool isGate){
        if (vec2 == Vector2.up){gateNorth = isGate; return;}
        if (vec2 == Vector2.right){gateEast = isGate; return;}
        if (vec2 == Vector2.down){gateSouth = isGate; return;}
        if (vec2 == Vector2.left){gateWest = isGate; return;}
    }

}
