using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomSize : MonoBehaviour
{
    [SerializeField] private int width = 5;
    public int Width { get => width; }
    [SerializeField] private int height = 5;
    public int Height { get => height; }
    public BoundsInt RoomBounds
    {
        get
        {
            return new(
                new(-width / 2, - height / 2, 0),
                new(width, height, 1)
            );
        }
    }
    public Tilemap[] RoomLayout
    {
        get
        {
            return GetComponentsInChildren<Tilemap>();
        }
    }
}
