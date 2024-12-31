using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100; // 最大血量
    public int currentHealth;   // 當前血量
    public float healthDecreaseInterval = 0.5f; // 每次減少的時間間隔（秒）
    public int healthDecreaseAmount = 1;  // 每次減少的血量

    [Header("Difficulty Settings")]
    public float intervalMultiplier = 0.9f; // 每級難度遞減的乘數
    private int increaseHealthCount = 0; // 計數器，用於追蹤 IncreaseHealth 次數
    private int difficultyLevel = 0; // 難度級別

    [Header("UI Settings")]
    public RectTransform healthBar; // 血量條的 RectTransform
    private float originalBarWidth; // 血量條的初始寬度

    [Header("Game State")]
    public bool isGameOver = false; // 遊戲是否結束

    void Start()
    {
        // 初始化血量
        currentHealth = maxHealth;

        // 獲取初始血量條寬度
        if (healthBar != null)
        {
            originalBarWidth = healthBar.sizeDelta.x;
        }

        // 啟動血量倒計時
        StartCoroutine(DecreaseHealthOverTime());
    }

    void Update()
    {
        // 更新血量條的顯示
        if (healthBar != null)
        {
            float healthPercentage = (float)currentHealth / maxHealth;
            healthBar.sizeDelta = new Vector2(originalBarWidth * healthPercentage, healthBar.sizeDelta.y);
        }
    }

    /// <summary>
    /// 每隔一定時間減少血量
    /// </summary>
    System.Collections.IEnumerator DecreaseHealthOverTime()
    {
        while (currentHealth > 0)
        {
            yield return new WaitForSeconds(healthDecreaseInterval);
            currentHealth -= healthDecreaseAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 確保血量不低於 0

            if (currentHealth <= 0 && !isGameOver)
            {
                isGameOver = true;
                Debug.Log("Game Over! Slime has starved.");
            }
        }
    }

    /// <summary>
    /// 增加血量
    /// </summary>
    /// <param name="amount">要增加的血量值</param>
    public void IncreaseHealth(int amount)
    {
        if (isGameOver) return; // 如果遊戲結束，無法加血

        // 增加血量
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // 確保血量不超過最大值

        // 增加計數器
        increaseHealthCount++;

        // 每增加五次血量，提升難度
        if (increaseHealthCount % 5 == 0)
        {
            difficultyLevel++; // 增加難度級別
            UpdateHealthDecreaseInterval();
        }

        Debug.Log($"Health increased by {amount}. Current Health: {currentHealth}");
    }

    /// <summary>
    /// 更新難度
    /// </summary>
    void UpdateHealthDecreaseInterval()
    {
        healthDecreaseInterval = Mathf.Pow(intervalMultiplier, difficultyLevel); // interval = 0.9 ^ x
        Debug.Log($"Difficulty increased! New Interval: {healthDecreaseInterval:F2}");
    }
}