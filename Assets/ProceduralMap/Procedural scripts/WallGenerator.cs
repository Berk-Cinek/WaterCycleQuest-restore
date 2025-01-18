using System;
using System.Collections.Generic;
using UnityEngine;

public class WallGenerator : MonoBehaviour
{
    public static (HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> cornerPositions) CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        HashSet<Vector2Int> cornerPositions = new HashSet<Vector2Int>();

        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);

        wallPositions.UnionWith(basicWallPositions);
        cornerPositions.UnionWith(cornerWallPositions);

        CreateExpandedWalls(tilemapVisualizer, basicWallPositions, floorPositions, 2);
        CreateExpandedCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions, 2);

        return (wallPositions, cornerPositions);
    }

    private static void CreateExpandedWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> floorPositions, int expansionSize)
    {
        foreach (var position in wallPositions)
        {
            string neighboursBinaryType = GetNeighboursBinaryType(position, floorPositions, Direction2D.cardinalDirectionsList);

            // Paint the initial wall
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);

            // Expand the wall by adding extra cells
            foreach (var direction in Direction2D.cardinalDirectionsList)
            {
                for (int i = 1; i < expansionSize; i++)
                {
                    var extendedPosition = position + direction * i;
                    if (!floorPositions.Contains(extendedPosition) && !wallPositions.Contains(extendedPosition))
                    {
                        tilemapVisualizer.PaintSingleBasicWall(extendedPosition, neighboursBinaryType);
                    }
                }
            }
        }
    }

    private static void CreateExpandedCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions, int expansionSize)
    {
        foreach (var position in cornerWallPositions)
        {
            string neighboursBinaryType = GetNeighboursBinaryType(position, floorPositions, Direction2D.eightDirectionsList);

            // Paint the initial corner wall
            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);

            // Expand the corner wall by adding extra cells
            foreach (var direction in Direction2D.eightDirectionsList)
            {
                for (int i = 1; i < expansionSize; i++)
                {
                    var extendedPosition = position + direction * i;
                    if (!floorPositions.Contains(extendedPosition) && !cornerWallPositions.Contains(extendedPosition))
                    {
                        tilemapVisualizer.PaintSingleCornerWall(extendedPosition, neighboursBinaryType);
                    }
                }
            }
        }
    }

    private static string GetNeighboursBinaryType(Vector2Int position, HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        string neighboursBinaryType = "";
        foreach (var direction in directionList)
        {
            var neighbourPosition = position + direction;
            neighboursBinaryType += floorPositions.Contains(neighbourPosition) ? "1" : "0";
        }
        return neighboursBinaryType;
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in directionList)
            {
                var neighbourPosition = position + direction;
                if (!floorPositions.Contains(neighbourPosition))
                {
                    wallPositions.Add(neighbourPosition);
                }
            }
        }
        return wallPositions;
    }
}

//public class WallGenerator : MonoBehaviour
//{
//    public static (HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> cornerPositions) CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
//    {
//        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
//        HashSet<Vector2Int> cornerPositions = new HashSet<Vector2Int>();

//        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
//        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);

//        wallPositions.UnionWith(basicWallPositions);
//        cornerPositions.UnionWith(cornerWallPositions);

//        CreateExpandedWalls(tilemapVisualizer, basicWallPositions, floorPositions);
//        CreateExpandedCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);

//        return (wallPositions, cornerPositions);
//    }

//    private static void CreateExpandedWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> floorPositions)
//    {
//        foreach (var position in wallPositions)
//        {
//            string neighboursBinaryType = GetNeighboursBinaryType(position, floorPositions, Direction2D.cardinalDirectionsList);

//            // Paint the initial wall
//            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);

//            // Expand the wall by adding an extra cell
//            foreach (var direction in Direction2D.cardinalDirectionsList)
//            {
//                var extendedPosition = position + direction;
//                if (!floorPositions.Contains(extendedPosition) && !wallPositions.Contains(extendedPosition))
//                {
//                    tilemapVisualizer.PaintSingleBasicWall(extendedPosition, neighboursBinaryType);
//                }
//            }
//        }
//    }

//    private static void CreateExpandedCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
//    {
//        foreach (var position in cornerWallPositions)
//        {
//            string neighboursBinaryType = GetNeighboursBinaryType(position, floorPositions, Direction2D.eightDirectionsList);

//            // Paint the initial corner wall
//            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);

//            // Expand the corner wall by adding an extra cell
//            foreach (var direction in Direction2D.eightDirectionsList)
//            {
//                var extendedPosition = position + direction;
//                if (!floorPositions.Contains(extendedPosition) && !cornerWallPositions.Contains(extendedPosition))
//                {
//                    tilemapVisualizer.PaintSingleCornerWall(extendedPosition, neighboursBinaryType);
//                }
//            }
//        }
//    }

//    private static string GetNeighboursBinaryType(Vector2Int position, HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
//    {
//        string neighboursBinaryType = "";
//        foreach (var direction in directionList)
//        {
//            var neighbourPosition = position + direction;
//            neighboursBinaryType += floorPositions.Contains(neighbourPosition) ? "1" : "0";
//        }
//        return neighboursBinaryType;
//    }

//    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
//    {
//        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
//        foreach (var position in floorPositions)
//        {
//            foreach (var direction in directionList)
//            {
//                var neighbourPosition = position + direction;
//                if (!floorPositions.Contains(neighbourPosition))
//                {
//                    wallPositions.Add(neighbourPosition);
//                }
//            }
//        }
//        return wallPositions;
//    }
//}

//public class WallGenerator : MonoBehaviour
//{
//    public static (HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> cornerPositions) CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
//    {
//        // Duvar ve köşe pozisyonlarını saklamak için setler
//        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
//        HashSet<Vector2Int> cornerPositions = new HashSet<Vector2Int>();

//        // Ana duvar ve köşe duvar pozisyonlarını hesapla
//        var basicWallPositions = FindWallsInDirections(floorPositions, Direction2D.cardinalDirectionsList);
//        var cornerWallPositions = FindWallsInDirections(floorPositions, Direction2D.diagonalDirectionsList);

//        // Duvar ve köşe pozisyonlarını setlere ekle
//        wallPositions.UnionWith(basicWallPositions);
//        cornerPositions.UnionWith(cornerWallPositions);

//        // Duvarları çiz
//        CreateBasicWall(tilemapVisualizer, basicWallPositions, floorPositions);
//        CreateCornerWalls(tilemapVisualizer, cornerWallPositions, floorPositions);

//        // Duvar ve köşe pozisyonlarını geri döndür
//        return (wallPositions, cornerPositions);
//    }

//    private static void CreateCornerWalls(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> cornerWallPositions, HashSet<Vector2Int> floorPositions)
//    {
//        foreach (var position in cornerWallPositions)
//        {
//            string neighboursBinaryType = "";
//            foreach (var direction in Direction2D.eightDirectionsList)
//            {
//                var neighbourPosition = position + direction;
//                neighboursBinaryType += floorPositions.Contains(neighbourPosition) ? "1" : "0";
//            }
//            tilemapVisualizer.PaintSingleCornerWall(position, neighboursBinaryType);
//        }
//    }

//    private static void CreateBasicWall(TilemapVisualizer tilemapVisualizer, HashSet<Vector2Int> basicWallPositions, HashSet<Vector2Int> floorPositions)
//    {
//        foreach (var position in basicWallPositions)
//        {
//            string neighboursBinaryType = "";
//            foreach (var direction in Direction2D.cardinalDirectionsList)
//            {
//                var neighbourPosition = position + direction;
//                neighboursBinaryType += floorPositions.Contains(neighbourPosition) ? "1" : "0";
//            }
//            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinaryType);
//        }
//    }

//    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
//    {
//        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
//        foreach (var position in floorPositions)
//        {
//            foreach (var direction in directionList)
//            {
//                var neighbourPosition = position + direction;
//                if (!floorPositions.Contains(neighbourPosition))
//                {
//                    wallPositions.Add(neighbourPosition);
//                }
//            }
//        }

//        return wallPositions;
//    }
//}
