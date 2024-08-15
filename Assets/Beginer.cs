using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beginer : MonoBehaviour
{
    public List<GameObject> gameObjectList;  // �洢������Ҫ��ҳ�� GameObject
    public Button nextButton;                // ��һҳ��ť
    public int maxPageCount;             // ���ҳ��
    private int currentIndex = 0;            // ��ǰ��ʾ������
    public GameObject Guide;
    public House house;
    void Start()
    {
        UpdatePage();                        
        nextButton.onClick.AddListener(ShowNextPage);
    }

    // ��ʾ��һҳ
    void ShowNextPage()
    {
        if (currentIndex < Mathf.Min(gameObjectList.Count, maxPageCount) - 1)
        {
            currentIndex++;
            UpdatePage();
        }
        else
        {
            Close();
        }
    }
    // ����ҳ����ʾ
    void UpdatePage()
    {
        for (int i = 0; i < gameObjectList.Count; i++)
        {
            if (i == currentIndex)
            {
                gameObjectList[i].SetActive(true);
            }
            else
            {
                gameObjectList[i].SetActive(false);
            }
        }
    }
    void Close()
    {
        Guide.SetActive(false);
        Time.timeScale = 1f;
        house.enabled = true;
    }
}