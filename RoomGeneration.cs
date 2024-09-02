using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using Random = UnityEngine.Random;

// namespace ArrayUtility;
public class RoomGeneration : MonoBehaviour
{
    public Transform[] startPos;
    public Room[] rooms;
    public int maxRooms = 3;
    public float tileSize = 1.0f;

    private float _timeRoomInterval = 0.0f;
    private string[] _extraRooms = new string[0];
    private string[] _extraRoomsMax = new string[0];
    private Vector2[] _roomPos = new Vector2[0];
    private Vector3[] _cardinalDirection = new Vector3[] 
    {
    Vector3.up, 
    Vector3.right, 
    Vector3.down, 
    Vector3.left,
    };
    private Vector3[] _emptySpace;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Room room in rooms){
            if(room.isIncluded){
                for (int i = 1 ; i <= room.min ; i++){
                    ArrayUtility.Add(ref _extraRooms, room.name);
                }
                // for (int i = 1 ; i <= room.max - room.min ; i++){
                //     ArrayUtility.Add(ref _extraRoomsMax, room.name);
                //     Debug.Log("The room  " + room.name + "  has (i) more rooms : " + i);
                // }
            }
        }

        if (maxRooms <=1){maxRooms = 2;}

        transform.position = startPos[0].position;
        Instantiate(FindRoom("home"), 
                    transform.position, 
                    Quaternion.identity);

        ArrayUtility.Add(ref _roomPos, transform.position);

    }

    void Update(){
        if(maxRooms > 0){
            if(_timeRoomInterval <= 0 ){
                // getNextRoom();
                procGenRoom();
                _timeRoomInterval = 0.1f;
                maxRooms -= 1;
            }
            else {
                _timeRoomInterval -= Time.deltaTime;
            }
        }
        // else if (maxRooms == 0){generateExtraRooms();
        // maxRooms -=1;}
    }

    private void getNextRoom(){
        _emptySpace = new Vector3[0];

        _emptySpace = getCardinalVec3(_emptySpace, transform.position);

        transform.position = _emptySpace[Random.Range(0, _emptySpace.Length)];

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
        ArrayUtility.Add(ref _roomPos, transform.position);

    }

    private void generateExtraRooms(){
        _emptySpace = new Vector3[0];

        int i = 1;

        for (i = 1 ; 
            i < _roomPos.Length - 1 ; 
            i++){
            
            _emptySpace = getCardinalVec3(_emptySpace, _roomPos[i]);
        }

        foreach(Vector3 vec3 in _emptySpace){

            string room;
            if (Random.Range(0, 2) == 0){

                room = _extraRooms[Random.Range(0, _extraRooms.Length)];
                ArrayUtility.Remove(ref _extraRooms, room);
                Instantiate(
                    FindRoom(room),
                    vec3,
                    Quaternion.identity
                );
            }
            if (_extraRooms.Length == 0){return;}
        }
    }
    
    /// <summary>
    /// Find room based on the names defined in the unity
    /// </summary>
    /// <param name="roomName"></param>
    /// <return>A room GameObject</returns>
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
        foreach (Vector3 vec3 in _cardinalDirection){
            if ((ArrayUtility.FindIndex(_roomPos, x => x == (Vector2)(currentPos + vec3 * tileSize))) == -1 &&
                (ArrayUtility.FindIndex(emptySpace, x => (Vector2)x == (Vector2)(currentPos + vec3 * tileSize))) == -1){
                ArrayUtility.Add(ref emptySpace, currentPos + vec3 * tileSize);
            }
        }

        return emptySpace;

    }

    private void procGenRoom(){
        // WORK IN PROGRESS
        _emptySpace = new Vector3[0];

        _emptySpace = getCardinalVec3(_emptySpace, transform.position);

        transform.position = _emptySpace[Random.Range(0, _emptySpace.Length)];
        ArrayUtility.Remove(ref _emptySpace, transform.position);

        if (maxRooms > 1){
            Instantiate(FindRoom("mob"), 
                transform.position, 
                Quaternion.identity);
        }
        else if (maxRooms == 1){
            Instantiate(FindRoom("exit"), 
                transform.position, 
                Quaternion.identity);
            return;
        }
        ArrayUtility.Add(ref _roomPos, transform.position);
        Debug.Log("generated a room at " + transform.position);
        
        if (_roomPos.Length > 2 && _extraRooms.Length != 0){
            
            string room;
            foreach(Vector3 vec3 in _emptySpace){

                if (_extraRooms.Length == 0){break;}

                if (Random.Range(0, 2) == 0){
                    room = _extraRooms[Random.Range(0, _extraRooms.Length)];
                    Debug.Log("generated a special room " + room + " at " + vec3);
                    ArrayUtility.Remove(ref _extraRooms, room);
                    Instantiate(
                        FindRoom(room),
                        vec3,
                        Quaternion.identity
                    );
                    ArrayUtility.Add(ref _roomPos, vec3);
                }
            }
        }
    }

}