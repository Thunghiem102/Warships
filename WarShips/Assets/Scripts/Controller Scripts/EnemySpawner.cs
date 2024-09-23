using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Tham chiếu tới prefab của enemy
    public float spawnInterval = 2.0f; // Khoảng thời gian giữa các lần spawn
    public float spawnRangeX = 5.0f; // Khoảng cách spawn theo trục X

    private void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    private IEnumerator SpawnEnemy()
    {
        while (true)
        {
            float randomX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPosition = new Vector3(randomX, transform.position.y, transform.position.z);
            

            Instantiate(enemyPrefab, spawnPosition,Quaternion.Euler(0,180,0));

            yield return new WaitForSeconds(spawnInterval);
        }
    }
}
