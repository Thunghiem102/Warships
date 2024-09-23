using System.Collections;
using UnityEngine;

public abstract class ShooterController : MonoBehaviour
{
    public float cooldown = 0.2f; // Thời gian chờ giữa các lần bắn
    public string allBulletType = "Bullets";
    private float time = 0f;
    protected int numberOfBullets = 2;

    // Variables for Burst Shooting
    private int bulletsPerBurst = 3; // Số lượng đạn trong mỗi loạt
    private float burstCooldown = 0.1f; // Thời gian chờ giữa các viên đạn trong loạt bắn
    private bool isBursting = false;
    private CharacterStats characterStats;

    protected virtual void Start()
    {
        time = cooldown; // Khởi tạo thời gian chờ ban đầu
        characterStats = gameObject.GetComponent<CharacterStats>();
    }

    protected virtual void Update()
    {
        time -= Time.deltaTime; // Giảm thời gian chờ theo thời gian trôi
    }

    // 1. Shoot Single: Bắn một viên đạn duy nhất
    protected void ShootSingle(Vector3 spawnPosition, Quaternion bulletRotation)
    {
        if (CanShoot())
        {
            FireBullet(spawnPosition, bulletRotation);
            time = cooldown; // Đặt lại thời gian chờ sau khi bắn
        }
    }

    // 2. Shoot Burst: Bắn một loạt đạn
    protected void ShootBurst(Vector3 spawnPosition, Quaternion bulletRotation)
    {
        if (CanShoot() && !isBursting)
        {
            StartCoroutine(BurstRoutine(spawnPosition, bulletRotation));
        }
    }

    // Coroutine để thực hiện việc bắn loạt đạn
    private IEnumerator BurstRoutine(Vector3 spawnPosition, Quaternion bulletRotation)
    {
        isBursting = true;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            Vector3 currentSpawnPosition = transform.TransformPoint(Vector3.forward * 2);
            FireBullet(currentSpawnPosition, bulletRotation);
            yield return new WaitForSeconds(burstCooldown);
        }

        isBursting = false;
        time = cooldown; // Đặt lại thời gian chờ giữa các loạt bắn
    }

    // 3. Shoot Multiple Directions: Bắn nhiều viên đạn theo nhiều hướng khác nhau
    protected void ShootMultipleDirections(Vector3 spawnPosition, int numberOfBullets)
    {
        if (CanShoot())
        {
            float totalAngle = 10f * numberOfBullets; // Góc tổng cộng muốn phân bố đạn
            float startAngle = -totalAngle / 2; // Bắt đầu từ góc trái nhất
            float angleStep = totalAngle / (numberOfBullets - 1); // Khoảng cách giữa các viên đạn

            for (int i = 0; i < numberOfBullets; i++)
            {
                float angle = startAngle + angleStep * i; // Tính toán góc cho từng viên đạn
                Quaternion bulletRotation = Quaternion.Euler(new Vector3(0, angle, 0));
                FireBullet(spawnPosition, bulletRotation);
            }

            time = cooldown; // Đặt lại thời gian chờ sau khi bắn
        }
    }

    // FireBullet: Phương thức dùng để bắn đạn (dùng chung)
    private void FireBullet(Vector3 spawnPosition, Quaternion bulletRotation)
    {
        GameObject bullet = ObjectPoolManager.Instance.GetFromPool(allBulletType);
        if (bullet != null)
        {
            bullet.transform.position = spawnPosition;
            bullet.transform.rotation = bulletRotation;
        }
        // Gọi hàm SetDamage trên đạn
        Bullet bulletComponent = bullet.GetComponent<Bullet>();
        if (bulletComponent != null)
        {
            bulletComponent.SetDamage(characterStats.attackPower);
        }
    }

    // Kiểm tra nếu có thể bắn
    protected bool CanShoot()
    {
        return time <= 0f;
    }
   
}
