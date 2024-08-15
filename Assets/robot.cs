using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class robot : MonoBehaviour
{
    public LandManager landManager;
    public void OnFarmComplete()
    {
       landManager.OnFarmComplete();
    }
    public void OnWaterComplete()
    {
        landManager.OnWaterComplete();
    }
    public void OnPlantComplete()
    {
        landManager.OnPlantComplete();
    }
    public void OnRestComplete()
    {
        landManager.OnRestComplete();
    }
    public void OnHarvestComplete()
    {
        landManager.OnHarvestComplete();
    }
    public void IfHeal()
    {
        landManager.ifHeal();
    }
}
