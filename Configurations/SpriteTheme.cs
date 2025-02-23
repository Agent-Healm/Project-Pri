using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTheme : MonoBehaviour
{
    public Sprite floorTile;
    public Sprite gateTile;
    public Sprite torchTile;
    public Sprite wallTile;
    public Sprite nullTile;
    public static SpriteTheme Instance;

    void Awake(){
        if (Instance == null){
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
}
