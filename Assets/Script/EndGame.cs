using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;  
using UnityEngine;
using UnityEngine.UI;   

public class EndGame: MonoBehaviour
{
    public GameObject EndGameCanvas;
    public HpController hpController;
    public FirstPersonCamera firstPersonCamera;
    public GameObject CameraCharacter;
    
    // Start is called before the first frame update
    void Start()
    {
        if (EndGameCanvas != null)
        {
            EndGameCanvas.SetActive(false);
        }
    
    }

    // Update is called once per frame
    void Update()
    {
        if (hpController != null && hpController.currentHealth <= 0)
        {
            firstPersonCamera.DisableMouseInput();
            TriggerEndGame();
            
        }
    }
    void TriggerEndGame()
    {
        // 显示结束界面
        EndGameCanvas.SetActive(true);
   
    }
    



    public void RestartGame()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Debug.Log("ding");
    }
    
}
