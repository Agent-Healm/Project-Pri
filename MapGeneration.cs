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

    private float _tempRoomLength; 
    private float _tempRoomWidth;
    private float _timeRoomInterval = 0.0f;
    private string[] _extraRooms = new string[0];
    private string[] _extraRoomsMax = new string[0];
    private Vector2[] _roomPos = new Vector2[0];
    private Vector2 _currentWorldPoint = Vector2.zero;
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
                for (int i = 1 ; i <= room.max - room.min ; i++){
                    ArrayUtility.Add(ref _extraRoomsMax, room.name);
                }
            }
        }

        if (maxRooms <=1){maxRooms = 2;}

        transform.position = startPos[0].position;

        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos, 1.0f);
        _newPosition = _emptySpace[Random.Range(0, _emptySpace.Length)];
        ArrayUtility.Clear(ref _emptySpace);

        GenerateAllRooms(2, "home", _newPosition * -1);
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

        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos, 1.0f);

        RoomUtility.Vec3Shuffle(ref _emptySpace);
        RoomUtility.Vec3Reduce(ref _emptySpace, (_extraRooms.Length != 0));

        if (maxRooms > 1){
            GenerateAllRooms(1, "mob", _newPosition);
        }
        else if (maxRooms == 1){
            ArrayUtility.Clear(ref _emptySpace);
            GenerateAllRooms(1, "exit", _newPosition);

            RoomDebug.ShowRoomWorldPositions(_roomPos, true);
            return;
        }
        
        _newPosition = _emptySpace[0];
        ArrayUtility.Remove(ref _emptySpace, _newPosition);

        if (_extraRooms.Length != 0){

            string room;
            foreach(Vector3 vec3 in _emptySpace){

                room = _extraRooms[Random.Range(0, _extraRooms.Length)];
                ArrayUtility.Remove(ref _extraRooms, room);

                GenerateAllRooms(2, room, vec3);

                if (_extraRooms.Length == 0){
                    if(_extraRoomsMax.Length != 0){
                        ArrayUtility.AddRange(ref _extraRooms, _extraRoomsMax);
                        ArrayUtility.Clear(ref _extraRoomsMax);
                    }
                    else {break;}
                }
            }
        }

        transform.position += _newPosition * tileSize;
        _currentWorldPoint += (Vector2)_newPosition;
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
            return room.roomObjects [Random.Range(0, room.roomObjects.Length)];
        }
    }

    private void GenerateAllRooms(int type, string roomName, Vector3 facingPos){
        
        GameObject room = FindRoom(roomName);
        float evenOffsetLength = 0.5f;
        float evenOffsetWidth = 0.5f;

        RoomGeneration roomGen = room.GetComponent<RoomGeneration>();
        WallGeneration wallGen = room.GetComponent<WallGeneration>();
        wallGen.GateReset();
        wallGen.setGate(facingPos * -1, true);

        Vector3 spawnPoint = transform.position;

        if (type == 2){spawnPoint += (facingPos * tileSize);}

        if(_roomPos.Length > 0){
            if((facingPos.x == 0.0f) && ((roomGen.width + _tempRoomWidth) % 2 != 0)){
                if (type == 1){
                    transform.position -= facingPos * evenOffsetWidth;
                    spawnPoint = transform.position;
                }
                else if (type == 2){
                    spawnPoint -= facingPos * evenOffsetWidth;
                }
            }
            else if((facingPos.y == 0.0f) && ((roomGen.length + _tempRoomLength) % 2 != 0)){
                if (type == 1){
                    transform.position -= facingPos * evenOffsetLength;
                    spawnPoint = transform.position;
                }
                else if(type == 2){
                    spawnPoint -= facingPos * evenOffsetLength;
                }
            }   
        }
        // spawnPoint = transform.position;

        if (type == 1){
            foreach(Vector2 vec2 in _emptySpace){wallGen.setGate(vec2, true);}
            ArrayUtility.Add(ref _roomPos, _currentWorldPoint);
        }
        else if (type == 2){
            // spawnPoint += (facingPos * tileSize);
            // spawnPoint += (facingPos * (int)(tileSize - (roomGen.length + _tempRoomLength)%2));
            ArrayUtility.Add(ref _roomPos, _currentWorldPoint + (Vector2)facingPos);
        }

        Instantiate(
            room, spawnPoint,
            Quaternion.identity
        );

        if (_roomPos.Length == 1){
            _tempRoomLength = roomGen.length;
            _tempRoomWidth  = roomGen.width;
        }

        else if(_roomPos.Length >=2){
            GameObject gate = FindRoom("path");
            RoomGeneration roomGate = gate.GetComponent<RoomGeneration>();

            if(facingPos.x == 0.0f){
                roomGate.isVertical = true;
                roomGate.length = (int)(tileSize - (_tempRoomWidth + roomGen.width) / 2) - 2;
            }
            else{
                roomGate.isVertical = false;
                roomGate.length = (int)(tileSize - (_tempRoomLength + roomGen.length) / 2) - 2;
            }
            
            WallGeneration wallGate = gate.GetComponent<WallGeneration>();
            wallGate.GateReset();
            wallGate.setGate(facingPos, true);
            wallGate.setGate(facingPos * -1, true);

            if (type == 1){
                _tempRoomLength = roomGen.length;
                _tempRoomWidth  = roomGen.width;

                if(facingPos.x == 0.0f){
                    spawnPoint = transform.position - facingPos * ((roomGate.length + roomGen.width) * 0.5f + 1.0f);
                }
                else {
                    spawnPoint = transform.position - facingPos * ((roomGate.length + roomGen.length) * 0.5f + 1.0f);
                }
            }
            else if (type == 2){
                if (facingPos.x == 0.0f){
                    spawnPoint = transform.position + facingPos * ((roomGate.length + _tempRoomWidth) * 0.5f + 1.0f);
                }
                else {
                    spawnPoint = transform.position + facingPos * ((roomGate.length + _tempRoomLength) * 0.5f + 1.0f);
                }
            }
            Instantiate(
                gate, spawnPoint,
                Quaternion.identity
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
            if ((ArrayUtility.FindIndex(_roomPos, x => x == (Vector2)(currentPos + vec3 * tileSize))) == -1 
                ){
                ArrayUtility.Add(ref emptySpace, vec3);
            }
        }
    }
    public static void Vec3Reduce(ref Vector3[] arr, bool needExtra = true){
        if (!needExtra){
            arr = new Vector3[]{arr[0]};
            // Debug.Log("returned single vec3");
            return;
        }

        Vector3[] temp = arr;
        foreach(Vector3 vec3 in temp){
            if(arr.Length == 1){return;}
            else if (Random.Range(0, 3) == 0){
                ArrayUtility.Remove(ref arr, vec3);
            }
        }
    }

}

public static class RoomDebug {
    public static void ShowRoomWorldPositions(Vector2[] roomWorldCoordinates, bool isSimplified = false){
        Vector2[] tempArray = new Vector2[0];
        foreach(Vector2 vec2 in roomWorldCoordinates){
            if (!isSimplified){
                Debug.Log("Vector 2 world position at : " + vec2);
            }
            if (ArrayUtility.FindIndex(tempArray, x => x == vec2) == -1){
                ArrayUtility.Add(ref tempArray, vec2);
            }
            else {
                Debug.Log("Rooms overlapping at " + vec2);
                ArrayUtility.Clear(ref tempArray);
                return;
            }
        }
        Debug.Log("Room Generated Successfully");
        if(!isSimplified){Debug.Log("No overlapping happened");}
    }
}