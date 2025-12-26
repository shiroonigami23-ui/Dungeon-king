using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Generator Settings")]
    public GameObject floorPrefab;
    public GameObject wallPrefab;
    public int totalTiles = 100;

    private List<Vector2> floorPositions = new List<Vector2>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        Vector2 currentPos = Vector2.zero;

        for (int i = 0; i < totalTiles; i++)
        {
            floorPositions.Add(currentPos);
            Instantiate(floorPrefab, currentPos, Quaternion.identity, transform);

            // Move in a random direction (Up, Down, Left, Right)
            int dir = Random.Range(0, 4);
            if (dir == 0) currentPos += Vector2.up;
            else if (dir == 1) currentPos += Vector2.down;
            else if (dir == 2) currentPos += Vector2.left;
            else if (dir == 3) currentPos += Vector2.right;
        }

        SpawnWalls();
    }

    void SpawnWalls()
    {
        // Simple logic: If a floor tile doesn't have a neighbor, put a wall there
        foreach (Vector2 pos in floorPositions)
        {
            Vector2[] neighbors = { pos + Vector2.up, pos + Vector2.down, pos + Vector2.left, pos + Vector2.right };
            foreach (Vector2 neighbor in neighbors)
            {
                if (!floorPositions.Contains(neighbor))
                {
                    Instantiate(wallPrefab, neighbor, Quaternion.identity, transform);
                }
            }
        }
    }
}