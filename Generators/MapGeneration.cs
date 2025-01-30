using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

using Random = UnityEngine.Random;

public class MapGeneration : MonoBehaviour
{
    public Transform[] startPos;
    public int maxRooms = 3;
    public float tileSize = 1.0f;

    private float _prevRoomLength; 
    private float _prevRoomWidth;
    private float _tempRoomInterval = 0.0f;
    private string[] _debug_createdRooms = new string[0];
    private string[] _extraRooms = new string[0];
    private string[] _extraRoomsMax = new string[0];
    private Room[] _rooms;
    private Vector2[] _roomPos = new Vector2[0];
    private Vector2 _currentWorldPoint = Vector2.zero;
    private Vector3[] _emptySpace = new Vector3[0];
    private Vector3 _newFacing;

    // Start is called before the first frame update
    void Start()
    {
        InitialRoomAssign();
        transform.position = startPos[0].position;

        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos);
        _newFacing = _emptySpace[Random.Range(0, _emptySpace.Length)];

        // GenerateAllRooms(RoomUtility.RoomType.SideRoom, "home", - _newFacing);
        
        // Generate first room using main room function
        ArrayUtility.Clear(ref _emptySpace);
        GenerateRoom(RoomUtility.RoomType.MainRoom, "home", - _newFacing);
        MoveToNextTile(_newFacing);

        // Generate first room using side room function
        // GenerateRoom(RoomUtility.RoomType.SideRoom, "home", - _newFacing);
    }

    void Update(){
        if(maxRooms > 0){
            if(_tempRoomInterval <= 0 ){
                procGenRoom();
                _tempRoomInterval = 0.2f;
                maxRooms -= 1;
            }
            else {
                _tempRoomInterval -= Time.deltaTime;
            }
        }
    }
    /// <summary>
    /// Return the RoomObject using name
    /// </summary>
    /// <remarks>Find the room based on its name</remarks>
    /// <param name="roomName">Literal string of a room name.</param>
    /// <returns>A Room GameObject if found</returns>
    private GameObject FindRoom(string roomName){
        
        Room room = Array.Find(_rooms, x => x.name == roomName);
        if (room == null){
            Debug.Log("The room "+ roomName +" not found");
            return FindRoom("null"); 
        }
        else {
            return room.roomObjects [Random.Range(0, room.roomObjects.Length)];
        }
    }
    private void procGenRoom(){

        _emptySpace = new Vector3[0];

        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos);

        RoomUtility.Vec3Shuffle(ref _emptySpace);
        RoomUtility.Vec3Reduce(ref _emptySpace, (_extraRooms.Length != 0));

        if (maxRooms > 1){
            GenerateAllRooms(RoomUtility.RoomType.MainRoom, "mob", _newFacing);
        }
        else if (maxRooms == 1){
            ArrayUtility.Clear(ref _emptySpace);
            GenerateAllRooms(RoomUtility.RoomType.MainRoom, "exit", _newFacing);

            RoomDebug.ShowRoomWorldPositions(_roomPos, false);
            RoomDebug.ShowAllRooms(_debug_createdRooms, true);
            return;
        }
        
        _newFacing = RoomUtility.PopArray(ref _emptySpace, -1);

        if (_extraRooms.Length != 0){

            string room;
            foreach(Vector3 vec3 in _emptySpace){

                room = RoomUtility.PopArray(ref _extraRooms, -1);

                GenerateAllRooms(RoomUtility.RoomType.SideRoom, room, vec3);

                if (_extraRooms.Length == 0){
                    if(_extraRoomsMax.Length != 0){
                        ArrayUtility.AddRange(ref _extraRooms, _extraRoomsMax);
                        ArrayUtility.Clear(ref _extraRoomsMax);
                    }
                    else {break;}
                }
            }
        }

        MoveToNextTile(_newFacing);
    } 
    private void GenerateAllRooms(RoomUtility.RoomType roomType, string roomName, Vector3 facingPos){
        
        int gateLength = GenerateRoom(roomType, roomName, facingPos);
        if (_roomPos.Length >1){
        GeneratePath(roomType, facingPos, gateLength);
        }        
        // float evenOffsetLength = 0.5f;
        // float evenOffsetWidth = 0.5f;

        // GameObject room = FindRoom(roomName);

        // FloorGeneration floorGen = room.GetComponent<FloorGeneration>();
        // WallGeneration wallGen = room.GetComponent<WallGeneration>();
        // wallGen.GateReset();
        // wallGen.setGate(facingPos * -1, true);

        // Vector3 spawnPoint = transform.position;

        // if(_roomPos.Length > 0){
        //     if((facingPos.x == 0.0f) && ((floorGen.width + _prevRoomWidth) % 2 != 0)){
        //         spawnPoint -= facingPos * evenOffsetWidth;
        //     }
        //     else if((facingPos.y == 0.0f) && ((floorGen.length + _prevRoomLength) % 2 != 0)){
        //         spawnPoint -= facingPos * evenOffsetLength;
        //     }
        // }

        // if (roomType == RoomUtility.RoomType.MainRoom){
        //     // reposition for offset 
        //     transform.position = spawnPoint;
        //     foreach(Vector2 vec2 in _emptySpace){wallGen.setGate(vec2, true);}
        //     ArrayUtility.Add(ref _roomPos, _currentWorldPoint);
        // }
        // else if (roomType == RoomUtility.RoomType.SideRoom){
        //     spawnPoint += (facingPos * tileSize);
        //     ArrayUtility.Add(ref _roomPos, _currentWorldPoint + (Vector2)facingPos);
        // }

        // Instantiate(
        //     room, spawnPoint,
        //     Quaternion.identity
        // );
        // RoomDebug.StoreRoom(ref _debug_createdRooms, room.name);


        // if (_roomPos.Length == 1){
        //     _prevRoomWidth  = floorGen.width;
        //     _prevRoomLength = floorGen.length;
        // }
        
        // if(_roomPos.Length >=2){
        //     GameObject gate = FindRoom("path");
        //     FloorGeneration floorGate = gate.GetComponent<FloorGeneration>();

        //     // dynamic length of the gate and orientation
        //     if(facingPos.x == 0.0f) {
        //         floorGate.isVertical = true;
        //         floorGate.length = (int)(tileSize - (_prevRoomWidth + floorGen.width) / 2) - 2;
        //     }
        //     else {
        //         floorGate.isVertical = false;
        //         floorGate.length = (int)(tileSize - (_prevRoomLength + floorGen.length) / 2) - 2;
        //     }
            
        //     if (roomType == RoomUtility.RoomType.MainRoom){
        //         _prevRoomLength = floorGen.length;
        //         _prevRoomWidth  = floorGen.width;
        //         facingPos = - facingPos;
        //     }

        //     WallGeneration wallGate = gate.GetComponent<WallGeneration>();
        //     wallGate.GateReset();
        //     wallGate.setGate(facingPos, true);
        //     wallGate.setGate(facingPos * -1, true);

        //     if(facingPos.x == 0.0f){
        //         spawnPoint = transform.position + facingPos * ((floorGate.length + _prevRoomWidth) * 0.5f + 1.0f);
        //     }
        //     else {
        //         spawnPoint = transform.position + facingPos * ((floorGate.length + _prevRoomLength) * 0.5f + 1.0f);
        //     }

        //     Instantiate(
        //         gate, spawnPoint,
        //         Quaternion.identity
        //     );
        // }
        
    }    
    private void GeneratePath(RoomUtility.RoomType roomType, Vector3 facingPos, int gateLength = 1){

        Vector3 spawnPoint;

        GameObject gate = FindRoom("path");
        FloorGeneration floorGate = gate.GetComponent<FloorGeneration>();
        floorGate.length = gateLength;

        // dynamic length of the gate and orientation
        floorGate.isVertical = (facingPos.x == 0.0f);
        // if(facingPos.x == 0.0f) {
        //     floorGate.isVertical = true;
        // }
        // else {
        //     floorGate.isVertical = false;
        // }

        if (roomType == RoomUtility.RoomType.MainRoom){
            facingPos = - facingPos;
        }

        WallGeneration wallGate = gate.GetComponent<WallGeneration>();
        wallGate.GateReset();
        wallGate.setGate(facingPos, true);
        wallGate.setGate(facingPos * -1, true);

        if(facingPos.x == 0.0f){
            spawnPoint = transform.position + facingPos * ((floorGate.length + _prevRoomWidth) * 0.5f + 1.0f);
        }
        else {
            spawnPoint = transform.position + facingPos * ((floorGate.length + _prevRoomLength) * 0.5f + 1.0f);
        }

        Instantiate(
            gate, spawnPoint,
            Quaternion.identity
        );
    }
    private int GenerateRoom(RoomUtility.RoomType roomType, string roomName, Vector3 facingPos){
        
        int gateLength;
        float evenOffsetLength = 0.5f;
        float evenOffsetWidth = 0.5f;

        GameObject room = FindRoom(roomName);

        FloorGeneration floorGen = room.GetComponent<FloorGeneration>();
        WallGeneration wallGen = room.GetComponent<WallGeneration>();
        wallGen.GateReset();
        wallGen.setGate(facingPos * -1, true);

        Vector3 spawnPoint = transform.position;

        if(_roomPos.Length > 0){
            if((facingPos.x == 0.0f) && ((floorGen.width + _prevRoomWidth) % 2 != 0)){
                spawnPoint -= facingPos * evenOffsetWidth;
            }
            else if((facingPos.y == 0.0f) && ((floorGen.length + _prevRoomLength) % 2 != 0)){
                spawnPoint -= facingPos * evenOffsetLength;
            }
        }

        if (roomType == RoomUtility.RoomType.MainRoom){
            // reposition for offset 
            transform.position = spawnPoint;
            foreach(Vector2 vec2 in _emptySpace){wallGen.setGate(vec2, true);}
            ArrayUtility.Add(ref _roomPos, _currentWorldPoint);
        }
        else if (roomType == RoomUtility.RoomType.SideRoom){
            spawnPoint += (facingPos * tileSize);
            ArrayUtility.Add(ref _roomPos, _currentWorldPoint + (Vector2)facingPos);
        }

        Instantiate(
            room, spawnPoint,
            Quaternion.identity
        );
        RoomDebug.StoreRoom(ref _debug_createdRooms, room.name);

        if (_roomPos.Length == 1){
            _prevRoomLength = floorGen.length;
            _prevRoomWidth  = floorGen.width;
            return 0;
        }

        // dynamic length of the gate and orientation
        if(facingPos.x == 0.0f) {
            gateLength = (int)(tileSize - (_prevRoomWidth + floorGen.width) / 2) - 2;
        }
        else {
            gateLength = (int)(tileSize - (_prevRoomLength + floorGen.length) / 2) - 2;
        }

        if (roomType == RoomUtility.RoomType.MainRoom){
            _prevRoomLength = floorGen.length;
            _prevRoomWidth  = floorGen.width;
        }
        return gateLength;
    }
    private void InitialRoomAssign(){
        if (maxRooms <=1){maxRooms = 2;}

        _rooms = RoomConfig.instance.rooms;
        
        foreach(Room room in _rooms){
            if(room.isIncluded){
                for (int i = 1 ; i <= room.min ; i++){
                    ArrayUtility.Add(ref _extraRooms, room.name);
                }
                for (int i = 1 ; i <= room.max - room.min ; i++){
                    ArrayUtility.Add(ref _extraRoomsMax, room.name);
                }
            }
        }
    }
    private void MoveToNextTile(Vector3 _newFacing){
        transform.position += _newFacing * tileSize;
        _currentWorldPoint += (Vector2)_newFacing;
    }
}