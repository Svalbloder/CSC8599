using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnPage : MonoBehaviour
{
    public List<GameObject> gameObjectList;  // 存储所有需要翻页的 GameObject
    public Button nextButton;                // 下一页按钮
    public Button prevButton;                // 上一页按钮
    public int maxPageCount = 0;             // 最大页数

    private int currentIndex = 0;            // 当前显示的索引

    void Start()
    {
        UpdatePage();                        // 初始化页面显示

        nextButton.onClick.AddListener(ShowNextPage);
        prevButton.onClick.AddListener(ShowPrevPage);
    }

    // 显示下一页
    void ShowNextPage()
    {
        if (currentIndex < Mathf.Min(gameObjectList.Count, maxPageCount) - 1)
        {
            currentIndex++;
            UpdatePage();
        }
    }

    // 显示上一页
    void ShowPrevPage()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePage();
        }
    }

    // 更新页面显示
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

        // 根据当前索引更新按钮的可用性
        prevButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < Mathf.Min(gameObjectList.Count, maxPageCount) - 1;
    }
}
