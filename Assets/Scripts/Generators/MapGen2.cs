using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGen2 : MonoBehaviour
{
    // [SerializeField] private Transform m_spawnPoint;
    [SerializeField] private int m_maxRooms = 2;
    [SerializeField] private int m_tileSize = 13;

    [SerializeField] private GameObject m_roomPrefab;
    [SerializeField] private TileBase m_tileBase;

    private Grid _Grid;
    private Tilemap _tilemap;

    private List<Vector3Int> _roomPositions;
    // [SerializeField] private GameObject m_pathPrefab;

    // Start is called before the first frame update
    void Start()
    {
        _Grid = GetComponent<Grid>();
        _tilemap = GetComponentInChildren<Tilemap>();

        // GenerateRoom(new(0, 0, 0));
        // GenerateRoom(new(0, 1, 0));
        // GenerateRoom(new(0, 2, 0));
        GenerateMap();

        _tilemap.SetTile(new(0, 0, 0), null);

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateRoom(Vector3Int l_localPosition)
    {
        l_localPosition *= m_tileSize;
        BoundsInt bounds = InitializeRoomBounds();
        Vector3Int boundsExtent = (bounds.size - new Vector3Int(1, 1, 1)) / 2;

        for (int y = -boundsExtent.y; y <= boundsExtent.y; y++)
        {
            for (int x = -boundsExtent.x; x <= boundsExtent.x; x++)
            {
                _tilemap.SetTile(new Vector3Int(x, y, 0) + l_localPosition, m_tileBase);
            }
        }
    }

    private BoundsInt InitializeRoomBounds()
    {
        BoundsInt bounds = new BoundsInt();
        if (m_roomPrefab.TryGetComponent<RoomSize>(out RoomSize l_roomSize))
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

    private void GenerateMap()
    {
        Vector3Int l_spawnPoint = new(0, 0, 0);
        Vector3Int[] adjacentDirection = new Vector3Int[]{
            new Vector3Int(1, 0, 0), // Right
            new Vector3Int(-1, 0, 0), // Left
            new Vector3Int(0, 1, 0), // Up
            new Vector3Int(0, -1, 0) // Down
        };
        List<Vector3Int> emptySpace = new List<Vector3Int>();
        _roomPositions = new List<Vector3Int> { l_spawnPoint };
        GenerateRoom(l_spawnPoint);

        for (int _ = 0; _ < m_maxRooms; _++)
        {
            emptySpace.Clear();
            foreach (Vector3Int vec3 in adjacentDirection)
            {
                Vector3Int l_newPosition = l_spawnPoint + vec3;
                if (!_roomPositions.Contains(l_newPosition))
                {
                    emptySpace.Add(vec3);
                }
            }
            if (emptySpace.Count == 0)
            {
                Debug.Log("No empty space found to place a new room.");
                break;
            }

            Vector3Int direction = emptySpace[Random.Range(0, emptySpace.Count)];
            l_spawnPoint += direction;
            GenerateRoom(l_spawnPoint);
            _roomPositions.Add(l_spawnPoint);
        }
    }
    
}
