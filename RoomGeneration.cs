using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType {
    Room = 0,
    Gate = 1
};
public class RoomGeneration : MonoBehaviour
{

    public GameObject floorTile;
    public GameObject gateTile;
    public GameObject torchTile;
    public GameObject wallTile;
    public RoomType roomType;
    public int length;
    public int width;
    private Vector2 _center;
    private int _length;
    private int _width;
    public bool isVertical = false;

    // Start is called before the first frame update
    void Start()
    {

        switch(roomType){
            case RoomType.Gate : {this.name = "Gate"; break;}
        }
        
        if((roomType == RoomType.Room)){
            _length = length;
            _width = width;
        }

        else if ((roomType == RoomType.Gate)){
            if (isVertical){
                _length = width;
                _width = length;
            }
            else if (!isVertical){
                _length = length;
                _width = width;
            }
        }
        
        _center = transform.position;
        Vector2Int pos;

        int halfLength = (_length - 1) / 2;
        int halfWidth = (_width - 1) / 2;

        for (int w = 0 ; w < _width ; w++){
            for (int l = 0 ; l < _length ; l++){
                pos = new Vector2Int(l - halfLength, halfWidth - w);
                Instantiate(floorTile, _center + pos, Quaternion.identity, transform);
            }
        }

    }

    public int getLength(){return _length;}
    public int getWidth(){return _width;}
}
