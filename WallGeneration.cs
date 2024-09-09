using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    private GameObject wallTile;
    private GameObject gateTile;
    private int _length;
    private int _width;
    private RoomGeneration _roomGen;
    private Vector2 _center;
    private RoomType roomType;

    // Start is called before the first frame update
    void Start()
    {

        _center = transform.position;
        int i;
        Vector2Int pos;
        _roomGen = GameObject.Find("Wall").GetComponent<RoomGeneration>();

        _length = _roomGen.length;
        _width = _roomGen.width;
        wallTile = _roomGen.wallTile;
        gateTile = _roomGen.gateTile;
        roomType = _roomGen.roomType;

        int halfLength = (_length - 1) / 2;
        int halfWidth = (_width - 1) / 2;

        for (i = 0 ; i < _length ; i++){
            pos = new Vector2Int(i - halfLength, halfWidth + 1);
            GenerateWall(wallTile, pos);

            pos = new Vector2Int(i - halfLength, halfWidth - _width);
            GenerateWall(wallTile, pos);
        }

        for (i = 0 ; i < _width ; i++){
            pos = new Vector2Int(_length - halfLength, halfWidth - i);
            GenerateWall(gateTile, pos);

            pos = new Vector2Int(- halfLength - 1, halfWidth - i);
            GenerateWall(gateTile, pos);
        }

        if(roomType == RoomType.Room){
            pos = new Vector2Int(- 1 - halfLength, halfWidth + 1);
            GenerateWall(wallTile, pos); // Top Left Corner

            pos = new Vector2Int(_length - halfLength, halfWidth + 1);
            GenerateWall(wallTile, pos); // Top Right Corner

            pos = new Vector2Int(_length - halfLength, halfWidth - _width);
            GenerateWall(wallTile, pos); // Bottom Right Corner
                        
            pos = new Vector2Int(- 1 - halfLength, halfWidth - _width);
            GenerateWall(wallTile, pos); // Bottom Left Corner
        }

        void GenerateWall(GameObject tile, Vector2Int vec2i){
            Instantiate(tile, _center + vec2i, 
                        Quaternion.identity, transform);
        }

    }

    // Update is called once per frame
    // void Update()
    // {
        
    // }
    
}
