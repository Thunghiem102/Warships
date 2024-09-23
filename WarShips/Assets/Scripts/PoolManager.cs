﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class PoolManager : MonoBehaviour
{
    public GameObject bulletsPrefab;
    public GameObject enemyBulletsPrefab;


    void Start()
    {

        if (ObjectPoolManager.Instance != null)
        {
            InitializePool();
            SceneManager.sceneLoaded += OnSceneLoaded;  // Đăng ký sự kiện load scene
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
            ObjectPoolManager.Instance.ResetPools(); // Reset tất cả các pool khi scene được load lại
            InitializePool();
        }
    }
    void InitializePool()
    {
        // Reset hoặc khởi tạo lại pool cho scene mới
        ObjectPoolManager.Instance.CreatePool("Bullets", bulletsPrefab, 10);
        ObjectPoolManager.Instance.CreatePool("EnemyBullets", enemyBulletsPrefab, 5);

    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  // Hủy đăng ký sự kiện
    }
}