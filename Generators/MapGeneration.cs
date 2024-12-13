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

    private float _tempRoomLength; 
    private float _tempRoomWidth;
    private float _timeRoomInterval = 0.0f;
    private string[] _createdRooms = new string[0];
    private string[] _extraRooms = new string[0];
    private string[] _extraRoomsMax = new string[0];
    private Room[] _rooms;
    private Vector2[] _roomPos = new Vector2[0];
    private Vector2 _currentWorldPoint = Vector2.zero;
    private Vector3[] _emptySpace = new Vector3[0];
    private Vector3 _newPosition;

    // Start is called before the first frame update
    void Start()
    {
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

        if (maxRooms <=1){maxRooms = 2;}

        transform.position = startPos[0].position;

        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos);
        _newPosition = _emptySpace[Random.Range(0, _emptySpace.Length)];
        // ArrayUtility.Clear(ref _emptySpace);

        GenerateAllRooms(RoomUtility.RoomType.SideRoom, "home", _newPosition * -1);
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

        RoomUtility.getAdjacentVec3(ref _emptySpace, _currentWorldPoint, _roomPos);

        RoomUtility.Vec3Shuffle(ref _emptySpace);
        RoomUtility.Vec3Reduce(ref _emptySpace, (_extraRooms.Length != 0));

        if (maxRooms > 1){
            GenerateAllRooms(RoomUtility.RoomType.MainRoom, "mob", _newPosition);
        }
        else if (maxRooms == 1){
            ArrayUtility.Clear(ref _emptySpace);
            GenerateAllRooms(RoomUtility.RoomType.MainRoom, "exit", _newPosition);

            RoomDebug.ShowRoomWorldPositions(_roomPos, false);
            RoomDebug.ShowAllRooms(_createdRooms, true);
            return;
        }
        
        _newPosition = _emptySpace[0];
        ArrayUtility.Remove(ref _emptySpace, _newPosition);

        if (_extraRooms.Length != 0){

            string room;
            foreach(Vector3 vec3 in _emptySpace){

                room = _extraRooms[Random.Range(0, _extraRooms.Length)];
                ArrayUtility.Remove(ref _extraRooms, room);

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

        transform.position += _newPosition * tileSize;
        _currentWorldPoint += (Vector2)_newPosition;
    }

    private GameObject FindRoom(string roomName){
        /// <summary>
        /// Find room based on the names defined in the unity
        /// </summary>
        /// <param name="roomName"></param>
        /// <return>A room GameObject</returns>
        
        Room room = Array.Find(_rooms, x => x.name == roomName);
        if (room == null){
            Debug.Log("The room "+ roomName +" not found");
            return FindRoom("null"); 
        }
        else {
            return room.roomObjects [Random.Range(0, room.roomObjects.Length)];
        }
    }

    private void GenerateAllRooms(RoomUtility.RoomType roomType, string roomName, Vector3 facingPos){
        
        float evenOffsetLength = 0.5f;
        float evenOffsetWidth = 0.5f;

        GameObject room = FindRoom(roomName);

        FloorGeneration floorGen = room.GetComponent<FloorGeneration>();
        WallGeneration wallGen = room.GetComponent<WallGeneration>();
        wallGen.GateReset();
        wallGen.setGate(facingPos * -1, true);

        Vector3 spawnPoint = transform.position;

        if(_roomPos.Length > 0){
            if((facingPos.x == 0.0f) && ((floorGen.width + _tempRoomWidth) % 2 != 0)){
                spawnPoint -= facingPos * evenOffsetWidth;
            }
            else if((facingPos.y == 0.0f) && ((floorGen.length + _tempRoomLength) % 2 != 0)){
                spawnPoint -= facingPos * evenOffsetLength;
            }
        }

        if (roomType == RoomUtility.RoomType.MainRoom){
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
        RoomDebug.StoreRoom(ref _createdRooms, room.name);

        if (_roomPos.Length == 1){
            _tempRoomLength = floorGen.length;
            _tempRoomWidth  = floorGen.width;
        }

        else if(_roomPos.Length >=2){
            GameObject gate = FindRoom("path");
            FloorGeneration floorGate = gate.GetComponent<FloorGeneration>();

            if(facingPos.x == 0.0f){
                floorGate.isVertical = true;
                floorGate.length = (int)(tileSize - (_tempRoomWidth + floorGen.width) / 2) - 2;
            }
            else{
                floorGate.isVertical = false;
                floorGate.length = (int)(tileSize - (_tempRoomLength + floorGen.length) / 2) - 2;
            }
            
            WallGeneration wallGate = gate.GetComponent<WallGeneration>();
            wallGate.GateReset();
            wallGate.setGate(facingPos, true);
            wallGate.setGate(facingPos * -1, true);

            if (roomType == RoomUtility.RoomType.MainRoom){
                _tempRoomLength = floorGen.length;
                _tempRoomWidth  = floorGen.width;

                if(facingPos.x == 0.0f){
                    spawnPoint = transform.position - facingPos * ((floorGate.length + floorGen.width) * 0.5f + 1.0f);
                }
                else {
                    spawnPoint = transform.position - facingPos * ((floorGate.length + floorGen.length) * 0.5f + 1.0f);
                }
            }
            else if (roomType == RoomUtility.RoomType.SideRoom){
                if (facingPos.x == 0.0f){
                    spawnPoint = transform.position + facingPos * ((floorGate.length + _tempRoomWidth) * 0.5f + 1.0f);
                }
                else {
                    spawnPoint = transform.position + facingPos * ((floorGate.length + _tempRoomLength) * 0.5f + 1.0f);
                }
            }
            Instantiate(
                gate, spawnPoint,
                Quaternion.identity
            );
        }
    }

}