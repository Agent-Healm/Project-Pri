using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Room
{
    public string name;
    [Header("Number of rooms able to spawn (inclusive)")]
    public int min;
    public int max;
    public bool isIncluded; 
    public GameObject[] roomObjects;
}
