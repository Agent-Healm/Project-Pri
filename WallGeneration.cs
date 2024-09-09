using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    private GameObject _wallTile;
    private GameObject _gateTile;
    private int _length;
    private int _width;
    private RoomGeneration _roomGen;
    private Vector2 _center;
    private RoomType _roomType;

    // Start is called before the first frame update
    void Start()
    {

        _center = transform.position;
        int i;
        Vector2Int pos;
        _roomGen = this.GetComponent<RoomGeneration>();

        _length = _roomGen.length;
        _width = _roomGen.width;
        _wallTile = _roomGen.wallTile;
        _gateTile = _roomGen.gateTile;
        _roomType = _roomGen.roomType;

        int halfLength = (_length - 1) / 2;
        int halfWidth = (_width - 1) / 2;

        for (i = 0 ; i < _length ; i++){
            pos = new Vector2Int(i - halfLength, halfWidth + 1);
            GenerateWall(_wallTile, pos);

            pos = new Vector2Int(i - halfLength, halfWidth - _width);
            GenerateWall(_wallTile, pos);
        }

        for (i = 0 ; i < _width ; i++){
            pos = new Vector2Int(_length - halfLength, halfWidth - i);
            GenerateWall(_gateTile, pos);

            pos = new Vector2Int(- halfLength - 1, halfWidth - i);
            GenerateWall(_gateTile, pos);
        }

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

        void GenerateWall(GameObject tile, Vector2Int vec2i){
            Instantiate(tile, _center + vec2i, 
                        Quaternion.identity, transform);
        }

    }
    
}
