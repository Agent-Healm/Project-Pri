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
    private GameObject _wallTile;
    private GameObject _gateTile;
    private int _length;
    private int _width;
    private RoomGeneration _roomGen;
    private Vector2 _center;
    private RoomType _roomType;
    private int halfLength;
    private int halfWidth;

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

        halfLength = (_length - 1) / 2;
        halfWidth = (_width - 1) / 2;

        if(_roomType == RoomType.Room){
            pos = new Vector2Int(- 1 - halfLength, halfWidth + 1);
            GenerateWall(_wallTile, pos); // Top Left Corner

            pos = new Vector2Int(_length - halfLength, halfWidth + 1);
            GenerateWall(_wallTile, pos); // Top Right Corner

            pos = new Vector2Int(_length - halfLength, halfWidth - _width);
            GenerateWall(_wallTile, pos); // Bottom Right Corner
                        
            pos = new Vector2Int(- 1 - halfLength, halfWidth - _width);
            GenerateWall(_wallTile, pos); // Bottom Left Corner
        }

    }
    private void GenerateWall(GameObject tile, Vector2Int vec2i){
            Instantiate(tile, _center + vec2i, 
                        Quaternion.identity, transform);
        }

    public void GenerateGate(Vector2 vec2, bool hasGate = false){

        int i;
        Vector2Int pos;
        // Debug.Log("Vector2 : " + vec2);
        // Debug.Log("halfLength : " + halfLength);

        // for some reason, the length and width is 0
        if (vec2.x == 1.0f){ 
            //EAST
            for (i = 0 ; i < _width ; i++){
                pos = new Vector2Int(_length - halfLength, halfWidth - i);
                GenerateWall(_wallTile, pos);
            }
        }
        else if (vec2.x == -1.0f){
            //WEST
            for (i = 0 ; i < _width ; i++){
                pos = new Vector2Int(- halfLength - 1, halfWidth - i);
                GenerateWall(_wallTile, pos);
            }
        }
        else if(vec2.x == 0.0f){
            if (vec2.y == 1.0f){
                //NORTH
                for (i = 0 ; i < _length ; i++){
                    pos = new Vector2Int(i - halfLength, halfWidth + 1);
                    GenerateWall(_wallTile, pos);
                }
            }
            else if(vec2.y == -1.0f){
                //SOUTH
                for (i = 0 ; i < _length ; i++){
                    pos = new Vector2Int(i - halfLength, halfWidth - _width);
                    GenerateWall(_wallTile, pos);
                }
            }
        }
        
    }

    // let gate close the room 
    // if there is gate, generate wall with gate in the middle
    // otherwise, generate solid wall

}
