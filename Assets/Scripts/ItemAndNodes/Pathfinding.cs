using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Pathfinding : MonoBehaviour
{
    [HideInInspector]
    public enum Priorites
    {
        Horizontal,
        Vertical,
        Random,
    }
    public Tilemap groundTiles;
    public Tilemap wallTiles;
    public Priorites priority = Priorites.Horizontal;
    private Vector3Int _currentTile;
    // Just horizontal
    private readonly List<Vector2> _CARDINAL_DIR_HR = new List<Vector2> { Vector2.left, Vector2.right, Vector2.up, Vector2.down };
    // Just vertical 
    private readonly List<Vector2> _CARDINAL_DIR_VR = new List<Vector2> { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
    private List<Vector3Int> _getPath(Transform target)
    {
        // Functional programming rocks.. 
        float _heuristic(Vector3Int a, Vector3Int b) => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        Vector3Int start = _currentTile;
        Vector3Int goal = groundTiles.WorldToCell(target.position);
        Debug.Log(goal);
        List<Vector2> dirList = (priority == Priorites.Horizontal) ? _CARDINAL_DIR_HR : _CARDINAL_DIR_VR;
        if (priority == Priorites.Random)
        {
            dirList.Shuffle();
        }
        // Open list of nodes to evaluate
        List<Vector3Int> openSet = new List<Vector3Int> { start };
        HashSet<Vector3Int> closedSet = new HashSet<Vector3Int>();
        Dictionary<Vector3Int, Vector3Int> cameFrom = new Dictionary<Vector3Int, Vector3Int>();
        Dictionary<Vector3Int, float> gScore = new Dictionary<Vector3Int, float> { [start] = 0 };
        Dictionary<Vector3Int, float> fScore = new Dictionary<Vector3Int, float>
        { [start] = _heuristic(start, goal) };

        while (openSet.Count > 0)
        {
            // Get the tile with the lowest fScore (best candidate)
            Vector3Int current = _getLowestFScore(openSet, fScore);

            // If we have reached the goal, reconstruct the path
            if (current == goal)
            {
                return _reconstructPath(cameFrom, current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            // Explore neighbors
            foreach (Vector2 direction in dirList)
            {
                Vector3Int neighbor = current + new Vector3Int((int)direction.x, (int)direction.y, 0);
                if (closedSet.Contains(neighbor) || !canMove(neighbor))
                {
                    continue;
                }

                float tentativeGScore = gScore[current] + 1; // All moves have equal cost FOR NOW

                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + _heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                    {
                        openSet.Add(neighbor);
                    }
                }
            }
        }
        Debug.Log("No Valid path");
        // Return an empty path if no valid path found
        return new List<Vector3Int>();
    }
    // This could be optimized with a PQ, not making one.. Might add this as issue for Week 4
    private Vector3Int _getLowestFScore(List<Vector3Int> openSet, Dictionary<Vector3Int, float> fScore)
    {
        Vector3Int bestNode = openSet[0];
        float bestScore = fScore[bestNode];

        foreach (Vector3Int node in openSet)
        {
            if (fScore[node] < bestScore)
            {
                bestNode = node;
                bestScore = fScore[node];
            }
        }

        return bestNode;
    }
    private List<Vector3Int> _reconstructPath(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
    {
        List<Vector3Int> totalPath = new List<Vector3Int> { current };

        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            totalPath.Insert(0, current);
        }

        return totalPath;
    }

    public bool canMove(Vector3Int gridPos)
    {
        // Lets check if we can move.. 
        if (!groundTiles.HasTile(gridPos) || wallTiles.HasTile(gridPos))
        {
            return false;
        }
        return true;
    }
}
