using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RoomConfig : MonoBehaviour
{
    [SerializeField] private Room path;
    public Room GetPath { get => path; }
    [SerializeField] private Room[] basicRooms;
    public Room[] GetBasicRooms { get => basicRooms; }
    [SerializeField] private Room[] rooms;
    public Room[] GetRooms { get => rooms; }
    [SerializeField] private int targetFrameRate = 30;
    [SerializeField] private static RoomConfig Instance;

    public static RoomConfig GetInstance {get => Instance;}
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
