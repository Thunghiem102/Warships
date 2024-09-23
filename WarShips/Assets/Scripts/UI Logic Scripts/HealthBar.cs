using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public Text currentHPText;
    public Text maxHPText;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
        maxHPText.text = health.ToString();
    }

    public void SetHealth(int health)
    {
        slider.value = health;
        currentHPText.text = health.ToString();
    }
}