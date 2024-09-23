using UnityEngine;

public class BoundaryChecker : MonoBehaviour
{
    private Camera mainCamera;
    private Vector3 minScreenBounds;
    private Vector3 maxScreenBounds;
    public float extraBoundary = 10f; // Khoảng cách thêm ngoài giới hạn màn hình

    void Start()
    {
        mainCamera = Camera.main;
        minScreenBounds = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, mainCamera.transform.position.y));
        maxScreenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.y));
        
    }

    void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        if (transform.position.x < minScreenBounds.x - extraBoundary || transform.position.x > maxScreenBounds.x + extraBoundary ||
            transform.position.z < minScreenBounds.z - extraBoundary || transform.position.z > maxScreenBounds.z + extraBoundary)
        {
            DestroyOrDeactivate();
        }
    }

    private void DestroyOrDeactivate()
    {
        if (IsPooledObject(gameObject))
        {
            ReturnToCorrectPool();
        }
        else
        {
            Destroy(gameObject); // Hủy đối với các đối tượng khác như Enemy
        }
    }

    private bool IsPooledObject(GameObject obj)
    {
        return obj.CompareTag("PlayerBullet") || obj.CompareTag("EnemyBullet");
    }

    private void ReturnToCorrectPool()
    {
        string poolName = GetPoolName();
        ObjectPoolManager.Instance.ReturnToPool(poolName, gameObject);
    }

    private string GetPoolName()
    {
        if (gameObject.CompareTag("PlayerBullet"))
        {
            return "Bullets";
        }
        else if (gameObject.CompareTag("EnemyBullet"))
        {
            return "EnemyBullets";
        }
        return "Bullets"; 
    }
}
