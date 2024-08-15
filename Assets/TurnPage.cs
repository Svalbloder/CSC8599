using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPage : MonoBehaviour
{
    public List<GameObject> gameObjectList;  // �洢������Ҫ��ҳ�� GameObject
    public Button nextButton;                // ��һҳ��ť
    public Button prevButton;                // ��һҳ��ť
    public int maxPageCount = 0;             // ���ҳ��

    private int currentIndex = 0;            // ��ǰ��ʾ������

    void Start()
    {
        UpdatePage();                        // ��ʼ��ҳ����ʾ

        nextButton.onClick.AddListener(ShowNextPage);
        prevButton.onClick.AddListener(ShowPrevPage);
    }

    // ��ʾ��һҳ
    void ShowNextPage()
    {
        if (currentIndex < Mathf.Min(gameObjectList.Count, maxPageCount) - 1)
        {
            currentIndex++;
            UpdatePage();
        }
    }

    // ��ʾ��һҳ
    void ShowPrevPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePage();
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

        // ���ݵ�ǰ�������°�ť�Ŀ�����
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < Mathf.Min(gameObjectList.Count, maxPageCount) - 1;
    }
}
