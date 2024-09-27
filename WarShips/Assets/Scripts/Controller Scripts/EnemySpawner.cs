using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab của enemy
    public Transform[] waypoints;  // Mảng các waypoints trong scene
    public float spawnInterval = 3f; // Khoảng thời gian giữa mỗi lần spawn

    private float timeSinceLastSpawn;

    void Update()
    {
        // Kiểm tra nếu đã đến lúc sinh enemy mới
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn >= spawnInterval)
        {
            SpawnEnemy();
            timeSinceLastSpawn = 0f; // Reset thời gian chờ
        } 
    }

    void SpawnEnemy()
    {
        // Tạo enemy tại vị trí của Spawner hoặc bất kỳ vị trí nào bạn muốn
        GameObject newEnemy = ObjectPoolManager.Instance.GetFromPool("Enemy");
        newEnemy.transform.position = this.transform.position;

        // Gán waypoints cho enemy
        EnemyController enemyController = newEnemy.GetComponent<EnemyController>();
        if (enemyController != null)
        {
            enemyController.SetWaypoints(waypoints); // Truyền mảng waypoints vào cho enemy
        }
    }
}
