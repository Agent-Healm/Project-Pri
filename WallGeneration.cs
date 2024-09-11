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

        _length = _roomGen.length;
        _width = _roomGen.width;
        _wallTile = _roomGen.wallTile;
        _gateTile = _roomGen.gateTile;
        _roomType = _roomGen.roomType;

        _halfLength = (_length - 1) / 2;
        _halfWidth = (_width - 1) / 2;

        if(_roomType == RoomType.Room){
            pos = new Vector2Int(- 1 - _halfLength, _halfWidth + 1);
            GenerateWall(_wallTile, pos); // Top Left Corner

            pos = new Vector2Int(_length - _halfLength, _halfWidth + 1);
            GenerateWall(_wallTile, pos); // Top Right Corner

            pos = new Vector2Int(_length - _halfLength, _halfWidth - _width);
            GenerateWall(_wallTile, pos); // Bottom Right Corner
                        
            pos = new Vector2Int(- 1 - _halfLength, _halfWidth - _width);
            GenerateWall(_wallTile, pos); // Bottom Left Corner
        }

        // GenerateGate(Vector2.right);
        // GenerateGate(Vector2.right);
        // GenerateGate(Vector2.up);
        // GenerateGate(Vector2.right);

        // Debug.Log(gateEast);
        // if (gateEast){GenerateGate(Vector2.right);}
        // if (gateSouth){GenerateGate(Vector2.down);}
        // if (gateWest){GenerateGate(Vector2.left);}
        // if (gateNorth){GenerateGate(Vector2.up);}

        GenerateGate(Vector2.right, gateEast);
        GenerateGate(Vector2.down, gateSouth);
        GenerateGate(Vector2.left, gateWest);
        GenerateGate(Vector2.up, gateNorth);
    }
    private void GenerateWall(GameObject tile, Vector2Int vec2i){
            Instantiate(tile, _center + vec2i, 
                        Quaternion.identity, transform);
        }

    public void GenerateGate(Vector2 vec2, bool hasGate = false){

        int i;
        Vector2Int pos;
        GameObject __tile = hasGate? _gateTile : _wallTile;
        // Debug.Log("Vector2 : " + vec2);
        // Debug.Log("halfLength : " + halfLength);

        // for some reason, the length and width is 0
        if (vec2 == Vector2.right){ 
            //EAST
            for (i = 0 ; i < _width ; i++){
                pos = new Vector2Int(_length - _halfLength, _halfWidth - i);
                GenerateWall(__tile, pos);
            }
        }
        else if (vec2 == Vector2.left){
            //WEST
            for (i = 0 ; i < _width ; i++){
                pos = new Vector2Int(- _halfLength - 1, _halfWidth - i);
                GenerateWall(__tile, pos);
            }
        }
        else if(vec2 == Vector2.up){
                //NORTH
                for (i = 0 ; i < _length ; i++){
                    pos = new Vector2Int(i - _halfLength, _halfWidth + 1);
                    GenerateWall(__tile, pos);
                }
        }
        else if(vec2 == Vector2.down){
            //SOUTH
            for (i = 0 ; i < _length ; i++){
                pos = new Vector2Int(i - _halfLength, _halfWidth - _width);
                GenerateWall(__tile, pos);
            }
        }
        
    }

    // let gate close the room 
    // if there is gate, generate wall with gate in the middle
    // otherwise, generate solid wall

}
