using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallGeneration : MonoBehaviour
{
    private GameObject wallTile;
    private int _length;
    private int _width;
    private RoomGeneration _roomGen;
    private Vector2 _center;
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

        int halfLength = (_length - 1) / 2;
        int halfWidth = (_width - 1) / 2;

        for (i = 0 ; i <= _length ; i++){
            pos = new Vector2Int(i - halfLength, halfWidth + 1);
            Instantiate(wallTile, _center + pos, Quaternion.identity, transform);
        }
        for (i = 0 ; i <= _width ; i++){
            pos = new Vector2Int(_length - halfLength, halfWidth - i);
            Instantiate(wallTile, _center + pos, Quaternion.identity, transform);
        }
        for (i = 1 ; i <= (_length + 1) ; i++){
            pos = new Vector2Int(_length - halfLength - i, halfWidth - _width);
            Instantiate(wallTile, _center + pos, Quaternion.identity, transform);
        }
        for (i = 1 ; i <= (_width + 1) ; i++){
            pos = new Vector2Int(-1 - halfLength, halfWidth - _width + i);
            Instantiate(wallTile, _center + pos, Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
