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
    private Vector3[] _emptySpace = new Vector3[0];
    
    private Vector3 _newPosition;

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

        RoomUtility.getAdjacentVec3(ref _emptySpace, transform.position, _roomPos, tileSize);
        _newPosition = _emptySpace[Random.Range(0, _emptySpace.Length)];
        ArrayUtility.Clear(ref _emptySpace);

        GenerateSideRoom("home", _newPosition * -1);

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

        RoomUtility.getAdjacentVec3(ref _emptySpace, transform.position, _roomPos, tileSize);

        RoomUtility.Vec3Shuffle(ref _emptySpace);

        if (maxRooms > 1){
            GenerateMainRoom("mob");
        }
        else if (maxRooms == 1){
            ArrayUtility.Clear(ref _emptySpace);
            GenerateMainRoom("exit");
            return;
        }
        
        _newPosition = _emptySpace[0];
        ArrayUtility.Remove(ref _emptySpace, _newPosition);

        RoomUtility.Vec3RandomErase(ref _emptySpace);

        if (_extraRooms.Length != 0){

            string room;
            foreach(Vector3 vec3 in _emptySpace){

                room = _extraRooms[Random.Range(0, _extraRooms.Length)];
                ArrayUtility.Remove(ref _extraRooms, room);
                GenerateSideRoom(room, 
                                vec3);

                if (_extraRooms.Length == 0){break;}
            }
        }

        transform.position += _newPosition * tileSize;

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

    private void GenerateMainRoom(string roomName){
        GameObject room = FindRoom(roomName);

        WallGeneration wallRoom = room.GetComponent<WallGeneration>();
        wallRoom.GateReset();

        foreach(Vector2 vec2 in _emptySpace){wallRoom.setGate(vec2, true);}
        wallRoom.setGate(_newPosition * -1, true);

        Instantiate(
            room,
            transform.position,
            Quaternion.identity
        );
        if (_roomPos.Length >=1){
            GameObject gate = FindRoom("path");
            RoomGeneration roomGen = gate.GetComponent<RoomGeneration>();
            roomGen.isVertical = false;
            if(_newPosition.y == 0.0f){roomGen.isVertical = true;}

            WallGeneration wallGate = gate.GetComponent<WallGeneration>();
            wallGate.GateReset();
            wallGate.setGate(_newPosition, true);
            wallGate.setGate(_newPosition * -1, true);

            Instantiate(
                gate,
                transform.position - _newPosition * tileSize * 0.5f,
                Quaternion.identity
            );
        }
        
        ArrayUtility.Add(ref _roomPos, transform.position);
    }

    private void GenerateSideRoom(string roomName, Vector3 directionPos){
        GameObject room = FindRoom(roomName);

        WallGeneration wallRoom = room.GetComponent<WallGeneration>();
        wallRoom.GateReset();
        wallRoom.setGate(directionPos * -1, true);

        Instantiate(
            room,
            transform.position + directionPos * tileSize,
            Quaternion.identity
        );

        if(_roomPos.Length >=1){
            GameObject gate = FindRoom("path");
            
            RoomGeneration roomGate = gate.GetComponent<RoomGeneration>();
            roomGate.isVertical = false;
            if(directionPos.y == 0.0f){roomGate.isVertical = true;}

            WallGeneration wallGate = gate.GetComponent<WallGeneration>();
            wallGate.GateReset();
            wallGate.setGate(directionPos, true);
            wallGate.setGate(directionPos * -1, true);

            Instantiate(
                gate,
                transform.position + directionPos * tileSize * 0.5f,
                Quaternion.identity
            );
        }
        ArrayUtility.Add(ref _roomPos, transform.position + directionPos * tileSize);
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
            if ((ArrayUtility.FindIndex(_roomPos, x => x == (Vector2)(currentPos + vec3 * tileSize))) == -1 
                ){
                ArrayUtility.Add(ref emptySpace, vec3);
            }
        }
    }
    public static void Vec3RandomErase(ref Vector3[] arr){
        Vector3[] temp = arr;
        foreach(Vector3 vec3 in temp){
            if (Random.Range(0, 2) == 0){
                ArrayUtility.Remove(ref arr, vec3);
            }
            if(arr.Length == 1){break;}
        }
    }

}
