using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beginer : MonoBehaviour
{
    public List<GameObject> gameObjectList;  // 存储所有需要翻页的 GameObject
    public Button nextButton;                // 下一页按钮
    public int maxPageCount;             // 最大页数
    private int currentIndex = 0;            // 当前显示的索引
    public GameObject Guide;
    public House house;
    void Start()
    {
        UpdatePage();                        
        nextButton.onClick.AddListener(ShowNextPage);
    }

    // 显示下一页
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
    }
    void Close()
    {
        Guide.SetActive(false);
        Time.timeScale = 1f;
        house.enabled = true;
    }
}