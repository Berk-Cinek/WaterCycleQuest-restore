using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PrefabGenerator : MonoBehaviour
{
    [SerializeField]
    private TilemapVisualizer tilemapVisualizer;
    [SerializeField]
    private int coinNumber = 10;
    [SerializeField]
    private GameObject coin;
    [SerializeField]
    private GameObject playerPrefab;
    [SerializeField]
    private float minDistanceFromWalls = 2.0f; // Duvarlardan minimum uzaklık
    [SerializeField]
    private float minDistanceFromCorners = 2.0f; // Köşelerden minimum uzaklık

    private Vector2 center;

    public void PlacePrefabs(HashSet<Vector2Int> positions)
    {
        // Duvar ve köşe pozisyonlarını al
        var (wallPositions, cornerPositions) = WallGenerator.CreateWalls(positions, tilemapVisualizer);

        // Pozisyonları listeye dönüştür
        List<Vector2Int> positionList = new List<Vector2Int>(positions);

        // Eğer pozisyon yoksa işlem yapma
        if (positionList.Count == 0) return;

        // Alanın merkezini hesapla
        center = CalculateCenter(positions);

        // Geçerli pozisyonları ve ağırlıkları hesapla
        PrecomputeValidPositions(positionList, wallPositions, cornerPositions);

        // Prefab yerleştir
        for (int i = 0; i < coinNumber; i++)
        {
            Vector2Int randomPosition = GetWeightedRandomPosition();

            if (randomPosition != Vector2Int.zero)
            {
                // Prefab oluştur
                Instantiate(coin, new Vector3(randomPosition.x, randomPosition.y, -1), Quaternion.identity);
            }
        }
    }

    public GameObject PlacePlayer()
    {
        // Oyuncuyu merkeze yerleştir
        GameObject _player = Instantiate(playerPrefab, new Vector3(center.x, center.y, -1), Quaternion.identity);
        return _player;
    }

    private Vector2 CalculateCenter(IEnumerable<Vector2Int> positions)
    {
        float sumX = 0;
        float sumY = 0;
        int count = 0;

        foreach (var position in positions)
        {
            sumX += position.x;
            sumY += position.y;
            count++;
        }

        return count == 0 ? Vector2.zero : new Vector2(sumX / count, sumY / count);
    }

    private List<Vector2Int> validPositions;
    private List<float> positionWeights;

    private void PrecomputeValidPositions(List<Vector2Int> positionList, HashSet<Vector2Int> wallPositions, HashSet<Vector2Int> cornerPositions)
    {
        validPositions = new List<Vector2Int>();
        positionWeights = new List<float>();

        foreach (var position in positionList)
        {
            if (IsFarEnoughFromWalls(position, wallPositions) && IsFarEnoughFromCorners(position, cornerPositions))
            {
                float distanceFromCenter = Vector2.Distance(center, position);

                // Uzaklık arttıkça ağırlık artar
                float weight = Mathf.Pow(distanceFromCenter, 2);
                validPositions.Add(position);
                positionWeights.Add(weight);
            }
        }
    }

    private Vector2Int GetWeightedRandomPosition()
    {
        if (validPositions.Count == 0) return Vector2Int.zero;

        // Ağırlığa dayalı rastgele seçim
        float totalWeight = 0;
        foreach (var weight in positionWeights) totalWeight += weight;

        float randomValue = Random.value * totalWeight;

        for (int i = 0; i < validPositions.Count; i++)
        {
            if (randomValue < positionWeights[i])
            {
                return validPositions[i];
            }
            randomValue -= positionWeights[i];
        }

        return validPositions[validPositions.Count - 1];
    }

    private bool IsFarEnoughFromWalls(Vector2Int position, HashSet<Vector2Int> wallPositions)
    {
        foreach (var wallPosition in wallPositions)
        {
            if (Vector2.Distance(position, wallPosition) < minDistanceFromWalls)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsFarEnoughFromCorners(Vector2Int position, HashSet<Vector2Int> cornerPositions)
    {
        foreach (var cornerPosition in cornerPositions)
        {
            if (Vector2.Distance(position, cornerPosition) < minDistanceFromCorners)
            {
                return false;
            }
        }
        return true;
    }
}