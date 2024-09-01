using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using Random = UnityEngine.Random;

public class RoomGeneration : MonoBehaviour
{
    public Transform[] startPos;
    public Room[] rooms;
    public int maxRooms = 3;
    public float tileSize = 1.0f;
    private float timeRoomInterval = 0.0f;

    private string[] extraRooms = new string[0];
    private string[] extraRoomsMax = new string[0];
    private Vector2[] roomPos = new Vector2[0];
    private Vector3[] cardinalDirection = new Vector3[] 
    {
    Vector3.up, 
    Vector3.right, 
    Vector3.down, 
    Vector3.left,
    };
    private Vector3[] emptySpace;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Room room in rooms){
            if(room.isIncluded){
                for (int i = 1 ; i <= room.min ; i++){
                    ArrayUtility.Add(ref extraRooms, room.name);
                }
                // for (int i = 1 ; i <= room.max - room.min ; i++){
                //     ArrayUtility.Add(ref extraRoomsMax, room.name);
                //     Debug.Log("The room  " + room.name + "  has (i) more rooms : " + i);
                // }
            }
        }

        if (maxRooms <=1){maxRooms = 2;}

        transform.position = startPos[0].position;
        Instantiate(FindRoom("home"), 
                    transform.position, 
                    Quaternion.identity);

        ArrayUtility.Add(ref roomPos, transform.position);

        // int a = 1;
        // foreach(Room room in rooms){
        //     Debug.Log("------------------");
        //     Debug.Log("Object " + a++);
        //     Debug.Log("Name : " + room.name);
        //     Debug.Log("Range : " + room.min + " - " + room.max);
        //     Debug.Log("------------------");
        // }

        // Debug.Log("initial position : " + (Vector2)transform.position);
        // Debug.Log("initial position : " + transform.position);
        // Debug.Log("home position" + roomPos[0]);

        // Debug.Log("dictionary : " + dict.Count);

        // foreach(KeyValuePair<string, int> kv in dict){
        //     Debug.Log("Key : " + kv.Key + " ,Value : " + kv.Value);
        // }

    }

    void Update(){
        if(maxRooms > 0){
            if(timeRoomInterval <= 0 ){
                // getNextRoom();
                procGenRoom();
                timeRoomInterval = 0.1f;
                maxRooms -= 1;
            }
            else {
                timeRoomInterval -= Time.deltaTime;
            }
        }
        // else if (maxRooms == 0){generateExtraRooms();
        // maxRooms -=1;}
    }

    private void getNextRoom(){
        emptySpace = new Vector3[0];

        // foreach (Vector3 vec3 in cardinalDirection){
        //     // if(roomPos.Length == 2){
        //     //     Debug.Log(roomPos[0] + " != " + (Vector2)(transform.position + vec3 * tileSize));
        //     //     Debug.Log(roomPos[0] != (Vector2)(transform.position + vec3 * tileSize));
        //     //     // Debug.Log("Therefore, contain should return the value : " + ArrayUtility.Contains(roomPos, transform.position) + " for " + transform.position);
        //     // }

        //     // if(roomPos.Length == 2){
        //     //     Debug.Log((Vector2)(transform.position + vec3 * tileSize) + " will be added if my value is true : " + !ArrayUtility.Contains(roomPos, (Vector2)(transform.position + vec3 * tileSize)));
        //     //     Debug.Log(roomPos[0] != (Vector2)(transform.position + vec3 * tileSize));
        //     //     Debug.Log(ArrayUtility.FindIndex(roomPos, x => x ==(Vector2)(transform.position + vec3 * tileSize)));
        //     // }

        //     // contains method is bugged :/
        //     // if (!ArrayUtility.Contains(
        //     // roomPos, 
        //     // (Vector2)(transform.position + vec3 * tileSize))){
        //     //     ArrayUtility.Add(ref emptySpace, transform.position + vec3 * tileSize);
        //     // }

        //     if (ArrayUtility.FindIndex(roomPos, x => x == (Vector2)(transform.position + vec3 * tileSize)) == -1){
        //         ArrayUtility.Add(ref emptySpace, transform.position + vec3 * tileSize);
        //     }
        // }

        emptySpace = getCardinalVec3(emptySpace, transform.position);

        transform.position = emptySpace[Random.Range(0, emptySpace.Length)];

        if (maxRooms > 1){
            Instantiate(FindRoom("mob"), 
                transform.position, 
                Quaternion.identity);
        }
        else if (maxRooms == 1){
            Instantiate(FindRoom("exit"), 
                transform.position, 
                Quaternion.identity);

        }
        ArrayUtility.Add(ref roomPos, transform.position);

    }

    private void generateExtraRooms(){
        emptySpace = new Vector3[0];

        int i = 1;
        // foreach (Vector3 vec3 in roomPos){
        //     Debug.Log("room " + i++ + " is in " + vec3);
        // }

        for (i = 1 ; 
            i < roomPos.Length - 1 ; 
            i++){
            
            // foreach(Vector2 vec2 in cardinalDirection){
            //     // if (!ArrayUtility.Contains(roomPos, roomPos[i] + vec2 * tileSize) && 
            //     //     !ArrayUtility.Contains(emptySpace, roomPos[i] + vec2 * tileSize)){
            //     //     ArrayUtility.Add(ref emptySpace, roomPos[i] + vec2 * tileSize);
            //     // }
            //     if (ArrayUtility.FindIndex(roomPos, x => x == (roomPos[i] + vec2 * tileSize)) == -1 && 
            //         ArrayUtility.FindIndex(emptySpace, x => (Vector2)x == (roomPos[i] + vec2 * tileSize)) == -1){
            //         ArrayUtility.Add(ref emptySpace, roomPos[i] + vec2 * tileSize);
            //     }
            // }
            emptySpace = getCardinalVec3(emptySpace, roomPos[i]);
        }

        // Debug.Log("----------------------");
        // i = 1;
        foreach(Vector3 vec3 in emptySpace){
            // Debug.Log("empty space " + i++ + " is in " + vec3);
            // Instantiate(FindRoom("special"), vec3, Quaternion.identity);

            // rooms[Random.Range(0, rooms.Length)]
            // }
    
            string room;
            if (Random.Range(0, 2) == 0){

                room = extraRooms[Random.Range(0, extraRooms.Length)];
                ArrayUtility.Remove(ref extraRooms, room);
                Instantiate(
                    FindRoom(room),
                    vec3,
                    Quaternion.identity
                );
            }
            if (extraRooms.Length == 0){return;}
        }
    }
    
    // <summary>
    // Find room based on the names defined in the unity
    // </summary>
    // <param name="roomName"></param>
    // <return>A room GameObject</returns>
    private GameObject FindRoom(string roomName){
        Room room = Array.Find(rooms, x => x.name == roomName);
        if (room == null){
            Debug.Log("The room "+ roomName +" not found");
            return FindRoom("null"); 
        }
        else {
            return room.roomObject;
        }
    }

    private Vector3[] getCardinalVec3(Vector3[] emptySpace, Vector3 currentPos){
        
        // ArrayUtility.contains method is still bugged
        foreach (Vector3 vec3 in cardinalDirection){
            if ((ArrayUtility.FindIndex(roomPos, x => x == (Vector2)(currentPos + vec3 * tileSize))) == -1 &&
                (ArrayUtility.FindIndex(emptySpace, x => (Vector2)x == (Vector2)(currentPos + vec3 * tileSize))) == -1){
                ArrayUtility.Add(ref emptySpace, currentPos + vec3 * tileSize);
            }
        }

        return emptySpace;

    }

    private void procGenRoom(){
        // WORK IN PROGRESS
        emptySpace = new Vector3[0];

        emptySpace = getCardinalVec3(emptySpace, transform.position);

        transform.position = emptySpace[Random.Range(0, emptySpace.Length)];

        if (maxRooms > 1){
            Instantiate(FindRoom("mob"), 
                transform.position, 
                Quaternion.identity);
        }
        else if (maxRooms == 1){
            Instantiate(FindRoom("exit"), 
                transform.position, 
                Quaternion.identity);

        }
        ArrayUtility.Add(ref roomPos, transform.position);
    }

}