using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject homepagePanel;  // 首頁面板
    public GameObject tutorialPanel; // 教學面板

    [Header("Button Reference")]
    public Button startButton;       // 開始按鈕

    [Header("Scene Settings")]
    public string gameSceneName = "GameScene"; // 遊戲場景名稱
    public float delayBeforeSceneLoad = 5f;    // 切換場景前的延遲時間（秒）

    private void Start()
    {
        // 確保兩個面板顯示
        homepagePanel.SetActive(true);
        tutorialPanel.SetActive(true);

        // 為按鈕添加點擊事件
        startButton.onClick.AddListener(OnStartButtonClicked);
    }

    private void OnStartButtonClicked()
    {
        // 隱藏首頁面板
        homepagePanel.SetActive(false);

        // 開始倒計時並切換場景
        StartCoroutine(SwitchSceneAfterDelay());
    }

    private IEnumerator SwitchSceneAfterDelay()
    {
        // 等待指定的時間
        yield return new WaitForSeconds(delayBeforeSceneLoad);

        // 切換到遊戲場景
        SceneManager.LoadScene(gameSceneName);
    }
}