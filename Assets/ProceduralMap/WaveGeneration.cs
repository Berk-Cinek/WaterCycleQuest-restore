using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveGenerator : MonoBehaviour
{
    [SerializeField]
    private PrefabGenerator placePrefab; // PlacePrefab scriptine referans
    [SerializeField]
    private float timeBetweenWaves = 5f; // Dalgalar arası bekleme süresi
    [SerializeField]
    private int initialEnemyCount = 3; // İlk dalga düşman sayısı
    [SerializeField]
    private int enemyIncrementPerWave = 2; // Her dalgada artan düşman sayısı
    [SerializeField]
    private string enemyTag = "Enemy"; // Düşman GameObject'lerinin etiketi

    private int currentWave = 1; // Şu anki dalga sayısı
    private int enemiesToSpawn; // O dalgada yaratılacak düşman sayısı
    private bool isWaveActive = false;

    void Start()
    {
        if (placePrefab == null)
        {
            Debug.LogError("PlacePrefab referansı eksik!");
            return;
        }

        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (isWaveActive && AreAllEnemiesDead())
        {
            isWaveActive = false;
            StartCoroutine(StartNextWave());
        }
    }

    private bool AreAllEnemiesDead()
    {
        // Sahnedeki tüm düşmanları kontrol et
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);

        // Eğer düşman yoksa true döner
        return enemies.Length == 0;
    }

    public void StartEnemyWave()
    {
        if (placePrefab == null)
        {
            Debug.LogError("PlacePrefab referansı eksik!");
            return;
        }

        // PlacePrefab içindeki SpawnEnemy fonksiyonunu çağır
        placePrefab.SpawnEnemy(enemiesToSpawn);

        Debug.Log($"Wave {currentWave}: {enemiesToSpawn} düşman yaratıldı.");
    }

    IEnumerator StartNextWave()
    {
        // Dalga başlatılmadan önce bekleme
        yield return new WaitForSeconds(timeBetweenWaves);

        Debug.Log($"Starting Wave {currentWave}");

        // Dalga ayarları
        enemiesToSpawn = initialEnemyCount + (currentWave - 1) * enemyIncrementPerWave;

        // Yeni dalgayı başlat
        isWaveActive = true;
        StartEnemyWave();

        // Dalga tamamlandıktan sonra sonraki dalgaya geçiş için dalga sayısını arttır
        currentWave++;
    }
}
