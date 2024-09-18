using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType {
    Room = 0,
    Gate = 1
};
public class RoomGeneration : MonoBehaviour
{
    public bool isHorizontal = false;
    public int length;
    public int width;
    private Vector2 _center;
    public GameObject roomIcon;
    public RoomType roomType;
    private int _length;
    private int _width;

    // Start is called before the first frame update
    void Awake(){
        switch(roomType){
            case RoomType.Gate : {this.name = "Gate"; break;}
        }
        
        _length = length;
        _width = width;
        if ((roomType == RoomType.Gate) && isHorizontal){
            _length = width;
            _width = length;
        }

    }
    void Start()
    {

        _center = transform.position;
        Vector2Int pos;
        
        // offset for even sized room
        int halfLength = (_length - 1) / 2;
        int halfWidth = (_width - 1) / 2;

        for (int w = 0 ; w < _width ; w++){
            for (int l = 0 ; l < _length ; l++){
                pos = new Vector2Int(l - halfLength, halfWidth - w);
                Instantiate(roomIcon, _center + pos, Quaternion.identity, transform);
            }
        }

    }

    public int getLength(){return _length;}
    public int getWidth(){return _width;}
}
