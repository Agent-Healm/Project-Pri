using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTheme : MonoBehaviour
{
    [Header("Base Tile")]
    [SerializeField] private TileBase m_tileFloor;
    public TileBase FloorTile
    {
        get => m_tileFloor;
    }
    [SerializeField] private TileBase m_tileWall;
    public TileBase WallTile
    {
        get => m_tileWall;
    }
    [SerializeField] private TileBase m_tileGate;
    public TileBase GateTile
    {
        get => m_tileGate;
    }

    [Header("Room Templates")]
    [SerializeField] private GameObject m_roomHome;
    public GameObject HomeRoom
    {
        get => m_roomHome;
    }
    [SerializeField] private GameObject m_roomMob;
    public GameObject MobRoom
    {
        get => m_roomMob;
    }
    [SerializeField] private GameObject m_roomExit;
    public GameObject ExitRoom
    {
        get => m_roomExit;
    }
    [SerializeField] private GameObject m_roomSpecial;
    public GameObject SpecialRoom
    {
        get => m_roomSpecial;
    }
}
