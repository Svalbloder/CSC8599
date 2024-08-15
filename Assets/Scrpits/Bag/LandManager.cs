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
    public static LandManager Instance; // 单例实例
    public float farmerSpeed = 2.0f; // 农民移动速度
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
    public Queue<FarmLand> taskQueue = new Queue<FarmLand>(); // 任务队列
    public List<FarmLand> farmLandList = new List<FarmLand>();
   
    public Animator animator; // 动画控制器
    public FarmLand currentField; // 当前农田
    public float speed = 2.0f; // 移动速度
    public Vector2 targetPosition; // 目标位置  
    public Transform wanderCenterObject; // 漫步中心物体
    public float wanderingRadius = 5.0f; // 漫步半径
    public float wanderingCooldown = 3.0f; // 漫步冷却时间
    public float GrowSpeed;
    public bool holdingSeed;
    private FarmLand selectedFarmLand; // 当前选中的农田
    public Seed currentSeed;
    public float allCondition;
    public Inventory myBag;
    public GameObject heartPartical;

    public bool isWorking = false; // 是否正在工作
    public bool hasTarget = false; // 是否有目标位置
    public bool isWandering = false; // 是否在漫步
    private void Awake()
    {
        // 单例模式
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
        // 检查是否有新的任务
        if (taskQueue.Count > 0 && !isWorking)
        {
            StartNextTask();           
        }
        if (hasTarget)
        {
            MoveToTarget(); // 移动到目标位置
        }
        else if (!isWorking && !isWandering)
        {
            StartWandering(); // 开始漫步
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
        // 移动到目标位置
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        if (transform.position.x < 0)
        {
            // 向右走
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // 向左走
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (Vector2.Distance(transform.position, targetPosition) < 0.1f)
        {
            hasTarget = false;
            if (isWandering)
            {
                // 如果在漫步，到达目标位置后等待一段时间再选择新的目标
                Invoke("StartWandering", wanderingCooldown);
            }
            else
            {
                PerformAction(); // 执行任务动作
            }
        }
    }
    public void AddTask(FarmLand field)
    {
        taskQueue.Enqueue(field);
        if (!isWorking)
        {
            StopWandering(); // 停止漫步
            StartNextTask(); // 开始下一个任务
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
            isWandering = false; // 有新任务时停止漫步
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
            // 向右走
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        else
        {
            // 向左走
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }
    private void StopWandering()
    {
        isWandering = false;
        hasTarget = false; // 停止漫步时停止移动
        CancelInvoke("StartWandering"); // 取消可能的再漫步调用
       
    }
    private void OnDrawGizmos()
    {
        // 在编辑器中绘制漫步半径范围
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
            selectedFarmLand.ResetPosition(); // 重置上一个被点击的农田位置
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
            fillImage.color = Color.red; // 填充颜色为红色
            text.color = Color.red; // 文本颜色为红色
        }
        else if(value >=0)
        {
            fillImage.color = Color.green; // 正常情况下，填充颜色为绿色（或其他颜色）
            text.color = Color.black; // 文本颜色为黑色
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

        // 保存 allCondition
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
