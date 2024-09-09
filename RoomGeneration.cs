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
    // Start is called before the first frame update
    void Start()
    {

        _center = transform.position;
        Vector2Int pos;
        int halfLength = (length - 1) / 2;
        int halfWidth = (width - 1) / 2;

        for (int w = 0 ; w < width ; w++){
            for (int l = 0 ; l < length ; l++){
                pos = new Vector2Int(l - halfLength, halfWidth - w);
                Instantiate(floorTile, _center + pos, Quaternion.identity, transform);
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
