using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private GameObject prefab;
    private Queue<GameObject> poolQueue;
    private int initialSize;
    private List<GameObject> activeObjects = new List<GameObject>(); // Lưu trữ các object đang hoạt động


    private Transform _poolTransform;
    // Constructor
    public ObjectPool(GameObject prefab, int initialSize, Transform transform)
    {
        this._poolTransform = transform;
        this.prefab = prefab;
        this.initialSize = initialSize;
        poolQueue = new Queue<GameObject>();

        // Tạo trước các object khi pool được khởi tạo
        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = GameObject.Instantiate(prefab, _poolTransform);
            obj.SetActive(false);
            poolQueue.Enqueue(obj);
        }
    }

    // Lấy object từ pool
    public GameObject GetObject()
    {
        GameObject obj;
        if (poolQueue.Count > 0)
        {
            obj = poolQueue.Dequeue();
        }
        else
        {
            obj = GameObject.Instantiate(prefab, _poolTransform);
        }

        obj.SetActive(true);
        obj.transform.parent = null;
        activeObjects.Add(obj); // Thêm object vào danh sách đang hoạt động
        return obj;
    }

    // Trả object vào pool
    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
        poolQueue.Enqueue(obj);
        obj.transform.parent = _poolTransform;
        activeObjects.Remove(obj); // Loại bỏ object khỏi danh sách đang hoạt động
    }
    // Phương thức reset pool
    public void ResetPool()
    {
        foreach (var obj in activeObjects)
        {
            ReturnObject(obj); // Trả lại tất cả object vào pool
        }
        activeObjects.Clear(); // Xóa danh sách các object đang hoạt động 

    }
}