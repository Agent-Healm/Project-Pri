using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Unity.Collections;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Tilemaps;

using Random = UnityEngine.Random;
public class MapGen2 : MonoBehaviour
{
    [SerializeField] private int m_maxRooms = 2;
    [SerializeField] private int m_maxRoomSize = 13;
    [SerializeField] private int m_pathWidth = 1;

    [Header("Texture Pack")]
    [SerializeField] private TileTheme tileTheme;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap m_tilemapFloor;
    [SerializeField] private Tilemap m_tilemapWall;
    [SerializeField] private Tilemap m_tilemapGate;

    private TileBase _tileGate;
    private TileBase _tileFloor;
    private TileBase _tileWall;
    
    private GameObject _roomHome;
    private GameObject _roomMob;
    private GameObject _roomSpecial;
    private GameObject _roomExit;
    private List<RoomInfo> _roomInfos = new();

    private void InitializeTiles()
    {
        _tileFloor = tileTheme.FloorTile;
        _tileWall = tileTheme.WallTile;
        _tileGate = tileTheme.GateTile;

        _roomHome = tileTheme.HomeRoom;
        _roomMob = tileTheme.MobRoom;
        _roomSpecial = tileTheme.SpecialRoom;
        _roomExit = tileTheme.ExitRoom;
    }
    private IEnumerator Start()
    {
        InitializeTiles();
        yield return StartCoroutine(GenerateMap());
        yield return StartCoroutine(DetermineRoom());
        yield return StartCoroutine(DeterminePath());
        m_tilemapFloor.SetTile(new(0, 0, 0), null);
    }

    private void GenerateRoom(int index, GameObject roomPrefab)
    {
        BoundsInt l_bounds = roomPrefab.GetComponent<RoomSize>().RoomBounds;
        RoomInfo l_roomPos = _roomInfos[index];
        l_bounds.position += l_roomPos.Position * m_maxRoomSize;
        l_roomPos.Size = l_bounds.size;

        _roomInfos[index] = l_roomPos;
        GenerateFloor(l_bounds);
        GenerateWall(l_bounds);
        GenerateLayout(l_bounds, roomPrefab.GetComponent<RoomSize>());
    }

    private void SelectPosition(Vector3Int vec)
    {
        _roomInfos.Add(new RoomInfo
        {
            Position = vec
        });
    }

    private IEnumerator DetermineRoom()
    {
        GameObject l_roomPrefab;
        GenerateRoom(0, _roomHome);
        GenerateRoom(1, _roomMob);
        yield return null;

        yield return IterateRoomLinks(2, (index, current, isAdjacent) =>
        RoomSelect(index, isAdjacent));

        IEnumerator RoomSelect(int index, bool isAdjacent)
        {
            l_roomPrefab = isAdjacent ? _roomSpecial : _roomMob;
            if (index == _roomInfos.Count - 1)
            {
                GenerateRoom(index, _roomExit);
            }
            else
            {
                GenerateRoom(index, l_roomPrefab);
            }
            yield return null;
        }
    }

    private IEnumerator GenerateMap()
    {
        Vector3Int l_spawnPoint = new(0, 0, 0);
        ReadOnlyArray<Vector3Int> l_adjacentDirection = new Vector3Int[]{
            new Vector3Int(1, 0, 0), // Right
            new Vector3Int(-1, 0, 0), // Left
            new Vector3Int(0, 1, 0), // Up
            new Vector3Int(0, -1, 0) // Down
        };
        List<Vector3Int> l_emptySpace = new();
        SelectPosition(l_spawnPoint);

        l_spawnPoint += l_adjacentDirection[Random.Range(0, l_adjacentDirection.Count)];
        SelectPosition(l_spawnPoint);
        // print($"Spawning basic room at {l_spawnPoint}");

        yield return null;

        for (int _ = 1; _ < m_maxRooms; _++)
        {
            l_emptySpace.Clear();
            l_emptySpace.AddRange(l_adjacentDirection.Where(vec3 =>
                !_roomInfos.Exists(room => room.Position == l_spawnPoint + vec3))
            );

            if (l_emptySpace.Count == 0)
            {
                Debug.Log("No empty space found to place a new room.");
                break;
            }

            Vector3Int l_selectedDirection = l_emptySpace[Random.Range(0, l_emptySpace.Count)];
            l_emptySpace.Remove(l_selectedDirection);

            Vector3Int l_newPosition = l_spawnPoint + l_selectedDirection;
            SelectPosition(l_newPosition);
            // print($"Spawning basic room at {l_newPosition}");

            StratergySideRoom(ref l_emptySpace);
            foreach (Vector3Int vec3 in l_emptySpace)
            {
                SelectPosition(l_spawnPoint + vec3);
            }

            l_spawnPoint = l_newPosition;
            yield return null;
        }

        // foreach (RoomInfo roomInfo in _roomInfos)
        // {
        //     Debug.Log($"Room at : {roomInfo.Position}");
        // }
    }

    private void StratergySideRoom(ref List<Vector3Int> emptySpace)
    {
        if (emptySpace.Count == 0)
        {
            return; // No need to remove any rooms if there's only one or no empty space
        }
        int l_randomIndex = Random.Range(1, emptySpace.Count);
        for (int _ = 0; _ < l_randomIndex; _++)
        {
            emptySpace.RemoveAt(Random.Range(0, emptySpace.Count));
        }
        // l_emptySpace.Clear();
    }

    private IEnumerator DeterminePath()
    {
        yield return IterateRoomLinks(1, (index, tempCurrent, isAdjacent) =>
        PathConnect(index, tempCurrent)
        );

        IEnumerator PathConnect(int index, int tempCurrent)
        {
            GeneratePath(_roomInfos[tempCurrent], _roomInfos[index]);
            yield return null;
        }
    }

    private void GeneratePath(RoomInfo roomInfoStart, RoomInfo roomInfoEnd)
    {
        BoundsInt l_boundsTemp = new BoundsInt(new(0, 0, 0), new(1, 1, 1));
        Vector3Int l_direction = roomInfoEnd.Position - roomInfoStart.Position;
        Vector3Int l_posStart = roomInfoStart.Position * m_maxRoomSize;

        bool isHorizontal = l_direction.y == 0;
        bool isVertical = l_direction.x == 0;
        bool isNegative = (isHorizontal && l_direction.x == -1) || (isVertical && l_direction.y == -1);

        int l_length = m_maxRoomSize - 1 - (isHorizontal
                            ? (roomInfoEnd.Extent.x + roomInfoStart.Extent.x)
                            : (roomInfoEnd.Extent.y + roomInfoStart.Extent.y));
        Vector3Int offset = isHorizontal ? new(-roomInfoStart.Size.x - l_length, 0, 0)
                                        : new(0, -roomInfoStart.Size.y - l_length, 0);

        l_boundsTemp.size = isHorizontal ? new(l_length, m_pathWidth, 1) : new(m_pathWidth, l_length, 1);
        l_boundsTemp.position = l_posStart + (isHorizontal
                            ? new(roomInfoStart.Extent.x + 1, -m_pathWidth / 2, 0)
                            : new(-m_pathWidth / 2, roomInfoStart.Extent.y + 1, 0));

        l_boundsTemp.position += isNegative ? offset : Vector3Int.zero;

        GenerateFloor(l_boundsTemp);
        GenerateWall(l_boundsTemp, l_direction.x == 0, l_direction.y == 0);
        GenerateGate(l_boundsTemp, l_direction.x == 0, l_direction.y == 0);
    }

    private void GenerateFloor(BoundsInt boundsInt)
    {
        m_tilemapFloor.SetTilesBlock(boundsInt, Enumerable.Repeat(_tileFloor, boundsInt.size.x * boundsInt.size.y).ToArray());
    }

    private void GenerateWall(BoundsInt boundsInt, bool drawHorizontal = true, bool drawVertical = true)
    {
        // Vector3Int l_tempPos = boundsInt.position;
        if (drawVertical)
        {
            for (int x = 0; x < boundsInt.size.x; x++)
            {
                m_tilemapWall.SetTile(boundsInt.position + new Vector3Int(x, 0, 0), _tileWall);
                m_tilemapWall.SetTile(boundsInt.position + new Vector3Int(x, boundsInt.size.y - 1, 0), _tileWall);
            }
        }

        if (drawHorizontal)
        {
            for (int y = 0; y < boundsInt.size.y; y++)
            {
                m_tilemapWall.SetTile(boundsInt.position + new Vector3Int(0, y, 0), _tileWall);
                m_tilemapWall.SetTile(boundsInt.position + new Vector3Int(boundsInt.size.x - 1, y, 0), _tileWall);
            }
        }
    }

    private void GenerateGate(BoundsInt boundsInt, bool horizontal = true, bool vertical = true)
    {
        int l_gateLength = horizontal ? boundsInt.size.x : boundsInt.size.y;
        Vector3Int l_gateSize = horizontal ? new(l_gateLength, 1, 1) : new(1, l_gateLength, 1);

        TileBase[] l_gateTiles = Enumerable.Repeat(_tileGate, l_gateLength).ToArray();
        l_gateTiles[0] = _tileWall;
        l_gateTiles[l_gateLength - 1] = _tileWall;

        BoundsInt l_bounds = boundsInt;
        l_bounds.size = l_gateSize;

        l_bounds.position += horizontal ? new(0, -1, 0) : new(-1, 0, 0);
        int l_index;
        for (int _ = 0; _ < 2; _++)
        {
            l_index = 0;
            foreach (Vector3Int i_pos in l_bounds.allPositionsWithin)
            {
                if (!(l_index == 0 || l_index == l_gateLength - 1))
                {
                    m_tilemapGate.SetTile(i_pos, l_gateTiles[l_index]);
                    m_tilemapWall.SetTile(i_pos, null);
                }
                else
                {
                    m_tilemapWall.SetTile(i_pos, l_gateTiles[l_index]);

                }
                l_index += 1;
            }
            l_bounds.position += horizontal ? new(0, boundsInt.size.y + 1, 0) : new(boundsInt.size.x + 1, 0, 0);
        }
    }

    private void GenerateLayout(BoundsInt boundsInt, RoomSize roomSize)
    {
        boundsInt = ShrinkBounds(boundsInt);
        BoundsInt roomBounds = ShrinkBounds(roomSize.RoomBounds);
        foreach (Tilemap tilemap in roomSize.RoomLayout)
        {
            m_tilemapWall.SetTilesBlock(boundsInt, tilemap.GetTilesBlock(roomBounds));
        }
    }

    private BoundsInt ShrinkBounds(BoundsInt boundsInt, int size = 1)
    {
        boundsInt.position += new Vector3Int(size, size, 0);
        boundsInt.size -= new Vector3Int(2 * size, 2 * size, 0);
        return boundsInt;
    }

    private IEnumerator IterateRoomLinks(int startIndex, Func<int, int, bool, IEnumerator> action)
    {
        int l_tempCurrent = 0;
        int l_tempNext = 1;
        for (int l_index = startIndex; l_index < _roomInfos.Count; l_index++)
        {
            bool l_isAdjacent = (_roomInfos[l_index].Position - _roomInfos[l_tempCurrent].Position).sqrMagnitude == 1;

            l_tempCurrent = l_isAdjacent ? l_tempCurrent : l_tempNext;
            l_tempNext = l_isAdjacent ? l_tempNext : l_index;

            yield return action(l_index, l_tempCurrent, l_isAdjacent);
        }
    }


}

struct RoomInfo
{
    public Vector3Int Position;
    public Vector3Int Size;
    public Vector3Int Extent
    {
        get
        {
            return (Size - new Vector3Int(1, 1, 1)) / 2;
        }
    }
}