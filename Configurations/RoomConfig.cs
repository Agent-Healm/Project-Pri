using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfig : MonoBehaviour
{
    public Room[] rooms;
    public static RoomConfig instance;
    // Start is called before the first frame update
    void Awake(){
        // set framerate limit
        Application.targetFrameRate = 30;
        
        if(instance == null){
            instance = this;
        }
        DontDestroyOnLoad(this);
    }
}
