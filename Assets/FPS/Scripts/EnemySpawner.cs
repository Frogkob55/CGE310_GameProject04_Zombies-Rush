using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public class EnemySpawnData
{
    public GameObject enemyPrefab;
    public int maxCount;
}

[System.Serializable]
public class WaveData
{
    public List<EnemySpawnData> enemies;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Wave Settings")]
    public List<WaveData> waveEnemies;
    public Transform[] spawnPoints;
    public int initialEnemiesPerWave = 5;
    public float timeBetweenWaves = 5f;

    [Header("UI Elements")]
    public Text waveText; // 🆕 เพิ่ม UI แสดง Wave
    public Text winText;

    private int currentWave = 0;
    private List<GameObject> activeEnemies = new List<GameObject>();
    private Dictionary<GameObject, int> enemyCountMap = new Dictionary<GameObject, int>();

    private void Start()
    {
        winText.gameObject.SetActive(false);
        UpdateWaveText(); // 🆕 แสดงข้อความ Wave แรก
        StartCoroutine(StartWave());
    }

    private IEnumerator StartWave()
    {
        while (currentWave < waveEnemies.Count)
        {
            currentWave++;
            UpdateWaveText(); // 🆕 อัปเดต UI แสดง Wave

            int enemiesThisWave = initialEnemiesPerWave + (currentWave * 2);
            Debug.Log($"Starting Wave {currentWave}, Enemies: {enemiesThisWave}");

            enemyCountMap.Clear();

            for (int i = 0; i < enemiesThisWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(Random.Range(0.5f, 2f));
            }

            while (activeEnemies.Count > 0)
            {
                yield return null;
            }

            yield return new WaitForSeconds(timeBetweenWaves);
        }

        WinGame();
    }

    private void SpawnEnemy()
    {
        if (spawnPoints.Length == 0 || waveEnemies[currentWave - 1].enemies.Count == 0)
            return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        List<EnemySpawnData> availableEnemies = new List<EnemySpawnData>();

        foreach (var enemyData in waveEnemies[currentWave - 1].enemies)
        {
            int currentCount = enemyCountMap.ContainsKey(enemyData.enemyPrefab) ? enemyCountMap[enemyData.enemyPrefab] : 0;
            if (currentCount < enemyData.maxCount)
            {
                availableEnemies.Add(enemyData);
            }
        }

        if (availableEnemies.Count == 0)
            return;

        EnemySpawnData selectedEnemyData = availableEnemies[Random.Range(0, availableEnemies.Count)];

        GameObject newEnemy = Instantiate(selectedEnemyData.enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        activeEnemies.Add(newEnemy);

        if (!enemyCountMap.ContainsKey(selectedEnemyData.enemyPrefab))
        {
            enemyCountMap[selectedEnemyData.enemyPrefab] = 0;
        }
        enemyCountMap[selectedEnemyData.enemyPrefab]++;

        newEnemy.GetComponent<EnemyAI>().SetSpawner(this);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (activeEnemies.Contains(enemy))
        {
            activeEnemies.Remove(enemy);
        }
    }

    private void WinGame()
    {
        Debug.Log("You Win!");
        winText.gameObject.SetActive(true);
        waveText.text = "All Waves Cleared!"; // 🆕 เปลี่ยนข้อความ Wave เป็นชนะ
        Invoke("LoadMenuScene", 5f);
    }

    private void LoadMenuScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuScene");
    }

    private void UpdateWaveText()
    {
        if (waveText != null)
        {
            waveText.text = $"Wave {currentWave}/{waveEnemies.Count}";
        }
    }
}
