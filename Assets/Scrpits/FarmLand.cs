using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class FarmLand : MonoBehaviour,IPointerDownHandler
{
    [Tooltip("土地默认数据")]
    [Header("土地默认数据")]
    public List<SeedPoint> seedPoints; // 种子点列表
    public GameObject land;
    public Animator iconAnim;
    public Sprite farmSprite; // 农田的精灵图片
    public SpriteRenderer farmRenderer; // 农田的精灵渲染器
    public GameObject needIcon; // 需要图标的游戏对象
    public SpriteRenderer needIconRenderer; // 需要图标的渲染器
    public Sprite farmIcon; // 农田图标
    public Sprite plantIcon; // 种植图标
    public Sprite waterIcon; // 浇水图标
    public Sprite harvestIcon; // 收获图标
    public Sprite LandSprite;//选中图标
    public Sprite restIcon;

    public int harvestCount;
    public enum FieldAction { None, Plant, Water, Farm, Harvest,Rest }
    public FieldAction currentAction = FieldAction.None;   

    [Tooltip("需要保存的数据")]
    [Header("需要保存的数据")]
    public float totalFertility;
    public float totalOrganic;
    public float totalCondition;
    public float totalWater;
    public float temCondition;
    public bool isFarm; // 是否已经是农田
    public bool needWater; // 是否需要浇水
    public bool needHarvest; // 是否需要收获
    public bool needPlant;
    public bool needRest;
    public bool canFarm;
    public bool canPlant;
    public bool isDry;
    public bool isUnlocked;

    public bool isHeal;
    void Start()
    {
        totalWater = 50f;
    }
    public void Update()
    {
        if (totalWater > 0)
        {
            needWater = false;
            isDry = false;
        }
        else if(totalWater <= 0 && !isDry)
        {
            needWater = true;
            isDry = true;
            Dry();      
        }
        if(needHarvest)
        {
            needIconRenderer.sprite = harvestIcon;
            if (House.Instance.isShrink)
            {
                House.Instance.houseRender.sprite = House.Instance.harvest;
            }
        }
        if (harvestCount >= 3)
        {
            needIconRenderer.sprite = restIcon;
            needRest = true;
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            LandManager.Instance.OnFarmLandClicked(this);
            if (!isUnlocked)
                return;
            if (!isFarm)
            {
                isFarm = true;
                currentAction = FieldAction.Farm;
                LandManager.Instance.AddTask(this);
                iconAnim.Play("farmIcon");
            }
            else if (isFarm)
            {
                if (needPlant && !LandManager.Instance.holdingSeed)
                {
                    needPlant = false;
                    currentAction = FieldAction.Plant;
                    LandManager.Instance.AddTask(this);
                    iconAnim.Play("plantIcon");
                }
                if (needWater)
                {
                    needWater = false;
                    currentAction = FieldAction.Water;
                    LandManager.Instance.AddTask(this);
                    iconAnim.Play("waterIcon");
                }
                else if (needHarvest)
                {
                    needHarvest = false;
                    currentAction = FieldAction.Harvest;
                    LandManager.Instance.AddTask(this);
                    iconAnim.Play("harvestIcon");
                }
                else if (needRest)
                {
                    needRest = false;
                    currentAction = FieldAction.Rest;
                    LandManager.Instance.AddTask(this);
                    iconAnim.Play("restIcon");
                    harvestCount = 0;
                }
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            ResetPosition();
        }
    }
    public void Plant()
    {
        foreach (SeedPoint seedPoint in seedPoints)
        {
            seedPoint.FarmerPlant();
            seedPoint.isPlant = true;
        }
        ChickIfHeal();
    }
    public void Harvest()
    {
        harvestCount++;
        foreach (SeedPoint seedPoint in seedPoints)
        {
            seedPoint.Harvest();
        }
    }
    public void Water()
    {
        totalWater = 100;
        needIconRenderer.sprite = null;
        foreach (SeedPoint seedPoint in seedPoints)
        {
            seedPoint.isWater = true;
        }
    }
    public void Rest()
    {
        if (totalFertility < 0)
        {
            totalFertility += 10;
        }
        if (totalOrganic < 0)
        {
            totalOrganic += 10;
        }
        needIconRenderer.sprite = null;
       
    }
    public void Dry()
    {
        totalWater = 0;
        needWater = true;
        needIconRenderer.sprite = waterIcon;
        if (House.Instance.isShrink)
        {
            House.Instance.houseRender.sprite = House.Instance.water;
        }
        foreach (SeedPoint seedPoint in seedPoints)
        {
            seedPoint.isWater = false;
        }
    }
    public void ChickNeedPlant()
    {
        foreach (SeedPoint seedPoint in seedPoints)
        {
            needIconRenderer.sprite = null;
            needPlant = false;
            if (seedPoint.isPrePlant)
            {
                needIconRenderer.sprite = plantIcon;
                if (House.Instance.isShrink)
                {
                    House.Instance.houseRender.sprite = House.Instance.plant;
                }
                needPlant = true;
                break;
            }
        }
    }
    public void ChickIfHeal()
    {
        foreach(SeedPoint seedPoint in seedPoints)
        {
            totalFertility += seedPoint.fert;
            totalOrganic += seedPoint.organic;           
        }
       
        if (totalFertility >= 0 && totalOrganic >= 0)
        {
           isHeal = true;
        }
        else
        {
            isHeal = false;
        }
        totalFertility = 0f; totalOrganic =0f;
    }
    public void UpdateTotalCondition(SeedPoint seedPoint)
    {
        if(isHeal)
        {
                totalCondition += seedPoint.condition;
        }
        else
        {
                totalCondition -= seedPoint.condition;
        }
    }
    public void MoveUp()
    {
        // 向上移动0.5单位
        land.transform.position = transform.position + Vector3.up * 0.5f;
    }
    public void ResetPosition()
    {
        land.transform.position = transform.position;
    }
    public void SaveData(string saveSlot)
    {
        int seedPointIndex = 0;
        foreach (SeedPoint seedPoint in seedPoints)
        {
            string seedPointKey = $"{saveSlot}_SeedPoint_{seedPointIndex}";
            seedPoint.SaveData(seedPointKey);
            seedPointIndex++;
        }
        ES3.Save(saveSlot + "_harvestCount",harvestCount);
        ES3.Save(saveSlot + "_totalFertility", totalFertility);
        ES3.Save(saveSlot + "_totalOrganic", totalOrganic);
        ES3.Save(saveSlot + "_totalCondition", totalCondition);
        ES3.Save(saveSlot + "_totalWater", totalWater);
        ES3.Save(saveSlot + "_temCondition", temCondition);
        ES3.Save(saveSlot + "_isFarm", isFarm);
        ES3.Save(saveSlot + "_needWater", needWater);
        ES3.Save(saveSlot + "_needHarvest", needHarvest);
        ES3.Save(saveSlot + "_needPlant", needPlant);
        ES3.Save(saveSlot + "_canFarm", canFarm);
        ES3.Save(saveSlot + "_canPlant", canPlant);
        ES3.Save(saveSlot + "_isDry", isDry);
        ES3.Save(saveSlot + "_isUnlocked", isUnlocked);
    }
    public void LoadData(string saveSlot)
    {
        int seedPointIndex = 0;
        foreach (SeedPoint seedPoint in seedPoints)
        {
            string seedPointKey = $"{saveSlot}_SeedPoint_{seedPointIndex}";
            seedPoint.LoadData(seedPointKey);
            seedPointIndex++;
        }
        harvestCount = ES3.Load<int>(saveSlot + "_harvestCount", 0);
        totalFertility = ES3.Load<float>(saveSlot + "_totalFertility", 0f);
        totalOrganic = ES3.Load<float>(saveSlot + "_totalOrganic", 0f);
        totalCondition = ES3.Load<float>(saveSlot + "_totalCondition", 0f);
        totalWater = ES3.Load<float>(saveSlot + "_totalWater", 0f);
        temCondition = ES3.Load<float>(saveSlot + "_temCondition", 0f);
        isFarm = ES3.Load<bool>(saveSlot + "_isFarm", false);
        needWater = ES3.Load<bool>(saveSlot + "_needWater", false);
        needHarvest = ES3.Load<bool>(saveSlot + "_needHarvest", false);
        needPlant = ES3.Load<bool>(saveSlot + "_needPlant", false);
        canFarm = ES3.Load<bool>(saveSlot + "_canFarm", false);
        canPlant = ES3.Load<bool>(saveSlot + "_canPlant", false);
        isDry = ES3.Load<bool>(saveSlot + "_isDry", false);
        isUnlocked = ES3.Load<bool>(saveSlot + "_isUnlocked", false);
    }
}
