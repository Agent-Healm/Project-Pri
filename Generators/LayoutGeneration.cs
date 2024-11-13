using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayoutGeneration : MonoBehaviour
{
    private int _length;
    private int _width;
    private Vector2 _center;
    private RoomGeneration _roomGen;

    // Start is called before the first frame update
    void Start()
    {

        Vector2Int pos;

        _roomGen = this.GetComponent<RoomGeneration>();
        _length = _roomGen._length;
        _width = _roomGen._width;
        _center = transform.position;

        // offset for even sized room
        int halfLength = (_length - 1) / 2;
        int halfWidth = (_width - 1) / 2;

        for (int w = 0 ; w < _width ; w++){
            for (int l = 0 ; l < _length ; l++){
                if((w + l) % 2 ==1){
                    pos = new Vector2Int(l - halfLength, halfWidth - w);
                    Instantiate(TextureTheme.instance.wallTile, _center + pos, 
                                    Quaternion.identity, transform);
                }
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
