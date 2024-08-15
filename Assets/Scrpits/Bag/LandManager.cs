using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LandManager : MonoBehaviour
{
    public static LandManager Instance; // ����ʵ��
    public float farmerSpeed = 2.0f; // ũ���ƶ��ٶ�
    public List<FarmLand> farmLands;
    public Slider GrowSpeedSilder;
    public Slider fertilitySlider; 
    public Slider organicSlider; 
    public Slider conditionSlider; 
    public Slider waterSlider;
    public Slider allSlider;
    public TMP_Text fertilityText;
    public TMP_Text organicText;
    public TMP_Text conditionText;
    public TMP_Text waterText;
    public TMP_Text allText;
    public Image fertilityFillImage;
    public Image organicFillImage;
    public Image conditionFillImage;
    public Image waterFillImage;
    public Image allConditionFillImage;
    public GameObject SilderUi;
    public GameObject SeedUi;
    public Queue<FarmLand> taskQueue = new Queue<FarmLand>(); // �������
    public List<FarmLand> farmLandList = new List<FarmLand>();
   
    public Animator animator; // ����������
    public FarmLand currentField; // ��ǰũ��
    public float speed = 2.0f; // �ƶ��ٶ�
    public Vector2 targetPosition; // Ŀ��λ��  
    public Transform wanderCenterObject; // ������������
    public float wanderingRadius = 5.0f; // �����뾶
    public float wanderingCooldown = 3.0f; // ������ȴʱ��
    public float GrowSpeed;
    public bool holdingSeed;
    private FarmLand selectedFarmLand; // ��ǰѡ�е�ũ��
    public Seed currentSeed;
    public float allCondition;
    public Inventory myBag;
    public GameObject heartPartical;

    public bool isWorking = false; // �Ƿ����ڹ���
    public bool hasTarget = false; // �Ƿ���Ŀ��λ��
    public bool isWandering = false; // �Ƿ�������
    private void Awake()
    {
        // ����ģʽ
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        targetPosition = transform.position;
        GrowSpeedSilder.value = 0.25f;
      
    }
    void Update()
    {
       GrowSpeed = GrowSpeedSilder.value * 4;
       CalculateTotal();
        // ����Ƿ����µ�����
        if (taskQueue.Count > 0 && !isWorking)
        {
            StartNextTask();           
        }
        if (hasTarget)
        {
            MoveToTarget(); // �ƶ���Ŀ��λ��
        }
        else if (!isWorking && !isWandering)
        {
            StartWandering(); // ��ʼ����
        }
        if (selectedFarmLand != null)
        {
            UpdateUI(fertilitySlider, fertilityFillImage, fertilityText, selectedFarmLand.totalFertility);
            UpdateUI(organicSlider, organicFillImage, organicText, selectedFarmLand.totalOrganic);
            UpdateUI(conditionSlider, conditionFillImage, conditionText, selectedFarmLand.totalCondition);
            UpdateUI(waterSlider, waterFillImage, waterText, selectedFarmLand.totalWater);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (!EventSystem.current.IsPointerOverGameObject()&&selectedFarmLand!=null&&!holdingSeed)
            {
                selectedFarmLand.ResetPosition();
                selectedFarmLand = null;
            }
        }
    }
    public void CalculateTotal()
    {
        allCondition = 0f; 
        foreach (FarmLand farmland in farmLandList)
        {
            allCondition += farmland.totalCondition;
        }
        UpdateUI(allSlider, allConditionFillImage, allText, allCondition);
    }
    private void MoveToTarget()
    {
        // �ƶ���Ŀ��λ��
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (transform.position.x < 0)
        {
            // ������
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // ������
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            hasTarget = false;
            if (isWandering)
            {
                // ���������������Ŀ��λ�ú�ȴ�һ��ʱ����ѡ���µ�Ŀ��
                Invoke("StartWandering", wanderingCooldown);
            }
            else
            {
                PerformAction(); // ִ��������
            }
        }
    }
    public void AddTask(FarmLand field)
    {
        taskQueue.Enqueue(field);
        if (!isWorking)
        {
            StopWandering(); // ֹͣ����
            StartNextTask(); // ��ʼ��һ������
        }
    }
    private void StartNextTask()
    {
        if (taskQueue.Count > 0)
        {
            currentField = taskQueue.Dequeue();
            targetPosition = currentField.transform.position;
            hasTarget = true;
            isWorking = true;
            isWandering = false; // ��������ʱֹͣ����
        }
        else
        {
            isWorking = false;
        }
    }
    private void PerformAction()
    {
        hasTarget = false;
        switch (currentField.currentAction)
        {
            case FarmLand.FieldAction.Plant:
                animator.Play("Plant");
                break;
            case FarmLand.FieldAction.Water:
                animator.Play("Water");
                break;
            case FarmLand.FieldAction.Farm:
                animator.Play("Farm");
                break;
            case FarmLand.FieldAction.Harvest:
                animator.Play("Harvest");
                break;
            case FarmLand.FieldAction.Rest:
                animator.Play("Rest");
                break;
        }
    }
    public void OnActionComplete()
    {
        currentField.currentAction = FarmLand.FieldAction.None;      
        StartNextTask();     
    }
    public void OnFarmComplete()
    {
        currentField.canPlant = true;
        currentField.farmRenderer.sprite = currentField.farmSprite;
        OnActionComplete();
    }
    public void OnWaterComplete()
    {
        currentField.Water();
        OnActionComplete();
    }
    public void OnRestComplete()
    {
        currentField.Rest();
        OnActionComplete();
    }
    public void OnPlantComplete()
    {
        currentField.Plant();
        OnActionComplete();
    }
    public void OnHarvestComplete()
    {
        currentField.Harvest();
        OnActionComplete();
    }
    private void StartWandering()
    {
        isWandering = true;
        Vector2 randomDirection = Random.insideUnitCircle.normalized * Random.Range(0, wanderingRadius);
        targetPosition = (Vector2)wanderCenterObject.position + randomDirection;
        hasTarget = true;
        if (randomDirection.x < 0)
        {
            // ������
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // ������
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private void StopWandering()
    {
        isWandering = false;
        hasTarget = false; // ֹͣ����ʱֹͣ�ƶ�
        CancelInvoke("StartWandering"); // ȡ�����ܵ�����������
       
    }
    private void OnDrawGizmos()
    {
        // �ڱ༭���л��������뾶��Χ
        if (wanderCenterObject != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(wanderCenterObject.position, wanderingRadius);
        }
    }
    public void OnFarmLandClicked(FarmLand field)
    {
        if (selectedFarmLand != null && selectedFarmLand != field)
        {
            selectedFarmLand.ResetPosition(); // ������һ���������ũ��λ��
        }
        selectedFarmLand = field; 
        selectedFarmLand.MoveUp();
        if (!holdingSeed)
            SilderUi.SetActive(true);
    }
    void UpdateUI(Slider slider, Image fillImage, TMP_Text text, float value)
    {
        float displayValue;
        if (slider == allSlider)
        {
            displayValue = value /80;
        }
        else if (slider == conditionSlider)
        {
            displayValue = value / 10;
        }
        else
        {
            displayValue = value;
        }
        slider.value = Mathf.Abs(displayValue);
        text.text = Mathf.Abs((int)displayValue).ToString();
        if (value < 0)
        {
            fillImage.color = Color.red; // �����ɫΪ��ɫ
            text.color = Color.red; // �ı���ɫΪ��ɫ
        }
        else if(value >=0)
        {
            fillImage.color = Color.green; // ��������£������ɫΪ��ɫ����������ɫ��
            text.color = Color.black; // �ı���ɫΪ��ɫ
        }
    }
    public void ifHeal()
    {
        if (selectedFarmLand.isHeal)
        {            
            heartPartical.SetActive(true);
            selectedFarmLand.isHeal = false;
            StartCoroutine(stopPartical());
        }
    }
    public void SaveData(string saveSlot)
    {
        int farmlandIndex = 0;
        foreach (FarmLand farmLand in farmLandList)
        {
            string farmlandKey = $"{saveSlot}_FarmLand_{farmlandIndex}";
            farmLand.SaveData(farmlandKey);
            farmlandIndex++;
        }

        // ���� allCondition
        ES3.Save(saveSlot + "_allCondition", allCondition);
    }
    public void LoadData(string saveSlot)
    {
        int farmlandIndex = 0;
        foreach (FarmLand farmLand in farmLandList)
        {
            string farmlandKey = $"{saveSlot}_FarmLand_{farmlandIndex}";
            farmLand.LoadData(farmlandKey);
            farmlandIndex++;
        }
        allCondition = ES3.Load<float>(saveSlot + "_allCondition", 0f);
        foreach (FarmLand farmland in farmLandList)
        {
            if (farmland.isFarm)
            {
                farmland.farmRenderer.sprite = farmland.farmSprite;
            }
            if (farmland.needWater)
            {
                farmland.needIconRenderer.sprite = farmland.waterIcon;
            }
            if (farmland.needHarvest)
            {
                farmland.needIconRenderer.sprite = farmland.harvestIcon;
            }
            if (farmland.needPlant)
            {
                farmland.needIconRenderer.sprite = farmland.plantIcon;
            }
        }
    }
    IEnumerator stopPartical()
    {
        yield return new WaitForSeconds(1f);
        heartPartical.SetActive(false);
    }
}
