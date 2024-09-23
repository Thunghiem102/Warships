using UnityEngine;

public abstract class Bullet : MonoBehaviour
{

    public string bulletS = "Bullets";
    public BulletType bulletType;
    [SerializeField] protected float speed;
    protected BoundaryChecker boundaryChecker;

    [SerializeField] protected float damageMultiplier = 1.0f;
    protected int damageAmount;
    protected virtual void Start()
    {
        boundaryChecker = gameObject.AddComponent<BoundaryChecker>();
       
    }
    protected virtual void FixedUpdate()
    {
        Move();
    }

    protected abstract void Move();

    public void SetDamage(int baseDamage)
    {
        damageAmount = Mathf.RoundToInt(baseDamage * damageMultiplier);
    }
    protected virtual void OnTriggerEnter(Collider collision)
    {
       
        var damageable = collision.GetComponent<CharacterStats>();
        if ((bulletType == BulletType.Player && collision.gameObject.CompareTag("Enemy")) ||
               (bulletType == BulletType.Enemy && collision.gameObject.CompareTag("Player")))
        {
            if (damageable != null)
            {
               
                damageable.TakeDamage(damageAmount);
                HandleCollision(collision);
                ReturntoPool(bulletS,gameObject);
            }
            else
            {
                Debug.LogWarning("Collision object does not have a CharacterStats component.");
            }
        }
       
    }
    protected virtual void ReturntoPool(string name, GameObject gameObject)
    {
        ObjectPoolManager.Instance.ReturnToPool(name, gameObject);
    }

    protected virtual void HandleCollision(Collider collision)
    {
        // Default collision handling (can be overridden in derived classes)
    }

}
