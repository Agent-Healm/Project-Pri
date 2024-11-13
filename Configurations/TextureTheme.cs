using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureTheme : MonoBehaviour
{
    public GameObject floorTile;
    public GameObject gateTile;
    public GameObject torchTile;
    public GameObject wallTile;
    public GameObject nullTile;
    
    public static TextureTheme instance;

    void Awake(){
        if (instance == null){
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
}
