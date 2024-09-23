using UnityEngine;
using UnityEngine.UI;
using static CharacterStats;

public class ExperienceSystem : MonoBehaviour
{
    public Slider Experiencebar;
    public int currentExperience = 0;
    public int experienceToLevelUp = 100;
    public int playerLevel = 1;
    public Text PlayerLevelText;
    private CharacterStats characterStats;

    public void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        PlayerLevelText.text = playerLevel.ToString();
        UpdateExperienceBar();
    }
    public void AddExperience(int amount)
    {
        currentExperience += amount;

        int levelsGained = 0;

        while (currentExperience >= experienceToLevelUp)
        {
            LevelUp();
            levelsGained++;
        }
        UpdateExperienceBar();
        if (levelsGained > 0)
        {
            // Mở bảng chọn nâng cấp sau khi đã lên cấp tất cả các lần
            UpgradeMenu.Instance.ShowUpgradeMenu(levelsGained);
        }

    }

    void LevelUp()
    {
        Debug.Log("Level Up");
        playerLevel++;
        PlayerLevelText.text = playerLevel.ToString();
        currentExperience -= experienceToLevelUp;
        Experiencebar.value = currentExperience;
        experienceToLevelUp = experienceToLevelUp * 15/10; // Tăng yêu cầu kinh nghiệm cho cấp tiếp theo

        // Gọi phương thức LevelUp từ CharacterStats và tăng các chỉ số
        if (characterStats != null)
        {
            int healthIncrease = 1;  // Tăng máu tối đa mỗi khi lên cấp
            int attackIncrease = 1;   // Tăng sức mạnh tấn công mỗi khi lên cấp
            int defenseIncrease = 1;  // Tăng sức mạnh phòng thủ mỗi khi lên cấp
            characterStats.LevelUp(healthIncrease, attackIncrease, defenseIncrease);
        }

    }
    public void UpdateExperienceBar()
    {
        // Tính toán tỷ lệ phần trăm và cập nhật fillAmount
        float fillAmount = Mathf.Clamp01((float)currentExperience / experienceToLevelUp);
        Experiencebar.value = fillAmount;
       
    }
}