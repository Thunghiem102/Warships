using System.Collections;
using UnityEngine;

public enum CharacterType
{
    Player,
    Enemy
}
public class CharacterStats : MonoBehaviour
{
    [Header("Character Stats")]
    public int attackPower = 50;        // Sức mạnh tấn công
    public int defensePower = 20;       // Sức mạnh phòng thủ
    public int maxHealth = 100;         // Máu tối đa
    public int currentHealth;           // Máu hiện tại

    public CharacterType characterType;
    public delegate void OnHealthChanged(int currentHealth, int maxHealth);
    public event OnHealthChanged onHealthChanged; // Sự kiện thay đổi máu
    public HealthBar healthBar;
    public float scoreValue = 125;
    public int experiencePoints = 50;
    private EnemyExplosion explosion;
    private ExperienceSystem experienceSystem;


    void Start()
    {
        experienceSystem = FindObjectOfType<ExperienceSystem>();
        explosion = GetComponent<EnemyExplosion>();
        if (this.CompareTag("Player"))
        {
            characterType = CharacterType.Player;
        }
        else if (this.CompareTag("Enemy"))
        {
            characterType = CharacterType.Enemy;
        }
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }
        onHealthChanged?.Invoke(currentHealth, maxHealth); // Gọi sự kiện thay đổi máu khi khởi tạo
    }

    public void TakeDamage(int damageAmount)
    {
        int damageAfterDefense = Mathf.Max(Mathf.RoundToInt(damageAmount * 100 / (100 + defensePower)), 0);
        currentHealth = Mathf.Max(currentHealth - damageAfterDefense, 0);
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }


        onHealthChanged?.Invoke(currentHealth, maxHealth); // Gọi sự kiện thay đổi máu

        if (currentHealth <= 0)
        {
            if (characterType == CharacterType.Enemy)
            {
                explosion.Die();
                gameObject.SetActive(false);
                AddScore.Instance.Scoring(scoreValue);
                if (experienceSystem != null)
                {
                    experienceSystem.AddExperience(experiencePoints);
                }

            }
            else
            {
                Die(); // Nhân vật chết
            }
            
        }
    }  
    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        onHealthChanged?.Invoke(currentHealth, maxHealth); // Gọi sự kiện thay đổi máu sau khi hồi máu
    }

    private void Die()
    {
        Destroy(gameObject); // Xóa đối tượng khi nhân vật chết
    }

    public void LevelUp(int healthIncrease, int attackIncrease, int defenseIncrease)
    {
        // Tăng các chỉ số
        maxHealth += healthIncrease;
        attackPower += attackIncrease;
        defensePower += defenseIncrease;

        Heal(healthIncrease);

        // Cập nhật lại HealthBar
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
            healthBar.SetHealth(currentHealth);
        }


        //        onHealthChanged?.Invoke(currentHealth, maxHealth);


    }

   
}