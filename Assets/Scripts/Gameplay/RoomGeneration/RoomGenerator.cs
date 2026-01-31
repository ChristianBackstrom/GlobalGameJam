using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomGenerator : MonoBehaviour
{
    [Header("Room Settings")]
    [SerializeField] private int roomWidth;
    [SerializeField] private int roomHeight;

    [Header("Tilemap References")]
    [SerializeField] private Tilemap floorTilemap;
    [SerializeField] private Tilemap wallTilemap;
    [SerializeField] private Tilemap obstacleTilemap;


    [Header("Tile References")]
    [SerializeField] private TileBase floorTile;
    [SerializeField] private WallDirections[] wallTiles;
    [SerializeField] private TileBase[] obstacleTiles;

    [Serializable]
    internal struct WallDirections
    {
        public WallDirection direction;
        public TileBase tile;
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
            TileBase obstacleTile = obstacleTiles[rand.Next(obstacleTiles.Length)];
            obstacleTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(x + offsetX), Mathf.RoundToInt(y + offsetY), 0), obstacleTile);
        }
    }

    private void GenerateWalls()
    {
        foreach (var wall in wallTiles)
        {
            switch (wall.direction)
            {
                case WallDirection.NORTH:
                    for (int x = 0; x < roomWidth; x++)
                    {
                        wallTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(x + offsetX), Mathf.RoundToInt(roomHeight + offsetY), 0), wall.tile);
                    }
                    break;
                case WallDirection.EAST:
                    for (int y = 0; y < roomHeight; y++)
                    {
                        wallTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(roomWidth + offsetX), Mathf.RoundToInt(y + offsetY), 0), wall.tile);
                    }
                    break;
                case WallDirection.SOUTH:
                    for (int x = 0; x < roomWidth; x++)
                    {
                        wallTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(x + offsetX), Mathf.RoundToInt(-1 + offsetY), 0), wall.tile);
                    }
                    break;
                case WallDirection.WEST:
                    for (int y = 0; y < roomHeight; y++)
                    {
                        wallTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(-1 + offsetX), Mathf.RoundToInt(y + offsetY), 0), wall.tile);
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
                floorTilemap.SetTile(new Vector3Int(Mathf.RoundToInt(x + offsetX), Mathf.RoundToInt(y + offsetY), 0), floorTile);
            }
        }
    }
}
