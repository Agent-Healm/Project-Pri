using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomConfig : MonoBehaviour
{
    public Room path;
    public Room[] basicRooms;
    public Room[] rooms;
    public int targetFrameRate = 30;
    public static RoomConfig Instance;
    // Start is called before the first frame update
    void Awake(){
        // set framerate limit
        Application.targetFrameRate = targetFrameRate;
        
        if(Instance == null){
            Instance = this;
        }
        DontDestroyOnLoad(this);
    }
}
