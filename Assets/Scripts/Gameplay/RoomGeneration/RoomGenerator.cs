using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] private int roomWidth;
    [SerializeField] private int roomHeight;

    [Header("Prefab References")]
    [SerializeField] private GameObject[] obstaclePrefabs;
    [Header("Optional: Parent Objects for Organization")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private RuleTile floorTile;

    private void Start()
    {
        GenerateRoom();
    }

    private float offsetX, offsetY;

    private void GenerateRoom()
    {
        offsetX = -roomWidth / 2f;
        offsetY = -roomHeight / 2f;
        GenerateFloor();
        // GenerateWalls();
        PlaceObstacles();
    }

    private void PlaceObstacles()
    {
        // System.Random rand = new System.Random();
        // int obstacleCount = (roomWidth * roomHeight) / 10; // Example: 10% of the room area

        // for (int i = 0; i < obstacleCount; i++)
        // {
        //     int x = rand.Next(0, roomWidth);
        //     int y = rand.Next(0, roomHeight);
        //     GameObject obstaclePrefab = obstaclePrefabs[rand.Next(obstaclePrefabs.Length)];
        //     Vector3 pos = new Vector3(x + offsetX, y + offsetY, 0);
        //     Instantiate(obstaclePrefab, pos, Quaternion.identity, obstacleParent);
        // }
    }

    private void GenerateFloor()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                Vector3Int tilePos = new Vector3Int(x + Mathf.FloorToInt(offsetX), y + Mathf.FloorToInt(offsetY), 0);
                tilemap.SetTile(tilePos, floorTile);
            }
        }
    }
}
