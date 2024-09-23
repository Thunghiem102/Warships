using UnityEngine;
using UnityEngine.UI;

public class BlinkingText : MonoBehaviour
{
    public Text buttonText; // Tham chiếu đến Text component
    public float blinkSpeed = 1f; // Tốc độ nhấp nháy (tùy chỉnh theo ý muốn)

    private bool isBlinking = true;

    void Start()
    {
        if (buttonText == null)
        {
            // Tự động lấy Text component của button nếu chưa được gán
            buttonText = GetComponent<Text>();
        }
    }

    void Update()
    {
        if (isBlinking)
        {
            Blink();
        }
    }

    void Blink()
    {
        // Thay đổi alpha của text theo thời gian
        float alpha = Mathf.Abs(Mathf.Sin(Time.time * blinkSpeed));
        buttonText.color = new Color(buttonText.color.r, buttonText.color.g, buttonText.color.b, alpha);
    }

    // Có thể thêm phương thức bật/tắt nhấp nháy nếu cần
    public void ToggleBlinking()
    {
        isBlinking = !isBlinking;
    }
}
