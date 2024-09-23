using UnityEngine;

public class SingleBullet : Bullet
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Handle additional logic specific to PlayerBullet hitting an Enemy
        }
    }
}