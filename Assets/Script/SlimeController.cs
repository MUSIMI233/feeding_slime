using UnityEngine;
using UnityEngine.UI;

public class SlimeController : MonoBehaviour
{
    public Transform player; // 參考玩家的 Transform
    public HpController hpController; // 參考 GameManager
    public PlayerController playerController;
    // public Image powerUpImage;
    public float powerUpDuration = 30f;

    void Update()
    {
        // 始終面向玩家
        if (player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.y = 0; // 忽略 Y 軸（防止傾斜）
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 檢測碰撞對象是否帶有 Food 標籤
        if (other.CompareTag("Food"))
        {
            FoodBehaviour food = other.GetComponent<FoodBehaviour>();
            if (food != null && hpController != null)
            {
                // 調用 HpController 的 IncreaseHealth 方法
                hpController.IncreaseHealth(food.GetNutritionalValue());
                Debug.Log($"Slime ate food with nutritional value: {food.GetNutritionalValue()}");

                // 如果是 Spinach，啟動超能力
                if (food.objectName == "Spinach" && playerController != null)
                {
                    Debug.Log("Slime ate Spinach! Activating sprint power-up.");
                    playerController.ActivateSprint(powerUpDuration);
                }

                // 銷毀食物
                Destroy(other.gameObject);
            }
        }
    }
}