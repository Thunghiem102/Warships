using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    // Dictionary để chứa các pool với key là string và value là object pool
    private Dictionary<string, ObjectPool> pools = new Dictionary<string, ObjectPool>();

    private void Awake()
    {
        // Đảm bảo ObjectPoolManager là singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Giữ lại ObjectPoolManager khi chuyển scene
        }
        else
        {
            Debug.Log("ObjectPoolManager already exists. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    // Phương thức khởi tạo pool
    public void CreatePool(string key, GameObject prefab, int initialSize)
    {
        if (!pools.ContainsKey(key))
        {
            ObjectPool newPool = new ObjectPool(prefab, initialSize, transform);
            pools.Add(key, newPool);
        }
    }

    // Lấy một object từ pool
    public GameObject GetFromPool(string key)
    {
        if (pools.ContainsKey(key))
        {
            return pools[key].GetObject();
        }

        Debug.LogWarning("Pool with key " + key + " doesn't exist.");
        return null;
    }

    // Trả lại object vào pool
    public void ReturnToPool(string key, GameObject obj)
    {
        if (pools.ContainsKey(key))
        {
            pools[key].ReturnObject(obj);
        }
        else
        {
            Debug.LogWarning("Pool with key " + key + " doesn't exist.");
        }
    }
    // Phương thức reset tất cả các pool
    public void ResetPools()
    {
        foreach (var pool in pools.Values)
        {
            pool.ResetPool(); // Reset mỗi pool
        }
    }
}