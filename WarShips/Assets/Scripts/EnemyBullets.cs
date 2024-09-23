using UnityEngine;

public class EnemyBullet : Bullet
{
    protected override void Start()
    {
        base.Start();
        damageMultiplier = 1;
    }
    protected override void Move()
    {
        transform.Translate(Vector3.forward * (speed * Time.deltaTime));
    }

    protected override void HandleCollision(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Handle additional logic specific to EnemyBullet hitting a Player
        }
    }
}