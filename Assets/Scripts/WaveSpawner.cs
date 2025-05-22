using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaveSpawner : MonoBehaviour
{
    public List<Wave> waves;
    public Transform spawnPoint;
    public GameObject youWinUI;

    private int currentWaveIndex = 0;
    private int enemiesAlive;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (currentWaveIndex < waves.Count)
        {
            Wave currentWave = waves[currentWaveIndex];
            enemiesAlive = currentWave.count;

            for (int i = 0; i < currentWave.count; i++)
            {
                GameObject enemyPrefab = currentWave.enemyPrefab;

                GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                IDamageable damageable = enemy.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.OnDeath += OnEnemyDeath;
                }
                else
                {
                    Debug.LogWarning("Enemy prefab does not implement IDamageable interface.");
                }

                yield return new WaitForSeconds(currentWave.spawnInterval);
            }

            // Așteaptă până când toți inamicii din valul curent au fost eliminați
            while (enemiesAlive > 0)
            {
                yield return null;
            }

            currentWaveIndex++;
        }

        Debug.Log("All waves completed. Loading next level...");
        LoadNextLevel();
    }

    void OnEnemyDeath()
    {
        enemiesAlive--;

        if (enemiesAlive <= 0 && currentWaveIndex < waves.Count)
        {
            if (currentWaveIndex == waves.Count - 1)
            {
                // Dacă am terminat toate valurile, putem afișa interfața de You Win
                Debug.Log("All enemies from the final wave have died.");
                ShowYouWinUI();
            }
            else
            {
                Debug.Log("All enemies from the current wave have died. Starting next wave...");
            }
        }
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No more levels in Build Settings. You win!");
            // Aici poți adăuga logica pentru a afișa ecranul de You Win sau a face alte acțiuni necesare pentru câștigarea jocului.
        }
    }

    void ShowYouWinUI()
    {
        if (youWinUI != null)
        {
            youWinUI.SetActive(true);
        }
        else
        {
            Debug.LogWarning("You Win UI container not assigned.");
        }
    }
}

[System.Serializable]
public class Wave
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnInterval;
}