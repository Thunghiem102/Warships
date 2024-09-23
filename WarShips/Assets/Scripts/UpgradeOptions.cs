public class UpgradeOption
{
    public string name; // Tên của nâng cấp
    public string description; // Mô tả của nâng cấp
    public System.Action applyUpgrade; // Hành động khi áp dụng nâng cấp

    public UpgradeOption(string name, string description, System.Action applyUpgrade)
    {
        this.name = name;
        this.description = description;
        this.applyUpgrade = applyUpgrade;
    }
}