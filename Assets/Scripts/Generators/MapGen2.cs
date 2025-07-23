using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using Unity.Collections;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Rendering;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class MapGen2 : MonoBehaviour
{
    [SerializeField] private int m_maxRooms = 2;
    [SerializeField] private int m_maxRoomSize = 13;
    [SerializeField] private int m_pathWidth = 1;

    [SerializeField] private GameObject m_roomPrefab;

    [Header("Tilebase")]
    [SerializeField] private TileBase m_tileFloor;
    [SerializeField] private TileBase m_tileWall;
    [SerializeField] private TileBase m_tileGate;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap m_tilemapFloor;
    [SerializeField] private Tilemap m_tilemapWall;
    [SerializeField] private Tilemap m_tilemapGate;

    private List<RoomInfo> _roomInfos = new();

    void Start()
    {
        StartCoroutine(GenerateMap());

        m_tilemapFloor.SetTile(new(0, 0, 0), null);

        StartCoroutine(DeterminePath());
    }

    private void GenerateRoom(Vector3Int localPosition, GameObject roomPrefab)
    {
        BoundsInt l_bounds = InitializeRoomBounds(roomPrefab);
        l_bounds.position += localPosition * m_maxRoomSize;
        _roomInfos.Add(new RoomInfo
        {
            Position = localPosition,
            Size = l_bounds.size
        });

        GenerateFloor(l_bounds);
        GenerateWall(l_bounds);
    }

    private BoundsInt InitializeRoomBounds(GameObject roomPrefab)
    {
        BoundsInt l_bounds = new BoundsInt();
        if (roomPrefab.TryGetComponent(out RoomSize l_roomSize))
        {
            int width = l_roomSize.Width;
            int height = l_roomSize.Height;

            // Create a new bounds for the tilemap
            l_bounds.position = new(-width / 2, -height / 2, 0);
            l_bounds.size = new Vector3Int(width, height, 1);
        }
        else
        {
            Debug.Log("RoomSize component not found on the room prefab.");
        }
        return l_bounds;
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

        GenerateRoom(l_spawnPoint, m_roomPrefab);
        l_spawnPoint += l_adjacentDirection[Random.Range(0, l_adjacentDirection.Count)];
        GenerateRoom(l_spawnPoint, m_roomPrefab);

        // yield return new WaitForEndOfFrame();

        for (int _ = 1; _ < m_maxRooms; _++)
        {
            l_emptySpace.Clear();
            l_emptySpace.AddRange(l_adjacentDirection.Where(
                vec3 =>
                !_roomInfos.Exists(
                    room => room.Position == l_spawnPoint + vec3))
            );

            if (l_emptySpace.Count == 0)
            {
                Debug.Log("No empty space found to place a new room.");
                break;
            }

            Vector3Int l_selectedDirection = l_emptySpace[Random.Range(0, l_emptySpace.Count)];
            l_emptySpace.Remove(l_selectedDirection);

            Vector3Int l_newPosition = l_spawnPoint + l_selectedDirection;
            GenerateRoom(l_newPosition, m_roomPrefab);

            StratergySideRoom(ref l_emptySpace);
            foreach (Vector3Int vec3 in l_emptySpace)
            {
                GenerateRoom(l_spawnPoint + vec3, m_roomPrefab);
            }

            l_spawnPoint = l_newPosition;

            yield return new WaitForEndOfFrame();
        }
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
        int l_temp = 1;
        for (int index = 1; index < _roomInfos.Count; index++)
        {
            while (l_temp <= 4)
            {
                if ((_roomInfos[index - l_temp].Position - _roomInfos[index].Position).sqrMagnitude == 1f)
                {
                    print($"{_roomInfos[index - l_temp].Position} -> {_roomInfos[index].Position}");
                    GeneratePath(_roomInfos[index - l_temp], _roomInfos[index]);
                    l_temp = 1;
                    break;
                }
                l_temp++;
            }
            yield return new WaitForEndOfFrame();
        }
        print("Number of rooms: " + _roomInfos.Count);
    }

    private void GeneratePath(RoomInfo roomInfoStart, RoomInfo roomInfoEnd)
    {
        BoundsInt l_boundsTemp = new BoundsInt(new(0, 0, 0), new(1, 1, 1));
        Vector3Int l_direction = roomInfoEnd.Position - roomInfoStart.Position;
        Vector3Int l_posStart = roomInfoStart.Position * m_maxRoomSize;

        bool isHorizontal = l_direction.y == 0;
        bool isVertical = l_direction.x == 0;
        bool isNegative = (isHorizontal && l_direction.x == -1) || (isVertical && l_direction.y == -1);
        Vector3Int offset = isHorizontal ? new(-m_maxRoomSize, 0, 0) : new(0, -m_maxRoomSize, 0);

        int l_length = m_maxRoomSize - 1 - (isHorizontal
                            ? (roomInfoEnd.Extent.x + roomInfoStart.Extent.x)
                            : (roomInfoEnd.Extent.y + roomInfoStart.Extent.y));

        l_boundsTemp.size = isHorizontal ? new(l_length, m_pathWidth, 1) : new(m_pathWidth, l_length, 1);
        l_boundsTemp.position = l_posStart + (isHorizontal
                            ? new(roomInfoStart.Extent.x + 1, -m_pathWidth / 2, 0)
                            : new(-m_pathWidth / 2, roomInfoStart.Extent.y + 1, 0));

        l_boundsTemp.position += isNegative ? offset : Vector3Int.zero;

        GenerateFloor(l_boundsTemp);
        GenerateWall(l_boundsTemp, l_direction.x == 0, l_direction.y == 0);
        StratergyGate(l_boundsTemp, l_direction.x == 0, l_direction.y == 0);
    }

    private void GenerateFloor(BoundsInt boundsInt)
    {
        m_tilemapFloor.SetTilesBlock(boundsInt, Enumerable.Repeat(m_tileFloor, boundsInt.size.x * boundsInt.size.y).ToArray());
    }

    private void GenerateWall(BoundsInt boundsInt, bool horizontal = true, bool vertical = true)
    {
        // invalidate threshold if any axis has value false
        int l_thresholdH = horizontal ? 0 : 1;
        int l_thresholdV = vertical ? 0 : 1;

        for (int y = 0; y < boundsInt.size.y; y++)
        {
            for (int x = 0; x < boundsInt.size.x; x++)
            {
                if (y == 0 - l_thresholdV || y == boundsInt.size.y - 1 + l_thresholdV ||
                    x == 0 - l_thresholdH || x == boundsInt.size.x - 1 + l_thresholdH)
                {
                    m_tilemapWall.SetTile(new Vector3Int(x, y, 0) + boundsInt.position, m_tileWall);
                }
            }
        }
    }

    private void StratergyGate(BoundsInt boundsInt, bool horizontal = true, bool vertical = true)
    {
        int l_gateLength = horizontal ? boundsInt.size.x : boundsInt.size.y;
        Vector3Int l_gateSize = horizontal ? new(l_gateLength, 1, 1) : new(1, l_gateLength, 1);

        TileBase[] l_gateTiles = Enumerable.Repeat(m_tileGate, l_gateLength).ToArray();
        l_gateTiles[0] = m_tileWall;
        l_gateTiles[l_gateLength - 1] = m_tileWall;

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
                l_index += 1;
            }
            l_bounds.position += horizontal ? new(0, boundsInt.size.y + 1, 0) : new(boundsInt.size.x + 1, 0, 0);
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