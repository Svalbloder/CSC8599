using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedPoint : MonoBehaviour
{
    [Header("默认数据")]
    private float growTime = 120f; // 完全生长所需的时间（秒）
    private float needWaterTime = 20f;
    public FarmLand farmLand;
    public SpriteRenderer plantSprite;

    [Header("编号")]
    public int index;

    [Header("植物图片")] 
    public Sprite prePlant;  
    public Sprite planting;
    public Sprite seedling;   
    public Sprite halfGrown;   
    public Sprite mature;

    [Header("植物种植数据")]
    public int stage = 0;   
    public float time = 0f;   
    public float waterTime = 0f;  
    public float speed;

    [Header("植物土地影响")]
    public float fert; 
    public float organic; 
    public float condition; 
    public float water;

    [Header("判断")]
    public bool isWater;
    public bool isPlant;
    public bool isGrown;
    public bool isPrePlant;

    void Update()
    {
        speed = LandManager.Instance.GrowSpeed;
        if (isPlant)
        {
            if (isWater&&!isGrown)
            {
                time += Time.deltaTime * speed;
                waterTime += Time.deltaTime * speed;
                if (waterTime >= needWaterTime)
                {
                    waterTime = 0f;
                    farmLand.totalWater += water;
                }
                // 检查是否该进入下一个生长阶段
                if (stage == 1 && time >= growTime / 3)
                {
                    stage++;
                    Stage();

                }
                else if (stage == 2 && time >= 2 * growTime / 3)
                {
                    stage++;
                    Stage();
                }
                else if (stage == 3 && time >= 3 * growTime / 3)
                {
                    stage++;
                    Stage();
                }
            }      
        }
      
    }
    void Stage()
    {
        SetTransparency(1);
        switch (stage)
        {
            case 0:
                plantSprite.sprite = farmLand.LandSprite;
                SetTransparency(0);
                break;
            case 1:
                plantSprite.sprite = seedling;
                break;
            case 2:           
                plantSprite.sprite = planting;
                UpdateSoil(farmLand);
                farmLand.UpdateTotalCondition(this);
                break;
            case 3:              
                plantSprite.sprite = halfGrown;
                UpdateSoil(farmLand);
                farmLand.UpdateTotalCondition(this);
                break;
            case 4:               
                plantSprite.sprite = mature;
                isGrown = true;
                farmLand.needHarvest = true;
                UpdateSoil(farmLand);
                farmLand.UpdateTotalCondition(this);
                break;
        }
    }
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    void UpdateSoil(FarmLand farmLand)
    {
        farmLand.totalFertility += fert;
        farmLand.totalOrganic += organic;
        if (farmLand.totalFertility >= 100f)
        {
            farmLand.totalFertility = 100f;
        }
        if (farmLand.totalOrganic >= 100f)
        {
            farmLand.totalOrganic = 100f;
        }
    }
    public void SetTransparency(float alpha)
    {
        Color color = plantSprite.color;
        color.a = Mathf.Clamp01(alpha);
        plantSprite.color = color;
    }
    public void FarmerPlant()
    {
        SetTransparency(1);
        stage++;
        Stage();
    }
    public void PlantSeed(Seed seed)
    {
        index = seed._index;
        LoadSprite();
        plantSprite.sprite = prePlant;
        if (seed._index == 0)
        {
            isPrePlant = false;
        }
        else
        {
            isPrePlant = true;
            
        }
        farmLand.ChickNeedPlant();
    }
    public void Harvest()
    {
        time = 0f;
        waterTime = 0f;
        fert = 0f;
        organic = 0f;
        condition = 0f;
        water = 0f;
        index = 0;
        stage = 0;
        isPlant = false;
        isGrown = false;
        SetTransparency(0f);
        plantSprite.sprite = farmLand.LandSprite;
        isPrePlant = false;
    }
    public void SaveData(string saveSlot)
    {
        SeedData data = new SeedData();
        data.seedIndex = index;
        data.stage = stage;
        data.time = time;
        data.waterTime = waterTime;
        data.speed = speed;
        data.fert = fert;
        data.organic = organic;
        data.condition = condition;
        data.water = water;
        data.isWater = isWater;
        data.isPlant = isPlant;
        data.isGrown = isGrown;
        data.isPrePlant = isPrePlant;
        ES3.Save(saveSlot, data);
    }
    public void LoadData(string saveSlot)
    {
        if (ES3.KeyExists(saveSlot))
        {
            SeedData data = ES3.Load<SeedData>(saveSlot);
            index = data.seedIndex;
            stage = data.stage;
            time = data.time;
            waterTime = data.waterTime;
            speed = data.speed;
            fert = data.fert;
            organic = data.organic;
            condition = data.condition;
            water = data.water;
            isWater = data.isWater;
            isPlant = data.isPlant;
            isGrown = data.isGrown;
            isPrePlant = data.isPrePlant;
        }
        LoadSprite();
        Stage();
        
    }
    public void LoadSprite()
    {
        CropData data = FindCropDataByIndex(index);
        if (data != null)
        {
            fert = data._fert;
            organic = data._organic;
            condition = data._condition;
            water = data._water;
            planting = data._plantingModel;
            seedling = data._seedlingModel;
            halfGrown = data._halfGrownModel;
            mature = data._matureModel;
            prePlant = data._fruit;
        }
    }
    public CropData FindCropDataByIndex(int index)
    {
        return LandManager.Instance.myBag.itemList.Find(crop => crop._index == index);
    }


}
