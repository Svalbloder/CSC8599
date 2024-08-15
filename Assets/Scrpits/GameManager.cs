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
        // �ڱ༭�����޷��˳�Ӧ�ó��򣬵��ڹ�������Ϸ�л���Ч
        Application.Quit();

        // ������ڱ༭���н��в���
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
    public void PauseGame()
    {
        Time.timeScale = 0f; // ��ͣʱ��
    }

    // ������Ϸ�ķ���
    public void ResumeGame()
    {
        InMain = false;
        Time.timeScale = 1f; // �ָ�ʱ��
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
