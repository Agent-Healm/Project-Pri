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

        // RoomUtility.getAdjacentVec3(ref _emptySpace, transform.position, _roomPos, tileSize);
        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos, 1.0f);
        _newPosition = _emptySpace[Random.Range(0, _emptySpace.Length)];
        ArrayUtility.Clear(ref _emptySpace);


        // GenerateSideRoom("home", _newPosition * -1);
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

        // RoomUtility.getAdjacentVec3(ref _emptySpace, transform.position, _roomPos, tileSize);
        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos, 1.0f);

        RoomUtility.Vec3Shuffle(ref _emptySpace);
        RoomUtility.Vec3Reduce(ref _emptySpace, (_extraRooms.Length != 0));

        if (maxRooms > 1){
            // GenerateMainRoom("mob");
            GenerateAllRooms(1, "mob", _newPosition);
        }
        else if (maxRooms == 1){
            ArrayUtility.Clear(ref _emptySpace);
            // GenerateMainRoom("exit");
            GenerateAllRooms(1, "exit", _newPosition);
            foreach(Vector2 vec2 in _roomPos){
                Debug.Log("Vector 2 world position at : " + vec2);
            }
            return;
        }
        
        _newPosition = _emptySpace[0];
        ArrayUtility.Remove(ref _emptySpace, _newPosition);

        if (_extraRooms.Length != 0){

            string room;
            foreach(Vector3 vec3 in _emptySpace){

                room = _extraRooms[Random.Range(0, _extraRooms.Length)];
                ArrayUtility.Remove(ref _extraRooms, room);

                // GenerateSideRoom(room, 
                //                 vec3);
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

    private void GenerateMainRoom(string roomName){
        
        GameObject room = FindRoom(roomName);

        RoomGeneration genRoom = room.GetComponent<RoomGeneration>();
        WallGeneration wallRoom = room.GetComponent<WallGeneration>();
        
        wallRoom.GateReset();
        wallRoom.setGate(_newPosition * -1, true);
        
        foreach(Vector2 vec2 in _emptySpace){wallRoom.setGate(vec2, true);}

        Instantiate(
            room,
            transform.position,
            Quaternion.identity
        );
        
        if (_roomPos.Length >=1){
            GameObject gate = FindRoom("path");
            RoomGeneration roomGate = gate.GetComponent<RoomGeneration>();
            if(_newPosition.x == 0.0f){
                roomGate.isVertical = true;
                roomGate.length = (int)(tileSize - (_tempRoomWidth + genRoom.width) / 2) - 2;
            }
            else{
                roomGate.isVertical = false;
                roomGate.length = (int)(tileSize - (_tempRoomLength + genRoom.length) / 2) - 2;
            }

            WallGeneration wallGate = gate.GetComponent<WallGeneration>();
            wallGate.GateReset();
            wallGate.setGate(_newPosition, true);
            wallGate.setGate(_newPosition * -1, true);

            Instantiate(
                gate,
                transform.position - _newPosition * ((genRoom.length + roomGate.length) * 0.5f + 1.0f),
                Quaternion.identity
            );
        }
        
        _tempRoomLength = genRoom.length;
        _tempRoomWidth  = genRoom.width;

        ArrayUtility.Add(ref _roomPos, transform.position);
    }

    private void GenerateSideRoom(string roomName, Vector3 directionPos){
        GameObject room = FindRoom(roomName);
        RoomGeneration genRoom = room.GetComponent<RoomGeneration>();
        WallGeneration wallRoom = room.GetComponent<WallGeneration>();

        wallRoom.GateReset();
        wallRoom.setGate(directionPos * -1, true);

        Instantiate(
            room,
            transform.position + directionPos * tileSize,
            Quaternion.identity
        );

        if (_roomPos.Length == 0){
            _tempRoomLength = genRoom.length;
            _tempRoomWidth  = genRoom.width;
        }

        else if(_roomPos.Length >=1){
            GameObject gate = FindRoom("path");
            
            RoomGeneration roomGate = gate.GetComponent<RoomGeneration>();
            if(directionPos.x == 0.0f){
                roomGate.isVertical = true;
                roomGate.length = (int)(tileSize - (_tempRoomWidth + genRoom.width) / 2) - 2;
            }
            else {
                roomGate.isVertical = false;
                roomGate.length = (int)(tileSize - (_tempRoomLength + genRoom.length) / 2) - 2;
            }

            WallGeneration wallGate = gate.GetComponent<WallGeneration>();
            wallGate.GateReset();
            wallGate.setGate(directionPos, true);
            wallGate.setGate(directionPos * -1, true);

            Instantiate(
                gate,
                transform.position + directionPos * ((roomGate.length + _tempRoomLength) * 0.5f + 1.0f),
                Quaternion.identity
            );
        }
        ArrayUtility.Add(ref _roomPos, transform.position + directionPos * tileSize);
    }
    
    private void GenerateAllRooms(int type, string roomName, Vector3 facingPos){
        
        Vector3 spawnPoint = transform.position;
        GameObject room = FindRoom(roomName);
        RoomGeneration roomGen = room.GetComponent<RoomGeneration>();
        WallGeneration wallGen = room.GetComponent<WallGeneration>();
        wallGen.GateReset();
        wallGen.setGate(facingPos * -1, true);
        
        switch(type){
            case 1:{
                foreach(Vector2 vec2 in _emptySpace){wallGen.setGate(vec2, true);}
                break;
            }
            case 2:{
                spawnPoint += facingPos * tileSize;
                break;
            }
        }

        Instantiate(
            room,
            spawnPoint,
            Quaternion.identity
        );

        // ArrayUtility.Add(ref _roomPos, spawnPoint);
        // ArrayUtility.Add(ref _roomPos, _currentWorldPoint);
        if(type == 1){
            ArrayUtility.Add(ref _roomPos, _currentWorldPoint);
            // _currentWorldPoint += (Vector2)facingPos;
        }
        else if (type == 2){
            ArrayUtility.Add(ref _roomPos, _currentWorldPoint + (Vector2)facingPos);
        }

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
                spawnPoint = transform.position - facingPos * ((roomGate.length + roomGen.length) * 0.5f + 1.0f);
                // spawnPoint = -((roomGate.length + genRoom.length) * 0.5f + 1.0f);
                _tempRoomLength = roomGen.length;
                _tempRoomWidth  = roomGen.width;
                // ArrayUtility.Add(ref _roomPos, _currentWorldPoint);
                // _currentWorldPoint += (Vector2)facingPos;
            }
            else if (type == 2){
                spawnPoint = transform.position + facingPos * ((roomGate.length + _tempRoomLength) * 0.5f + 1.0f);
                // spawnPoint = ((roomGate.length + _tempRoomLength) * 0.5f + 1.0f);
                // ArrayUtility.Add(ref _roomPos, _currentWorldPoint + (Vector2)facingPos);
            }

            // spawnPoint = transform.position + facingPos * spawnPoint;

            Instantiate(
                gate, spawnPoint,
                Quaternion.identity
            );

        }
        // if(type == 1){
        //     ArrayUtility.Add(ref _roomPos, _currentWorldPoint);
        //     _currentWorldPoint += (Vector2)facingPos;
        // }
        // else if (type == 2){
        //     ArrayUtility.Add(ref _roomPos, _currentWorldPoint + (Vector2)facingPos);
        // }
 
        // // _currentWorldPoint += facingPos;
        // ArrayUtility.Add(ref _roomPos, _currentWorldPoint);

        /// ---------------------------------------------------
        /*GameObject room = FindRoom(roomName);

        RoomGeneration genRoom = room.GetComponent<RoomGeneration>();
        WallGeneration wallRoom = room.GetComponent<WallGeneration>();
        
        wallRoom.GateReset();
        wallRoom.setGate(_newPosition * -1, true);
        // foreach(Vector2 vec2 in _emptySpace){wallRoom.setGate(vec2, true);}

        // Instantiate(
        //     room,
        //     transform.position,
        //     Quaternion.identity
        // );
        // if (_roomPos.Length >=1){
            // GameObject gate = FindRoom("path");
            // RoomGeneration roomGate = gate.GetComponent<RoomGeneration>();
        */

        
            // if(_newPosition.x == 0.0f){
            //     roomGate.isVertical = true;
            //     roomGate.length = (int)(tileSize - (_tempRoomWidth + genRoom.width) / 2) - 2;
            // }
            // else{
            //     roomGate.isVertical = false;
            //     roomGate.length = (int)(tileSize - (_tempRoomLength + genRoom.length) / 2) - 2;
            // }

            // WallGeneration wallGate = gate.GetComponent<WallGeneration>();
            // wallGate.GateReset();
            // wallGate.setGate(_newPosition, true);
            // wallGate.setGate(_newPosition * -1, true);

            // Instantiate(
            //     gate,
            //     transform.position - _newPosition * ((roomGate.length + roomGen.length) * 0.5f + 1.0f),
            //     Quaternion.identity
            // );
        // }
        
        // _tempRoomLength = genRoom.length;
        // _tempRoomWidth  = genRoom.width;

        // ArrayUtility.Add(ref _roomPos, transform.position);
    
        /// ---------------------------------------------------
        /*
        GameObject room = FindRoom(roomName);
        RoomGeneration genRoom = room.GetComponent<RoomGeneration>();
        WallGeneration wallRoom = room.GetComponent<WallGeneration>();

        wallRoom.GateReset();
        wallRoom.setGate(directionPos * -1, true);
        // Instantiate(
        //     room,
        //     transform.position + directionPos * tileSize,
        //     Quaternion.identity
        // );
        // if (_roomPos.Length == 0){
        //     _tempRoomLength = genRoom.length;
        //     _tempRoomWidth  = genRoom.width;
        // }
        // else if(_roomPos.Length >=1){
            // GameObject gate = FindRoom("path");
            
            // RoomGeneration roomGate = gate.GetComponent<RoomGeneration>();
            // if(directionPos.x == 0.0f){
            //     roomGate.isVertical = true;
            //     roomGate.length = (int)(tileSize - (_tempRoomWidth + genRoom.width) / 2) - 2;
            // }
            // else {
            //     roomGate.isVertical = false;
            //     roomGate.length = (int)(tileSize - (_tempRoomLength + genRoom.length) / 2) - 2;
            // }

            // WallGeneration wallGate = gate.GetComponent<WallGeneration>();
            // wallGate.GateReset();
            // wallGate.setGate(directionPos, true);
            // wallGate.setGate(directionPos * -1, true);
        */
            // Instantiate(
            //     gate,
            //     transform.position + directionPos * ((roomGate.length + _tempRoomLength) * 0.5f + 1.0f),
            //     Quaternion.identity
            // );
        // }
        // ArrayUtility.Add(ref _roomPos, transform.position + directionPos * tileSize);
    
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
