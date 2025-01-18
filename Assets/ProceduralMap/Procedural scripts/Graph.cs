using System;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{
    private static List<Vector2Int> neighbourse4directions = new List<Vector2Int>
    {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0)
    };
    private static List<Vector2Int> neighbourse8directions = new List<Vector2Int>
    {
        new Vector2Int(0,1),
        new Vector2Int(1,0),
        new Vector2Int(0,-1),
        new Vector2Int(-1,0),
        new Vector2Int(1,1),
        new Vector2Int(1,-1),
        new Vector2Int(-1,1),
        new Vector2Int(-1,-1)
    };

    List<Vector2Int> graph;

    public Graph(IEnumerable<Vector2Int> vertices)
    {
        graph = new List<Vector2Int>(vertices);
    }

    public List<Vector2Int> GetNeighbours4Directions(Vector2Int startPositions)
    {
        return GetNeighbours(startPositions, neighbourse4directions);
    }

    public List<Vector2Int> GetNeighbours8Directions(Vector2Int startPositions)
    {
        return GetNeighbours(startPositions, neighbourse8directions);
    }

    private List<Vector2Int> GetNeighbours(Vector2Int startPositions, List<Vector2Int> neighboursOffsetList)
    {
        List<Vector2Int> neighbours = new List<Vector2Int>();
        foreach(var neighbourDirection in neighboursOffsetList)
        {
            Vector2Int potentialNeighbour = startPositions + neighbourDirection;
            if (graph.Contains(potentialNeighbour))
            {
                neighbours.Add(potentialNeighbour);
            }
        }
        return neighbours;
    }
}
