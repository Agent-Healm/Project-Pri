using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen2 : MonoBehaviour
{
    [SerializeField] private int m_maxRooms = 2;
    [SerializeField] private int m_tileSize = 13;

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

    private void GenerateRoom(Vector3Int l_localPosition, GameObject l_roomPrefab)
    {
        BoundsInt bounds = InitializeRoomBounds(l_roomPrefab);

        _roomInfos.Add(new RoomInfo
        {
            Position = l_localPosition,
            Size = bounds.size
        });

        l_localPosition *= m_tileSize;
        Vector3Int boundsExtent = (bounds.size - new Vector3Int(1, 1, 1)) / 2;

        for (int y = -boundsExtent.y; y <= boundsExtent.y; y++)
        {
            for (int x = -boundsExtent.x; x <= boundsExtent.x; x++)
            {
                m_tilemapFloor.SetTile(new Vector3Int(x, y, 0) + l_localPosition, m_tileFloor);

                if (y == -boundsExtent.y ||
                    y == boundsExtent.y ||
                    x == -boundsExtent.x ||
                    x == boundsExtent.x)
                {
                    m_tilemapWall.SetTile(new Vector3Int(x, y, 0) + l_localPosition, m_tileWall);
                }
            }
        }
    }

    private BoundsInt InitializeRoomBounds(GameObject l_roomPrefab)
    {
        BoundsInt bounds = new BoundsInt();
        if (l_roomPrefab.TryGetComponent(out RoomSize l_roomSize))
        {
            int width = l_roomSize.Width;
            int height = l_roomSize.Height;

            // Create a new bounds for the tilemap
            bounds.position = new(0, 0, 0);
            bounds.size = new Vector3Int(width, height, 1);
        }
        else
        {
            Debug.Log("RoomSize component not found on the room prefab.");
        }
        return bounds;
    }

    private IEnumerator GenerateMap()
    {
        Vector3Int l_spawnPoint = new(0, 0, 0);
        Vector3Int[] adjacentDirection = new Vector3Int[]{
            new Vector3Int(1, 0, 0), // Right
            new Vector3Int(-1, 0, 0), // Left
            new Vector3Int(0, 1, 0), // Up
            new Vector3Int(0, -1, 0) // Down
        };
        List<Vector3Int> emptySpace = new List<Vector3Int>();
        GenerateRoom(l_spawnPoint, m_roomPrefab);

        for (int _ = 0; _ < m_maxRooms; _++)
        {
            emptySpace.Clear();
            foreach (Vector3Int vec3 in adjacentDirection)
            {
                if (_roomInfos.FindIndex(x => x.Position.Equals(l_spawnPoint + vec3)) == -1)
                {
                    emptySpace.Add(vec3);
                }
            }

            if (emptySpace.Count == 0)
            {
                Debug.Log("No empty space found to place a new room.");
                break;
            }

            int randomIndex = Random.Range(0, emptySpace.Count);
            Vector3Int direction = emptySpace[randomIndex];
            emptySpace.RemoveAt(randomIndex);

            GenerateRoom(l_spawnPoint + direction, m_roomPrefab);

            if (emptySpace.Count > 1)
            {
                SideRoomStratergy(ref emptySpace);
                foreach (Vector3Int vec3 in emptySpace)
                {
                    GenerateRoom(l_spawnPoint + vec3, m_roomPrefab);
                }
            }
            l_spawnPoint += direction;

            yield return new WaitForEndOfFrame();
        }
    }

    private void SideRoomStratergy(ref List<Vector3Int> l_emptySpace)
    {
        int randomIndex = Random.Range(1, l_emptySpace.Count);
        for (int _ = 0; _ < randomIndex; _++)
        {
            l_emptySpace.RemoveAt(Random.Range(0, l_emptySpace.Count));
        }
    }

    private IEnumerator DeterminePath()
    {
        int temp = 1;
        for (int index = 1; index < _roomInfos.Count; index++)
        {
            while (temp <= 4)
            {
                if ((_roomInfos[index - temp].Position - _roomInfos[index].Position).sqrMagnitude == 1f)
                {
                    print($"{_roomInfos[index - temp].Position} -> {_roomInfos[index].Position}");
                    temp = 1;
                    break;
                }
                temp++;
            }
            yield return new WaitForEndOfFrame();
        }
    }
}

struct RoomInfo
{
    public Vector3Int Position { get; set; }
    public Vector3Int Size { get; set; }
}