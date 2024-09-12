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
    public bool gateEast;
    public bool gateSouth;
    public bool gateWest;
    public bool gateNorth;
    private GameObject _wallTile;
    private GameObject _gateTile;
    private int _length;
    private int _width;
    private RoomGeneration _roomGen;
    private Vector2 _center;
    private RoomType _roomType;
    private int _halfLength;
    private int _halfWidth;

    // Start is called before the first frame update
    void Start()
    {

        _center = transform.position;
        Vector2Int pos;
        _roomGen = this.GetComponent<RoomGeneration>();

        _length = _roomGen.getLength();
        _width = _roomGen.getWidth();
        
        _wallTile = _roomGen.wallTile;
        _gateTile = _roomGen.gateTile;
        _roomType = _roomGen.roomType;

        // offset for even sized room
        _halfLength = (_length - 1) / 2;
        _halfWidth = (_width - 1) / 2;

        if (_roomType == RoomType.Room){
            pos = new Vector2Int(- 1 - _halfLength, _halfWidth + 1);
            GenerateWall(_wallTile, pos); // Top Left Corner

            pos = new Vector2Int(_length - _halfLength, _halfWidth + 1);
            GenerateWall(_wallTile, pos); // Top Right Corner

            pos = new Vector2Int(_length - _halfLength, _halfWidth - _width);
            GenerateWall(_wallTile, pos); // Bottom Right Corner
                        
            pos = new Vector2Int(- 1 - _halfLength, _halfWidth - _width);
            GenerateWall(_wallTile, pos); // Bottom Left Corner

        }
        
        if (_roomType == RoomType.Room){
            GenerateGate(Vector2.right, gateEast);
            GenerateGate(Vector2.left, gateWest);
            GenerateGate(Vector2.down, gateSouth);
            GenerateGate(Vector2.up, gateNorth);
        }
        else if (_roomType == RoomType.Gate){
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
    private void GenerateWall(GameObject tile, Vector2Int vec2i){
        Instantiate(tile, _center + vec2i, 
                    Quaternion.identity, transform);
    }

    public void GenerateGate(Vector2 vec2, bool hasGate = false){

        int i;
        Vector2Int pos;
        GameObject __tile = hasGate? _gateTile : _wallTile;
        
        // if (vec2 == Vector2.right){ 
        //     //EAST
        //     for (i = 0 ; i < _width ; i++){
        //         pos = new Vector2Int(- _halfLength - 1 + _length + 1 , _halfWidth - i);
        //         GenerateWall(__tile, pos);
        //     }
        // }
        // else if (vec2 == Vector2.left){
        //     //WEST
        //     for (i = 0 ; i < _width ; i++){
        //         pos = new Vector2Int(- _halfLength - 1, _halfWidth - i);
        //         GenerateWall(__tile, pos);
        //     }
        // }
        // else 
        // if(vec2 == Vector2.up){
        //     //NORTH
        //     for (i = 0 ; i < _length ; i++){
        //         pos = new Vector2Int(i - _halfLength, _halfWidth + 1);
        //         GenerateWall(__tile, pos);
        //     }
        // }
        // else if(vec2 == Vector2.down){
        //     //SOUTH
        //     for (i = 0 ; i < _length ; i++){
        //         pos = new Vector2Int(i - _halfLength, _halfWidth  + 1 - _width - 1);
        //         GenerateWall(__tile, pos);
        //     }
        // }
        

        // if (vec2.y == 0.0f){
        //     int __xPos = - _halfLength - 1;
        //     if (vec2.x == 1.0f){
        //         __xPos += (_length + 1);
        //     }
        //     // HORIZONTAL
        //     for (i = 0 ; i < _width ; i++){
        //         pos = new Vector2Int(__xPos, _halfWidth - i);
        //         GenerateWall(__tile, pos);
        //     }

        // }
        // else 
        if (vec2.x == 0.0f){
            int __yPos = _halfWidth + 1;
            if (vec2.y == -1.0f){
                __yPos -= (_width + 1);
            }
            // VERTICAL
            // if(vec2 == Vector2.up){
            //     //NORTH
            //     for (i = 0 ; i < _length ; i++){
            //         pos = new Vector2Int(i - _halfLength, _halfWidth + 1);
            //         GenerateWall(__tile, pos);
            //     }
            // }
            // else if(vec2 == Vector2.down){
            //     //SOUTH
            //     for (i = 0 ; i < _length ; i++){
            //         pos = new Vector2Int(i - _halfLength, _halfWidth  + 1 - _width - 1);
            //         GenerateWall(__tile, pos);
            //     }
            // }

            for (i = 0 ; i < _length ; i++){
                pos = new Vector2Int(i - _halfLength, __yPos);
                GenerateWall(__tile, pos);
            }

        }
    }

}
