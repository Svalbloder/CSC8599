using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance; // ����ʵ��
    public string currentSaveSlot; // ��ǰѡ�еĴ浵
    public Button[] saveSlotButtons; // �浵�۰�ť
    private string[] saveSlotNames = { "SaveSlot1", "SaveSlot2", "SaveSlot3" };
    public TextMeshProUGUI[] saveSlotText; // TextMeshPro �ı�����

    private void Start()
    {
        // Ϊÿ����ť�����¼�����ʼ����ť����
        for (int i = 0; i < saveSlotButtons.Length; i++)
        {
            int index = i;
            saveSlotButtons[i].onClick.AddListener(() => OnSaveSlotSelected(saveSlotNames[index]));

            // ��ʼ�� TextMeshPro �ı�Ϊ���ڻ�New Game��
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
        // ����ģʽ
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ���ֱ���������ڳ����л�ʱ����
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSaveSlotSelected(string saveSlot)
    {
        SetSaveSlot(saveSlot);

        // ���浵�Ƿ����
        if (ES3.KeyExists($"{saveSlot}_allCondition"))
        {
            LoadData(); // ����ѡ��Ĵ浵����
            // ������Ϸ���������˵�
        }
        else
        {
            // ��ʼ����Ϸ�������߼�
        }
    }

    // ���õ�ǰ�浵��
    public void SetSaveSlot(string saveSlot)
    {
        currentSaveSlot = saveSlot;
    }

    // �������ݵ�ָ���浵
    public void SaveData()
    {
        LandManager.Instance.SaveData(currentSaveSlot);

        // ���浱ǰ����
        string currentDate = DateTime.Now.ToString("MMM dd, yyyy");
        ES3.Save($"{currentSaveSlot}_LastSavedDate", currentDate);

        // ����TextMeshPro�ı�
        UpdateSaveSlotButtonText(currentSaveSlot, currentDate);
    }

    // ��ָ���浵��������
    public void LoadData()
    {
        LandManager.Instance.LoadData(currentSaveSlot);
    }

    // ����TextMeshPro�ı�
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
