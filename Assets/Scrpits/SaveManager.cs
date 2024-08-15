using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance; // 单例实例
    public string currentSaveSlot; // 当前选中的存档
    public Button[] saveSlotButtons; // 存档槽按钮
    private string[] saveSlotNames = { "SaveSlot1", "SaveSlot2", "SaveSlot3" };
    public TextMeshProUGUI[] saveSlotText; // TextMeshPro 文本数组

    private void Start()
    {
        // 为每个按钮设置事件并初始化按钮名称
        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            int index = i;
            saveSlotButtons[i].onClick.AddListener(() => OnSaveSlotSelected(saveSlotNames[index]));

            // 初始化 TextMeshPro 文本为日期或“New Game”
            if (ES3.KeyExists($"{saveSlotNames[i]}_allCondition"))
            {
                string lastSavedDate = ES3.Load<string>($"{saveSlotNames[i]}_LastSavedDate");
                saveSlotText[i].text = lastSavedDate;
            }
            else
            {
                saveSlotText[i].text = "New Game";
            }
        }
    }

    private void Awake()
    {
        // 单例模式
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持保存管理器在场景切换时存在
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSaveSlotSelected(string saveSlot)
    {
        SetSaveSlot(saveSlot);

        // 检查存档是否存在
        if (ES3.KeyExists($"{saveSlot}_allCondition"))
        {
            LoadData(); // 加载选择的存档数据
            // 继续游戏或启动主菜单
        }
        else
        {
            // 开始新游戏或其它逻辑
        }
    }

    // 设置当前存档槽
    public void SetSaveSlot(string saveSlot)
    {
        currentSaveSlot = saveSlot;
    }

    // 保存数据到指定存档
    public void SaveData()
    {
        LandManager.Instance.SaveData(currentSaveSlot);

        // 保存当前日期
        string currentDate = DateTime.Now.ToString("MMM dd, yyyy");
        ES3.Save($"{currentSaveSlot}_LastSavedDate", currentDate);

        // 更新TextMeshPro文本
        UpdateSaveSlotButtonText(currentSaveSlot, currentDate);
    }

    // 从指定存档加载数据
    public void LoadData()
    {
        LandManager.Instance.LoadData(currentSaveSlot);
    }

    // 更新TextMeshPro文本
    private void UpdateSaveSlotButtonText(string saveSlot, string newText)
    {
        for (int i = 0; i < saveSlotNames.Length; i++)
        {
            if (saveSlotNames[i] == saveSlot)
            {
                saveSlotText[i].text = newText;
                break;
            }
        }
    }
}
