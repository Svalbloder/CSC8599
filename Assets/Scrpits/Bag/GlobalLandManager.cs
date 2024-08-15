using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // 引入UI命名空间

public class GlobalLandManager : MonoBehaviour
{
    //public List<LandManager> landManagers; // 存储所有LandManager对象的列表
    //public Slider totalConditionSlider; // 存储代表所有LandManager对象土壤总体情况总值的Slider对象

    //void Start()
    //{
    //    // 初始化LandManager列表
    //    landManagers = new List<LandManager>(GameObject.FindObjectsOfType<LandManager>());
    //}

    //void Update()
    //{
    //    // 计算所有LandManager对象的土壤总体情况总值
    //    float totalCondition = 0f;

    //    foreach (LandManager landManager in landManagers)
    //    {
    //        foreach (SeedPoint seedPoint in landManager.seedPoints)
    //        {
    //            totalCondition += seedPoint.condition;
    //        }
    //    }

    //    // 更新Slider的值
    //    totalConditionSlider.value = totalCondition;
    //}
}

