using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfig : MonoBehaviour
{
    public Room path;
    public Room[] rooms;
    public int targetFrameRate = 30;
    public static RoomConfig instance;
    // Start is called before the first frame update
    void Awake(){
        // set framerate limit
        Application.targetFrameRate = targetFrameRate;
        
        if(instance == null){
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
}
