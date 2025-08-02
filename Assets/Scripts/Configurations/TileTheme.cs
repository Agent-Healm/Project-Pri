using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileTheme : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private int m_maxFPS;

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

    [Header("Experimental room groups")]
    [SerializeField] private RoomFrequency[] m_roomFrequencies;
    public RoomFrequency[] GetRoomFrequency
    {
        get => m_roomFrequencies;
    }

    [SerializeField] private RoomFrequency[] m_roomMobVariant;
    public RoomFrequency[] GetRoomMobVariant
    {
        get => m_roomMobVariant;
    }

    private void Awake()
    {
        Application.targetFrameRate = m_maxFPS;
    }
}

[Serializable]
public class RoomFrequency
{
    [SerializeField] public GameObject roomPrefab;
    [SerializeField] public int quantity;
}