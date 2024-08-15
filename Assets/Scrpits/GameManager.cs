using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject PauseUI;
    public bool InMain;
    public GameObject MainUI;
    public GameObject Beginer;
    public bool ifStart;
    public void Start()
    {
        PauseGame();
        InMain = true; 
        MainUI.SetActive(true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&&!InMain)
        {
            PauseUI.SetActive(true);
            PauseGame();
        }
    }
    public void QuitGame()
    {
        // 在编辑器中无法退出应用程序，但在构建的游戏中会有效
        Application.Quit();

        // 如果是在编辑器中进行测试
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void PauseGame()
    {
        Time.timeScale = 0f; // 暂停时间
    }

    // 继续游戏的方法
    public void ResumeGame()
    {
        InMain = false;
        Time.timeScale = 1f; // 恢复时间
    }
    public void IfStart()
    {
        if (!ifStart)
        {
            Beginer.SetActive(true);
            ifStart = true;
            InMain = false;
        }
        else
        {
            ResumeGame();
        }
    }
}
