using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int target1Count; 
        public int target2Count; 
        public int target3Count; 
        public int bossCount;    
        public float breakDuration; 
    }

    public List<Wave> waves = new List<Wave>(); 
    public Transform[] spawnPoints; 
    public GameObject target1Prefab; 
    public GameObject target2Prefab; 
    public GameObject target3Prefab;
    public GameObject bossPrefab; 
    public TextMeshProUGUI waveText; 
    public TextMeshProUGUI countdownText; 
    public float timeBetweenSpawns = 0.5f; 

    private int currentWaveIndex = 0;
    private bool isWaveActive = false;
    private int enemiesRemaining;

    void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        if (currentWaveIndex < waves.Count)
        {
            
            waveText.text = "Wave " + (currentWaveIndex + 1);
            waveText.gameObject.SetActive(true);
            yield return new WaitForSeconds(2f); 
            waveText.gameObject.SetActive(false);

            
            isWaveActive = true;
            StartCoroutine(SpawnWave(waves[currentWaveIndex]));
        }
        else
        {
            Debug.Log("All waves completed!\nBoss time!");
            waveText.text = "All Waves Complete!\n       Boss time!";
            waveText.gameObject.SetActive(true);
        }
    }

    IEnumerator SpawnWave(Wave wave)
    {
        
        enemiesRemaining = wave.target1Count + wave.target2Count + wave.target3Count;

       
        for (int i = 0; i < wave.target1Count; i++)
        {
            SpawnEnemy(target1Prefab);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        
        for (int i = 0; i < wave.target2Count; i++)
        {
            SpawnEnemy(target2Prefab);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

       
        for (int i = 0; i < wave.target3Count; i++)
        {
            SpawnEnemy(target3Prefab);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        
        while (enemiesRemaining > 0)
        {
            yield return null;
        }

       
        for (int i = 0; i < wave.bossCount; i++)
        {
            SpawnEnemy(bossPrefab);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

      
        while (enemiesRemaining > 0)
        {
            yield return null;
        }

        
        isWaveActive = false;
        Debug.Log("Wave " + (currentWaveIndex + 1) + " complete!");

       
        yield return StartCoroutine(ShowBreakCountdown(wave.breakDuration));

      
        currentWaveIndex++;
        StartCoroutine(StartNextWave());
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

       
        IDamageable damageable = enemy.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.OnDeath += EnemyDefeated;
        }
    }

    void EnemyDefeated()
    {
        enemiesRemaining--;
        Debug.Log("Enemy defeated. Remaining: " + enemiesRemaining);
    }

   
    IEnumerator ShowBreakCountdown(float breakDuration)
    {
        float countdownTime = breakDuration;
        while (countdownTime > 0)
        {
            countdownText.text = Mathf.Ceil(countdownTime).ToString(); 
            yield return new WaitForSeconds(1f); 
            countdownTime--; 
        }
        countdownText.text = ""; 
    }
}