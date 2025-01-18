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
    [SerializeField]
    private int lastWave = 5;

    private int currentWave = 1; // Şu anki dalga sayısı
    private int enemiesToSpawn; // O dalgada yaratılacak düşman sayısı
    private bool isWaveActive = false;

    private float waveStartTime; // Dalga başladığındaki zaman
    private float totalElapsedTime; // Toplam geçen süre

    private bool bossCreated = false;

    void Start()
    {
        if (placePrefab == null)
        {
            Debug.LogError("PlacePrefab referansı eksik!");
            return;
        }

        totalElapsedTime = 0f; // Toplam süre başlangıçta sıfır
        StartCoroutine(StartNextWave());
    }

    void Update()
    {
        if (currentWave <= lastWave)
        {
            // Eğer dalga aktifse ve tüm düşmanlar öldüyse, dalga tamamlandı
            if (isWaveActive && AreAllEnemiesDead())
            {
                isWaveActive = false;

                // Dalga süresini hesapla
                float waveDuration = Time.time - waveStartTime;
                totalElapsedTime += waveDuration;

                Debug.Log($"Wave {currentWave - 1} completed in {waveDuration:F2} seconds. Total elapsed time: {totalElapsedTime:F2} seconds.");

                // Bir sonraki dalgayı başlat
                if (currentWave < lastWave)
                {
                    StartCoroutine(StartNextWave());
                }
                else if (!bossCreated)
                {
                    CreateBoss();
                }
            }
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
        waveStartTime = Time.time; // Dalganın başlangıç zamanını kaydet
        StartEnemyWave();

        // Dalga sayısını artır
        currentWave++;
    }

    private void CreateBoss()
    {
        if (placePrefab == null)
        {
            Debug.LogError("PlacePrefab referansı eksik!");
            return;
        }

        placePrefab.CreateBoss();
        Debug.Log("Boss create method");
        bossCreated = true;
    }
}