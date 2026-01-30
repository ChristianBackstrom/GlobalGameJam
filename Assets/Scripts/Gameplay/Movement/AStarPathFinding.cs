using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AStarPathfinding : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private Tilemap obstacleTilemap; // Optional: separate tilemap for obstacles

    private class Node
    {
        public Vector3Int position;
        public Node parent;
        public float gCost; // Distance from start
        public float hCost; // Distance to end
        public float fCost => gCost + hCost;

        public Node(Vector3Int pos)
        {
            position = pos;
        }
    }

    public List<Vector3Int> FindPath(Vector3Int start, Vector3Int end)
    {
        List<Node> openList = new List<Node>();
        HashSet<Vector3Int> closedList = new HashSet<Vector3Int>();

        Node startNode = new Node(start);
        Node endNode = new Node(end);
        openList.Add(startNode);

        while (openList.Count > 0)
        {
            // Get node with lowest fCost
            Node currentNode = openList[0];
            for (int i = 1; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentNode.fCost ||
                    (openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost))
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode.position);

            // Path found
            if (currentNode.position == end)
            {
                return RetracePath(startNode, currentNode);
            }

            // Check neighbors
            foreach (Vector3Int neighborPos in GetNeighbors(currentNode.position))
            {
                if (!IsWalkable(neighborPos) || closedList.Contains(neighborPos))
                    continue;

                float newGCost = currentNode.gCost + GetDistance(currentNode.position, neighborPos);
                Node neighbor = openList.Find(n => n.position == neighborPos);

                if (neighbor == null)
                {
                    neighbor = new Node(neighborPos)
                    {
                        gCost = newGCost,
                        hCost = GetDistance(neighborPos, end),
                        parent = currentNode
                    };
                    openList.Add(neighbor);
                }
                else if (newGCost < neighbor.gCost)
                {
                    neighbor.gCost = newGCost;
                    neighbor.parent = currentNode;
                }
            }
        }

        return null; // No path found
    }

    private List<Vector3Int> GetNeighbors(Vector3Int pos)
    {
        List<Vector3Int> neighbors = new List<Vector3Int>
        {
            pos + Vector3Int.up,
            pos + Vector3Int.down,
            pos + Vector3Int.left,
            pos + Vector3Int.right
        };

        // Uncomment for diagonal movement
        // neighbors.Add(pos + new Vector3Int(1, 1, 0));
        // neighbors.Add(pos + new Vector3Int(1, -1, 0));
        // neighbors.Add(pos + new Vector3Int(-1, 1, 0));
        // neighbors.Add(pos + new Vector3Int(-1, -1, 0));

        return neighbors;
    }

    private bool IsWalkable(Vector3Int pos)
    {
        // Check if tile exists on main tilemap
        if (!tilemap.HasTile(pos))
            return false;

        // Check if there's an obstacle (if using obstacle tilemap)
        if (obstacleTilemap != null && obstacleTilemap.HasTile(pos))
            return false;

        return true;
    }

    private float GetDistance(Vector3Int a, Vector3Int b)
    {
        // Manhattan distance for 4-directional movement
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        // Uncomment for Euclidean distance (diagonal movement)
        // return Vector3Int.Distance(a, b);
    }

    private List<Vector3Int> RetracePath(Node startNode, Node endNode)
    {
        List<Vector3Int> path = new List<Vector3Int>();
        Node currentNode = endNode;

        while (currentNode != startNode)
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }

        path.Reverse();
        return path;
    }

    // Convert world position to cell position
    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        return tilemap.WorldToCell(worldPosition);
    }

    // Convert cell position to world position
    public Vector3 CellToWorld(Vector3Int cellPosition)
    {
        return tilemap.CellToWorld(cellPosition);
    }
}