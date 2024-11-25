using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CorridorFirstMapGenerator : SimpleRandomWalkMapGenerator
{
    [SerializeField]
    private int corridorLength = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)]
    private float roomPercent = 0.8f;

    [SerializeField]
    private PrefabGenerator prefabGenerator;

    private Vector2Int lastMapDimensions;

    private void StoreMapDimensions(HashSet<Vector2Int> floorPositions)
    {
        lastMapDimensions = CalculateMapBounds(floorPositions);
    }

    public Vector2Int GetMapDimensions()
    {
        return lastMapDimensions;
    }

    protected override void RunProceduralGeneration()
    {
        CorridorFirstGeneration();
    }
    
    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPositions = new HashSet<Vector2Int>();


        prefabGenerator.GetPosition(floorPositions);
        //prefabGenerator.PlacePrefabs(floorPositions);

        List<List<Vector2Int>> corridors = CreateCorridors(floorPositions, potentialRoomPositions);

        HashSet<Vector2Int> roomPositions = CreateRooms(potentialRoomPositions);

        List<Vector2Int> deadEnds = FindAllDeadEnds(floorPositions);

        CreateRoomsAtDeadEnd(deadEnds, roomPositions);

        floorPositions.UnionWith(roomPositions);

        foreach (var corridor in corridors)
        {
            floorPositions.UnionWith(IncreaseCorridorSizeTo2x2(corridor));
        }

        // Calculate map bounds
        Vector2Int mapDimensions = CalculateMapBounds(floorPositions);

        // Store the map dimensions for later use
        StoreMapDimensions(floorPositions);

        Debug.Log($"Map Dimensions: Width = {mapDimensions.x}, Height = {mapDimensions.y}");

        tilemapVisualizer.PaintFloorTiles(floorPositions);
        WallGenerator.CreateWalls(floorPositions, tilemapVisualizer);
    }

    private List<Vector2Int> IncreaseCorridorSizeTo2x2(List<Vector2Int> corridor)
    {
        List<Vector2Int> newCorridor = new List<Vector2Int>();

        foreach (var position in corridor)
        {
            for (int x = 0; x < 2; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    Vector2Int newTile = position + new Vector2Int(x, y);
                    if (!newCorridor.Contains(newTile))
                    {
                        newCorridor.Add(newTile);
                    }
                }
            }
        }

        return newCorridor;
    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomFloors)
    {
        foreach (var position in deadEnds)
        {
            if (!roomFloors.Contains(position))
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        roomFloors.Add(position + new Vector2Int(x, y));
                    }
                }
            }
        }
    }

    private List<Vector2Int> FindAllDeadEnds(HashSet<Vector2Int> floorPositions)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach (var position in floorPositions)
        {
            int neighboursCount = 0;
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                if (floorPositions.Contains(position + direction))
                {
                    neighboursCount++;
                }
            }
            if (neighboursCount == 1)
            {
                deadEnds.Add(position);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPositions)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomToCreateCount = Mathf.RoundToInt(potentialRoomPositions.Count * roomPercent);

        List<Vector2Int> roomsToCreate = potentialRoomPositions.OrderBy(x => Guid.NewGuid()).Take(roomToCreateCount).ToList();

        foreach (var roomPosition in roomsToCreate)
        {
            var roomFloor = RunRandomWalk(randomWalkParameters, roomPosition);
            foreach (var position in roomFloor)
            {
                for (int x = 0; x < 2; x++)
                {
                    for (int y = 0; y < 2; y++)
                    {
                        roomPositions.Add(position + new Vector2Int(x, y));
                    }
                }
            }
        }

        return roomPositions;
    }

    private List<List<Vector2Int>> CreateCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPositions)
    {
        var currentPosition = startPosition;
        potentialRoomPositions.Add(currentPosition);

        List<List<Vector2Int>> corridors = new List<List<Vector2Int>>();
        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(currentPosition, corridorLength);
            corridor = IncreaseCorridorSizeTo2x2(corridor);

            foreach (var tile in corridor)
            {
                floorPositions.Add(tile);
            }

            corridors.Add(corridor);
            currentPosition = corridor[corridor.Count - 1];
            potentialRoomPositions.Add(currentPosition);
        }
        return corridors;
    }
    private Vector2Int CalculateMapBounds(HashSet<Vector2Int> floorPositions)
    {
        if (floorPositions == null || floorPositions.Count == 0)
        {
            Debug.LogError("No floor positions available to calculate bounds.");
            return Vector2Int.zero;
        }

        int minX = int.MaxValue, maxX = int.MinValue;
        int minY = int.MaxValue, maxY = int.MinValue;

        foreach (Vector2Int position in floorPositions)
        {
            if (position.x < minX) minX = position.x;
            if (position.x > maxX) maxX = position.x;
            if (position.y < minY) minY = position.y;
            if (position.y > maxY) maxY = position.y;
        }

        // Map width = maxX - minX + 1, Map height = maxY - minY + 1
        return new Vector2Int(maxX - minX + 1, maxY - minY + 1);
    }
    public Vector3 GetMapOrigin()
    {
        return new Vector3(startPosition.x, startPosition.y, 0); // Assuming startPosition is a Vector2Int
    }

}
