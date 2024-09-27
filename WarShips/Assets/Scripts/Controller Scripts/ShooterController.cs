using System.Collections;
using UnityEngine;

public abstract class ShooterController : MonoBehaviour
{
    public float cooldown = 0.2f; // Thời gian chờ giữa các lần bắn
    public string allBulletType = "Bullets";
    private float time = 0f;
    protected int numberOfBullets = 3;

    // Variables for Burst Shooting
    private int bulletsPerBurst = 3; // Số lượng đạn trong mỗi loạt
    private float burstCooldown = 0.1f; // Thời gian chờ giữa các viên đạn trong loạt bắn
    private bool isBursting = false;
    private CharacterStats characterStats;

    private Transform player;  // Tham chiếu đến vị trí player

    protected virtual void Start()
    {
        time = cooldown; // Khởi tạo thời gian chờ ban đầu
        characterStats = gameObject.GetComponent<CharacterStats>();
        player = GameObject.FindWithTag("Player").transform;
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
    protected void EnemyShoot(Vector3 spawnPosition, int numberOfBullets)
    {
        if (CanShoot() && !isBursting)
        {
            StartCoroutine(EnemyBurstRoutine(spawnPosition, numberOfBullets));
        }
    }
    protected void EnemyShootMultiple(Vector3 spawnPosition, int numberOfBullets)
    {
        if (CanShoot() && player != null)
        {
            // Tính toán hướng từ enemy tới player
            Vector3 directionToPlayer = (player.position - spawnPosition).normalized;

            // Xác định góc chính giữa (hướng tới player)
            Quaternion mainRotation = Quaternion.LookRotation(directionToPlayer);

            // Góc lệch giữa mỗi viên đạn
            float totalAngle = 45f; // Góc tổng cộng phân bố đạn (có thể thay đổi tùy yêu cầu)
            float startAngle = -totalAngle / 2; // Bắt đầu từ góc trái nhất
            float angleStep = totalAngle / (numberOfBullets - 1); // Khoảng cách giữa mỗi viên đạn

            for (int i = 0; i < numberOfBullets; i++)
            {
                // Tính toán góc xoay cho từng viên đạn
                float angle = startAngle + angleStep * i;
                Quaternion bulletRotation = mainRotation * Quaternion.Euler(0, angle, 0); // Xoay quanh trục y

                FireBullet(spawnPosition, bulletRotation);
            }

            time = cooldown; // Đặt lại thời gian chờ sau khi bắn
        }

    }

    private IEnumerator EnemyBurstRoutine(Vector3 spawnPosition, int numberOfBullets)
    {
        isBursting = true;

        for (int i = 0; i < bulletsPerBurst; i++)
        {
            Vector3 currentSpawnPosition = transform.TransformPoint(Vector3.forward * 2);
            EnemyShootMultiple(currentSpawnPosition, numberOfBullets);
            yield return new WaitForSeconds(burstCooldown);
        }

        isBursting = false;
        time = cooldown; // Đặt lại thời gian chờ giữa các loạt bắn
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
