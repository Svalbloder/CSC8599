using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LevelManager : MonoBehaviour
{
    public GainSeed gainSeed;
    public int nextUnlockIndex = 0; // 下一个待解锁的索引
    public int landIndex = 0;
    public TurnPage turnPage;
    // 存储每个阈值对应的解锁条件
    public float[] unlockThresholds = new float[] { 0f, 50f, 100f, 150f, 200f, 250f, 400f, 800f };

    private void Update()
    {
        float allCondition = LandManager.Instance.allCondition;

        // 检查是否达到下一个解锁条件
        if (nextUnlockIndex < unlockThresholds.Length && allCondition >= unlockThresholds[nextUnlockIndex])
        {
            ExecuteUnlockAction(nextUnlockIndex);
            nextUnlockIndex++;
        }
    }

    // 根据索引执行对应的解锁操作
    private void ExecuteUnlockAction(int index)
    {
        switch (index)
        {
            case 0:
                turnPage.maxPageCount++;
                UnlockNextFarmLand();               
                break;
            case 1:
                turnPage.maxPageCount++;
                UnlockNextFarmLand();
                UnlockNextFarmLand();
                gainSeed.AddNewItem();
                gainSeed.AddNewItem();
                break;
            case 2:
                turnPage.maxPageCount++;            
                UnlockNextFarmLand();
                UnlockNextFarmLand();
                gainSeed.AddNewItem();
                gainSeed.AddNewItem();
                break;
            case 3:
                gainSeed.AddNewItem();
                UnlockNextFarmLand();
                break;
            case 4:
                gainSeed.AddNewItem();
                UnlockNextFarmLand();
                break;
            case 5:
                gainSeed.AddNewItem();
                UnlockNextFarmLand();
                break;
            default:
               
                break;
        }
    }
    private void UnlockNextFarmLand()
    {
        if (landIndex < LandManager.Instance.farmLandList.Count)
        {
            LandManager.Instance.farmLandList[landIndex].isUnlocked = true;
            if (!LandManager.Instance.farmLandList[landIndex].isFarm)
            {
                LandManager.Instance.farmLandList[landIndex].needIconRenderer.sprite = LandManager.Instance.farmLandList[landIndex].farmIcon;
                if (House.Instance.isShrink)
                {
                    House.Instance.houseRender.sprite = House.Instance.farm;
                }
            }
            landIndex++;
        }
    }
}
