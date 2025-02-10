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

    private int _numberOfRooms = 0;
    private float _prevRoomLength; 
    private float _prevRoomWidth;
    private float _tempRoomInterval = 0.0f;

    private Room[] _rooms;
    private string[] _debug_createdRooms = new string[0];
    private string[] _extraRooms = new string[0];
    private string[] _extraRoomsMax = new string[0];

    private GameObject _gate;
    private int _gateWidth;

    private Vector2 _currentWorldPoint = Vector2.zero;
    private Vector2[] _roomPos = new Vector2[0];
    private Vector3 _newFacing;
    private Vector3[] _emptySpace = new Vector3[0];

    void Start(){
        InitialRoomAssign();
        _gate = RoomConfig.instance.path.roomVariance[0];
        // _gateWidth = _gate.GetComponent<WallGeneration>();
        // _gate.GetComponent<WallGeneration>().gateWidth = ;

        transform.position = startPos[0].position;

        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos);
        _newFacing = _emptySpace[Random.Range(0, _emptySpace.Length)];
        
        // Generate first room using main room function
        ArrayUtility.Clear(ref _emptySpace);
        GenerateRoom(RoomUtility.RoomType.MainRoom, "home", _newFacing);
        MoveToNextTile(_newFacing);
    }
    void Update(){
        if(maxRooms > _numberOfRooms){
            if(_tempRoomInterval <= 0 ){
                procGenRoom();
                _tempRoomInterval = 0.2f;
                _numberOfRooms += 1;
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
            return room.roomVariance [Random.Range(0, room.roomVariance.Length)];
        }
    }
    private void GenerateAllRooms(RoomUtility.RoomType roomType, string roomName, Vector3 facingPos){
        int gateLength = GenerateRoom(roomType, roomName, facingPos);
        GeneratePath(roomType, facingPos, gateLength);
    }    
    private void GeneratePath(RoomUtility.RoomType roomType, Vector3 facingPos, int gateLength = 1){

        // GameObject gate = RoomConfig.instance.path.roomVariance[0];
        FloorGeneration floorGate = _gate.GetComponent<FloorGeneration>();
        floorGate.length = gateLength;
        floorGate.isVertical = (facingPos.x == 0.0f);

        WallGeneration wallGate = _gate.GetComponent<WallGeneration>();
        wallGate.GateReset();
        wallGate.setGate(facingPos, true);
        wallGate.setGate(- facingPos, true);

        if (roomType == RoomUtility.RoomType.MainRoom){
            facingPos = - facingPos;
        }

        Vector3 spawnPoint;

        if(facingPos.x == 0.0f){
            spawnPoint = transform.position + facingPos * ((gateLength + _prevRoomWidth) * 0.5f + 1.0f);
        }
        else {
            spawnPoint = transform.position + facingPos * ((gateLength + _prevRoomLength) * 0.5f + 1.0f);
        }

        Instantiate(
            _gate, spawnPoint,
            Quaternion.identity
        );
    }
    private int  GenerateRoom(RoomUtility.RoomType roomType, string roomName, Vector3 facingPos){
        
        int gateLength;
        float evenOffsetLength = 0.5f;
        float evenOffsetWidth = 0.5f;

        GameObject room = FindRoom(roomName);

        FloorGeneration floorGen = room.GetComponent<FloorGeneration>();
        WallGeneration wallGen = room.GetComponent<WallGeneration>();
        wallGen.GateReset();
        wallGen.setGate(- facingPos, true);

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
            _newFacing = - _newFacing;
        }
        return gateLength;
    }
    private void InitialRoomAssign(){
        if (maxRooms <=1){maxRooms = 2;}

        _rooms = RoomConfig.instance.rooms;
        ArrayUtility.AddRange(ref _rooms, RoomConfig.instance.basicRooms);

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
    private void procGenRoom(){

        _emptySpace = new Vector3[0];

        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos);

        RoomUtility.Vec3Shuffle(ref _emptySpace);
        RoomUtility.Vec3Reduce(ref _emptySpace, (_extraRooms.Length != 0));

        // if (maxRooms > 1){
        if (maxRooms > _numberOfRooms + 1){
            GenerateAllRooms(RoomUtility.RoomType.MainRoom, "mob", _newFacing);
        }
        // else if (maxRooms == 1){
        else if (maxRooms - _numberOfRooms == 1){
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
    
}