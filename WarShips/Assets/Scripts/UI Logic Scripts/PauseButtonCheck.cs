using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; 
using System.Collections.Generic; 

public class PauseButtonCheck : MonoBehaviour
{
    public GraphicRaycaster raycaster; // Gắn GraphicRaycaster của canvas vào đây
    public EventSystem eventSystem; // Gắn EventSystem của canvas vào đây
    public string targetElementName; // Tên của UI element cần kiểm tra
    public PlayerController playerController;

    void Start()
    {
        GameObject playerObject = GameObject.Find("Player");
        playerController = playerObject.GetComponent<PlayerController>();
    }
    void Update()
    {
        if (IsPointerOverSpecificUIElement(targetElementName))
        {
            Debug.Log("Clicked on UI element: " + targetElementName);
            playerController.isHover = true;
        }
        else { playerController.isHover = false; }
    }

    bool IsPointerOverSpecificUIElement(string elementName)
    {
        // Tạo PointerEventData để biết thông tin vị trí con trỏ
        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = Input.mousePosition; // Lấy vị trí chuột hiện tại

        // Tạo danh sách để chứa kết quả của raycast
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        // Raycast UI
        raycaster.Raycast(pointerEventData, raycastResults);

        // Kiểm tra xem có UI element nào trúng và có tên khớp với elementName
        foreach (RaycastResult result in raycastResults)
        {
            if (result.gameObject.name == elementName)
            {
                return true; // Nếu tên khớp, trả về true
            }
        }

        return false; // Không khớp
    }
}
