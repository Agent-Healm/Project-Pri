using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class RoomConfig : MonoBehaviour
{
    [SerializeField] private RoomObsolete path;
    public RoomObsolete GetPath { get => path; }
    [SerializeField] private RoomObsolete[] basicRooms;
    public RoomObsolete[] GetBasicRooms { get => basicRooms; }
    [SerializeField] private RoomObsolete[] rooms;
    public RoomObsolete[] GetRooms { get => rooms; }
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
