using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private SpriteRenderer areaSprite;
    [SerializeField] private float spacing = 2f;
    [SerializeField] private float noiseScale = 1f;
    [SerializeField] private float spawnThreshold = 0.5f;
    [SerializeField] private int maxEnemyCount = 10;
    [SerializeField] private float minDistance = 1.5f;
    [SerializeField] private TextMeshProUGUI enemyCountText;

    private float noiseOffset = 0f;
    private int currentEnemyCount = 0;
    private int spawned;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnEnemiesWithPerlinNoise), 0, 2);
        UpdateEnemyCountText();
        spawned = 0;
    }

    public void OnEnemyDestroyed()
    {
        currentEnemyCount = Mathf.Max(0, currentEnemyCount - 1);
        UpdateEnemyCountText();
    }

    private void SpawnEnemiesWithPerlinNoise()
    {
        if (currentEnemyCount >= maxEnemyCount)
            return;
        var bounds = areaSprite.bounds;
        Vector2 min = bounds.min;
        Vector2 max = bounds.max;
        List<Vector3> spawnedPositions = new List<Vector3>();
        
        for (float x = min.x; x < max.x; x += spacing)
        {
            for (float y = min.y; y < max.y; y += spacing)
            {
                if (currentEnemyCount + spawned >= maxEnemyCount)
                    break; // Only break inner loop
                float u = Mathf.InverseLerp(min.x, max.x, x) * noiseScale + noiseOffset;
                float v = Mathf.InverseLerp(min.y, max.y, y) * noiseScale + noiseOffset;
                float noise = Mathf.PerlinNoise(u, v);
                if (noise > spawnThreshold)
                {
                    Vector3 position = new Vector3(x, y, 0);
                    bool tooClose = false;
                    foreach (var pos in spawnedPositions)
                    {
                        if (Vector3.Distance(pos, position) < minDistance)
                        {
                            tooClose = true;
                            break;
                        }
                    }
                    if (tooClose)
                        continue; 
                    spawned++;
                    Debug.Log($"Spawning enemy at {position} with noise {noise}");
                    var enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
                    var enemyComponent = enemy.GetComponent<EnemySpawner_Enemy>();
                    if (enemyComponent == null)
                        enemyComponent = enemy.AddComponent<EnemySpawner_Enemy>();
                    enemyComponent.spawner = this;
                    spawnedPositions.Add(position);
                }
            }
            if (currentEnemyCount + spawned >= maxEnemyCount)
                break; // Break outer loop if needed
        } 
        Debug.Log($"your going here");
        currentEnemyCount += spawned;
        noiseOffset += 0.5f;
        UpdateEnemyCountText();
    }

    private void UpdateEnemyCountText()
    {
        if (enemyCountText != null)
            enemyCountText.text = $"Enemies: {currentEnemyCount}/{maxEnemyCount}";
    }
}
