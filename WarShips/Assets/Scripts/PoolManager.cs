using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager : MonoBehaviour
{
    public GameObject bulletsPrefab;
    public GameObject enemyBulletsPrefab;
    public GameObject enemyPrefab;

    void Start()
    {

        if (ObjectPoolManager.Instance != null)
        {
            InitializePool();
            SceneManager.sceneLoaded += OnSceneLoaded;  // Đăng ký sự kiện load scene
            SceneManager.sceneUnloaded += OnSceneUnloaded;  // Đăng ký sự kiện unload scene
        }
        else
        {
            Debug.LogWarning("ObjectPoolManager is null!");
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "PlayScene 1")
        {
            InitializePool();
        }
    }

    // Reset pool khi scene hiện tại bị unload
    void OnSceneUnloaded(Scene scene)
    {
        ObjectPoolManager.Instance.ResetPools(); // Trả lại tất cả object vào pool khi scene bị unload
    }
    void InitializePool()
    {
        // Reset hoặc khởi tạo lại pool cho scene mới
        ObjectPoolManager.Instance.CreatePool("Bullets", bulletsPrefab, 10);
        ObjectPoolManager.Instance.CreatePool("EnemyBullets", enemyBulletsPrefab, 5);
        ObjectPoolManager.Instance.CreatePool("Enemy", enemyPrefab, 5);

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Hủy đăng ký sự kiện
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }
}