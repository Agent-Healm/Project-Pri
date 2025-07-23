using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.Tilemaps;

public class MapGen2 : MonoBehaviour
{
    [SerializeField] private int m_maxRooms = 2;
    [SerializeField] private int m_maxRoomSize = 13;
    [SerializeField] private int m_pathWidth = 1;

    [SerializeField] private GameObject m_roomPrefab;

    [Header("Tilebase")]
    [SerializeField] private TileBase m_tileFloor;
    [SerializeField] private TileBase m_tileWall;

    [Header("Tilemaps")]
    [SerializeField] private Tilemap m_tilemapFloor;
    [SerializeField] private Tilemap m_tilemapWall;

    // [SerializeField] private GameObject m_pathPrefab;
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
            l_emptySpace.AddRange(l_adjacentDirection.Where(vec3 => _roomInfos.FindIndex(x => x.Position.Equals(l_spawnPoint + vec3)) == -1));

            if (l_emptySpace.Count == 0)
            {
                Debug.Log("No empty space found to place a new room.");
                break;
            }

            Vector3Int l_direction = l_emptySpace[Random.Range(0, l_emptySpace.Count)];
            l_emptySpace.Remove(l_direction);

            GenerateRoom(l_spawnPoint + l_direction, m_roomPrefab);

            SideRoomStratergy(ref l_emptySpace);
            foreach (Vector3Int vec3 in l_emptySpace)
            {
                GenerateRoom(l_spawnPoint + vec3, m_roomPrefab);
            }

            l_spawnPoint += l_direction;

            yield return new WaitForEndOfFrame();
        }
    }

    private void SideRoomStratergy(ref List<Vector3Int> emptySpace)
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

        if (l_direction.y == 0)
        {   // horizontal path
            int l_length = m_maxRoomSize - roomInfoEnd.Extent.x - roomInfoStart.Extent.x - 1;
            l_boundsTemp.size = new Vector3Int(l_length, m_pathWidth, 1);
            l_boundsTemp.position = l_posStart + new Vector3Int(roomInfoStart.Extent.x + 1, -m_pathWidth / 2, 0);

            if (l_direction.x == -1)
            {
                l_boundsTemp.position += new Vector3Int(-m_maxRoomSize, 0, 0);
            }
        }
        else if (l_direction.x == 0)
        {   // vertical path
            int l_length = m_maxRoomSize - roomInfoEnd.Extent.y - roomInfoStart.Extent.y - 1;
            l_boundsTemp.size = new Vector3Int(m_pathWidth, l_length, 1);
            l_boundsTemp.position = l_posStart + new Vector3Int(-m_pathWidth / 2, roomInfoStart.Extent.y + 1, 0);
            if (l_direction.y == -1)
            {
                l_boundsTemp.position += new Vector3Int(0, -m_maxRoomSize, 0);
            }
        }
        GenerateFloor(l_boundsTemp);
        GenerateWall(l_boundsTemp, l_direction.x == 0, l_direction.y == 0);
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

}

struct RoomInfo
{
    public Vector3Int Position { get; set; }
    public Vector3Int Size { get; set; }

    public Vector3Int Extent
    {
        get
        {
            return (Size - new Vector3Int(1, 1, 1)) / 2;
        }
    }

}