using UnityEngine;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform playerBody; // 玩家物件的 Transform
    public float mouseSensitivity = 100f; // 鼠標靈敏度

    private float pitch = 0f; // 垂直旋轉角度（俯仰角度）
    public bool isActive =  true;

    void Start()
    {
        // 鎖定鼠標，使其不離開遊戲窗口
        if (isActive)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void Update()
    {
        if (isActive)
        {
            HandleMouseInput();
        }
        
    }

    void HandleMouseInput()
    {
        // 獲取鼠標輸入
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // 更新水平旋轉（影響玩家物件）
        playerBody.Rotate(Vector3.up * mouseX);

        // 更新垂直旋轉（影響攝像機）
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90f, 90f); // 限制垂直旋轉角度

        // 應用垂直旋轉到攝像機
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        
    }
    
    public void DisableMouseInput()
    {
        isActive = false;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EnableMouseInput()
    {
        isActive = true;
    }
}