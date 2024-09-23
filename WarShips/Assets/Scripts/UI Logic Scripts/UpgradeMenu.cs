using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UpgradeMenu : MonoBehaviour
{
    public static UpgradeMenu Instance;
    public GameObject upgradeMenuUI;

    public Text[] upgradeTitleTexts;
    public TextMeshProUGUI[] upgradeDescriptionTexts; // Mảng chứa các TextMeshProUGUI cho mô tả nâng cấp
    public Button[] upgradeButtons; // Các nút nâng cấp tương ứng với các chỉ số

    private int levelsRemaining;
    private CharacterStats characterStats;

    private List<UpgradeOption> shootingModeUpgrades = new List<UpgradeOption>();
    private List<UpgradeOption> statUpgrades = new List<UpgradeOption>();
    private List<UpgradeOption> currentUpgrades = new List<UpgradeOption>(); // Danh sách nâng cấp hiện tại

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Khởi tạo danh sách tất cả các nâng cấp
        InitializeUpgrades();
    }

    private void InitializeUpgrades()
    {
        // Khởi tạo các tùy chọn chế độ bắn
        shootingModeUpgrades.Add(new UpgradeOption(
            "Single Shot",
            "Switch to Single Shot Mode",
            () =>
            {
                FindObjectOfType<PlayerController>().currentShootingMode = ShootingMode.Single;
            }));

        shootingModeUpgrades.Add(new UpgradeOption(
            "Burst Shot",
            "Switch to Burst Shot Mode",
            () =>
            {
                FindObjectOfType<PlayerController>().currentShootingMode = ShootingMode.Burst;
            }));

        shootingModeUpgrades.Add(new UpgradeOption(
            "Multiple Directions Shot",
            "Switch to Multiple Directions Shot Mode",
            () =>
            {
                FindObjectOfType<PlayerController>().currentShootingMode = ShootingMode.MultipleDirections;
            }));

        // Khởi tạo các tùy chọn nâng cấp chỉ số
        statUpgrades.Add(new UpgradeOption(
            "Increase Attack Power",
            "Increase Attack Power by 15%",
            () =>
            {
                int attackIncrease = Mathf.RoundToInt(characterStats.attackPower * 0.15f);
                characterStats.attackPower += attackIncrease;
            }));

        statUpgrades.Add(new UpgradeOption(
            "Increase Defense Power",
            "Increase Defense Power by 20%",
            () =>
            {
                int defenseIncrease = Mathf.RoundToInt(characterStats.defensePower * 0.20f);
                characterStats.defensePower += defenseIncrease;
            }));

        statUpgrades.Add(new UpgradeOption(
            "Increase Max Health",
            "Increase Max Health by 20%",
            () =>
            {
                int healthIncrease = Mathf.RoundToInt(characterStats.maxHealth * 0.20f);
                characterStats.maxHealth += healthIncrease;
                characterStats.Heal(healthIncrease);
                characterStats.healthBar.SetMaxHealth(characterStats.maxHealth);
                characterStats.healthBar.SetHealth(characterStats.currentHealth);
            }));

    }
    private bool hasSelectedShootingUpgrades = false; // Biến để kiểm tra xem đã chọn chế độ bắn hay chưa

    private void SelectInitialUpgrades()
    {
        currentUpgrades.Clear();

        // Đảm bảo ba lựa chọn đầu tiên là các chế độ bắn
        for (int i = 0; i < 3; i++)
        {
            currentUpgrades.Add(shootingModeUpgrades[i]);
        }
    }

    private void SelectStatUpgrades()
    {
        currentUpgrades.Clear();
        // Chọn ngẫu nhiên các nâng cấp chỉ số
        List<int> usedIndices = new List<int>();
        while (currentUpgrades.Count < upgradeButtons.Length)
        {
            int randomIndex = Random.Range(0, statUpgrades.Count);
            if (!usedIndices.Contains(randomIndex))
            {
                currentUpgrades.Add(statUpgrades[randomIndex]);
                usedIndices.Add(randomIndex);
            }
        }
    }

    public void ShowUpgradeMenu(int levelsGained)
    {
        // Tìm kiếm `CharacterStats` chỉ dành cho Player
        characterStats = FindObjectsOfType<CharacterStats>().FirstOrDefault(cs => cs.characterType == CharacterType.Player);

        levelsRemaining = levelsGained;

        // Chọn nâng cấp tùy thuộc vào trạng thái hiện tại
        if (!hasSelectedShootingUpgrades)
        {
            SelectInitialUpgrades();
            hasSelectedShootingUpgrades = true;
        }
        else
        {
            SelectStatUpgrades();
        }

        // Cập nhật mô tả cho các nâng cấp
        UpdateUpgradeDescriptions();

        upgradeMenuUI.SetActive(true);
        Time.timeScale = 0f; // Dừng game khi bảng nâng cấp hiện lên
    }


    private void UpdateUpgradeDescriptions()
    {
        for (int i = 0; i < upgradeDescriptionTexts.Length; i++)
        {
            upgradeTitleTexts[i].text = currentUpgrades[i].name;
            upgradeDescriptionTexts[i].text = currentUpgrades[i].description;

            Color upgradeColor = GetColorForUpgrade(currentUpgrades[i].name);
            upgradeButtons[i].image.color = upgradeColor;
            upgradeTitleTexts[i].color = upgradeColor;
        }
    }

    public void OnUpgradeOptionSelected(int index)
    {
        currentUpgrades[index].applyUpgrade(); // Áp dụng nâng cấp đã chọn

        levelsRemaining--;

        if (levelsRemaining > 0)
        {
            ShowUpgradeMenu(levelsRemaining);
        }
        else
        {
            CloseUpgradeMenu();
        }
    }

    public void CloseUpgradeMenu()
    {
        upgradeMenuUI.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game khi đóng bảng nâng cấp
    }
    private Color GetColorForUpgrade(string upgradeName)
    {
        switch (upgradeName)
        {
            case "Increase Attack Power":
                return Color.red; // Màu đỏ cho tăng tấn công
            case "Increase Defense Power":
                return Color.blue; // Màu xanh lam cho tăng phòng thủ
            case "Increase Max Health":
                return Color.green; // Màu xanh lá cây cho tăng máu
            default:
                return Color.white; // Màu trắng mặc định
        }
    }
}



