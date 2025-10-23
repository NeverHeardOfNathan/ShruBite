using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject zombiePrefab;
    public float spawnRadius = 15f;
    public float initialSpawnDelay = 2f;
    public float spawnInterval = 2f;
    public int zombiesPerWave = 5;

    [Header("Dynamic")]
    public int currentScore = 0;
    public int zombiesSpawned = 0;

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Start background music
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic();
        }

        InvokeRepeating("SpawnZombie", initialSpawnDelay, spawnInterval);
    }

    void SpawnZombie()
    {
        if (playerTransform == null) return;

        // Spawn at random position around edge of play area
        Vector2 randomCircle = Random.insideUnitCircle.normalized * spawnRadius;
        Vector3 spawnPos = new Vector3(randomCircle.x, 0, randomCircle.y);

        Instantiate(zombiePrefab, spawnPos, Quaternion.identity);
        zombiesSpawned++;

        // Gradually increase difficulty
        if (zombiesSpawned % 15 == 0)
        {
            spawnInterval = Mathf.Max(0.1f, spawnInterval * 0.8f);
            CancelInvoke("SpawnZombie");
            InvokeRepeating("SpawnZombie", spawnInterval, spawnInterval);
        }
    }

    public void AddScore(int points)
    {
        currentScore += points;
    }
}

