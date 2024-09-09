using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    public Transform[] startPos;
    public Room[] rooms;
    public int maxRooms = 3;
    public float tileSize = 1.0f;

    private float _timeRoomInterval = 0.0f;
    private string[] _extraRooms = new string[0];
    private string[] _extraRoomsMax = new string[0];
    private Vector2[] _roomPos = new Vector2[0];
    // private Vector3[] _adjacentDirection = new Vector3[] 
    // {
    // Vector3.up, 
    // Vector3.right, 
    // Vector3.down, 
    // Vector3.left,
    // };

    private Vector2[] _worldPoints = new Vector2[0];
    private Vector2 _worldPoint = Vector2.zero;
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
        RenderRoom("home", 
                Vector3.zero);
    }

    void Update(){
        if(maxRooms > 0){
            if(_timeRoomInterval <= 0 ){
                procGenRoom();
                _timeRoomInterval = 0.1f;
                maxRooms -= 1;
            }
            else {
                _timeRoomInterval -= Time.deltaTime;
            }
        }
    }

    private void procGenRoom(){
        
        _emptySpace = new Vector3[0];
        Vector3 _newPosition;

        RoomUtility.getAdjacentVec3(ref _emptySpace, transform.position, _roomPos, tileSize);

        RoomUtility.Vec3Shuffle(ref _emptySpace);

        _newPosition = _emptySpace[0];
        ArrayUtility.Remove(ref _emptySpace, _newPosition);

        if (maxRooms > 1){
            RenderRoom("mob",
                    _newPosition);
        }
        else if (maxRooms == 1){
            RenderRoom("exit",
                    _newPosition);

            // foreach(Vector2 vec2 in _roomPos){
            //     Debug.Log("Room position at : " + vec2);
            // }

            // foreach(Vector2 vec2 in _worldPoints){
            //     Debug.Log("Room position at : " + vec2);
            // }

            return;
        }
        
        if (_roomPos.Length > 2 && _extraRooms.Length != 0){

            string room;
            foreach(Vector3 vec3 in _emptySpace){

                if (_extraRooms.Length == 0){break;}

                if (Random.Range(0, 2) == 0){
                    room = _extraRooms[Random.Range(0, _extraRooms.Length)];
                    ArrayUtility.Remove(ref _extraRooms, room);
                    RenderRoom(room,
                            vec3);
                }
            }
        }

        transform.position += _newPosition * tileSize;

        // _worldPoint += (Vector2)_newPosition;

    }

    private GameObject FindRoom(string roomName){
        /// <summary>
        /// Find room based on the names defined in the unity
        /// </summary>
        /// <param name="roomName"></param>
        /// <return>A room GameObject</returns>
        Room room = Array.Find(rooms, x => x.name == roomName);
        if (room == null){
            Debug.Log("The room "+ roomName +" not found");
            return FindRoom("null"); 
        }
        else {
            return room.roomObject;
        }
    }

    private void RenderRoom(string roomName, Vector3 directionPos){
        Instantiate(
            FindRoom(roomName),
            transform.position + directionPos * tileSize,
            Quaternion.identity
        );

        if(roomName != "path"){
            ArrayUtility.Add(ref _roomPos, transform.position + directionPos * tileSize);
            // ArrayUtility.Add(ref _worldPoints, _worldPoint + (Vector2)directionPos);
        }
        if (_roomPos.Length >=2){
            Instantiate(
                FindRoom("path"),
                transform.position + directionPos * tileSize * 0.5f,
                Quaternion.FromToRotation(Vector3.right, directionPos)
            );
        }

    }
}

public static class RoomUtility {
    public static IEnumerable<Vector3> adjacentDirection(){
        yield return Vector3.up;
        yield return Vector3.right;
        yield return Vector3.down;
        yield return Vector3.left;
    }
    public static void Vec3Shuffle(ref Vector3[] arr){
        /// <summary>
        /// Shuffles an array of Vector3
        /// </summary>
        int rand;
        Vector3 temp;

        for (int i = 0 ; i < arr.Length ; i++){
            rand = Random.Range(0, arr.Length);
            temp = arr[rand];
            arr[rand] = arr[i];
            arr[i] = temp;
        }
    }
    public static void getAdjacentVec3(ref Vector3[] emptySpace, Vector3 currentPos, Vector2[] _roomPos, float tileSize){
        
        /// <summary>
        /// scanning empty spaces adjacent to the current tile position
        /// </summary>
        // ArrayUtility.contains method is still bugged
        foreach (Vector3 vec3 in adjacentDirection()){
            // if ((ArrayUtility.FindIndex(_roomPos, x => x == (Vector2)(currentPos + vec3 * tileSize))) == -1 
            if ((ArrayUtility.FindIndex(_roomPos, x => x == (Vector2)(currentPos + vec3 * tileSize))) == -1 
                ){
                ArrayUtility.Add(ref emptySpace, vec3);
            }
        }
    }

}