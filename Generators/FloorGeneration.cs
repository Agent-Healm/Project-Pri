using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorGeneration : MonoBehaviour
{
    public bool isVertical = false;
    public int length;
    public int width;
    private Vector2 _center;
    public GameObject roomIcon;
    public RoomUtility.LayoutType layoutType;
    
    // readonly from public view, set-able in private view
    public int _length {get; private set;}
    public int _width {get; private set;}

    void Awake(){
        // switch(layoutType){
        //     case RoomUtility.LayoutType.Path : {this.name = "Path"; break;}
        // }
        this.name = layoutType switch {
            RoomUtility.LayoutType.Path => "Path",
            _ => this.name
        };

        _length = length;
        _width = width;
        if ((layoutType == RoomUtility.LayoutType.Path) && isVertical){
            _length = width;
            _width = length;
        }

    }
    void Start()
    {

        _center = transform.position;
        Vector2 pos;
        
        // offset for even sized room
        float halfLength = (_length - 1.0f) / 2;
        float halfWidth = (_width - 1.0f) / 2;
        
        for (int w = 0 ; w < _width ; w++){
            for (int l = 0 ; l < _length ; l++){
                pos = new Vector2(l - halfLength, halfWidth - w);
                Instantiate(roomIcon, _center + pos, Quaternion.identity, transform);
            }
        }

    }

}
