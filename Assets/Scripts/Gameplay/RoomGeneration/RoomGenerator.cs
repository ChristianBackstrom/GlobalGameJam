using System;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] private int roomWidth;
    [SerializeField] private int roomHeight;

    [Header("Prefab References")]
    [SerializeField] private GameObject floorPrefab;
    [SerializeField] private WallPrefab[] wallPrefabs;
    [SerializeField] private GameObject[] obstaclePrefabs;
    [Header("Optional: Parent Objects for Organization")]
    [SerializeField] private Transform floorParent;
    [SerializeField] private Transform wallParent;
    [SerializeField] private Transform obstacleParent;

    [Serializable]
    internal struct WallPrefab
    {
        public WallDirection direction;
        public GameObject prefab;
    }

    internal enum WallDirection
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

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
        GenerateWalls();
        PlaceObstacles();
    }

    private void PlaceObstacles()
    {
        System.Random rand = new System.Random();
        int obstacleCount = (roomWidth * roomHeight) / 10; // Example: 10% of the room area

        for (int i = 0; i < obstacleCount; i++)
        {
            int x = rand.Next(0, roomWidth);
            int y = rand.Next(0, roomHeight);
            GameObject obstaclePrefab = obstaclePrefabs[rand.Next(obstaclePrefabs.Length)];
            Vector3 pos = new Vector3(x + offsetX, y + offsetY, 0);
            Instantiate(obstaclePrefab, pos, Quaternion.identity, obstacleParent);
        }
    }

    private void GenerateWalls()
    {
        foreach (var wall in wallPrefabs)
        {
            switch (wall.direction)
            {
                case WallDirection.NORTH:
                    for (int x = 0; x < roomWidth; x++)
                    {
                        Vector3 pos = new Vector3(x + offsetX, roomHeight + offsetY, 0);
                        Instantiate(wall.prefab, pos, Quaternion.identity, wallParent);
                    }
                    break;
                case WallDirection.EAST:
                    for (int y = 0; y < roomHeight; y++)
                    {
                        Vector3 pos = new Vector3(roomWidth + offsetX, y + offsetY, 0);
                        Instantiate(wall.prefab, pos, Quaternion.identity, wallParent);
                    }
                    break;
                case WallDirection.SOUTH:
                    for (int x = 0; x < roomWidth; x++)
                    {
                        Vector3 pos = new Vector3(x + offsetX, -1 + offsetY, 0);
                        Instantiate(wall.prefab, pos, Quaternion.identity, wallParent);
                    }
                    break;
                case WallDirection.WEST:
                    for (int y = 0; y < roomHeight; y++)
                    {
                        Vector3 pos = new Vector3(-1 + offsetX, y + offsetY, 0);
                        Instantiate(wall.prefab, pos, Quaternion.identity, wallParent);
                    }
                    break;
            }
        }
    }

    private void GenerateFloor()
    {
        for (int x = 0; x < roomWidth; x++)
        {
            for (int y = 0; y < roomHeight; y++)
            {
                Vector3 pos = new Vector3(x + offsetX, y + offsetY, 0);
                Instantiate(floorPrefab, pos, Quaternion.identity, floorParent);
            }
        }
    }
}
