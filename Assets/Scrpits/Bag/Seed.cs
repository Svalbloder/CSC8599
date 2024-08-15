using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Seed : MonoBehaviour
{
    public int _index;//序号
    public SpriteRenderer _renderer;
    public bool seedCanPlant;
    void OnTriggerStay2D(Collider2D other)
    {
        SeedPoint seedPoint = other.GetComponent<SeedPoint>();
        if(seedPoint != null)
        {
            if (seedPoint.farmLand.canPlant && seedCanPlant)
            {
                if (_index == 0 || !seedPoint.isPlant)
                {
                    // 更新SeedPoint的属性
                    seedPoint.PlantSeed(this);
                }
            }
            if (seedPoint != null && seedPoint.farmLand.canPlant)
            {
                seedPoint.SetTransparency(1f);
            }
        }
       
    }
    void OnTriggerExit2D(Collider2D other)
    {
        SeedPoint seedPoint = other.GetComponent<SeedPoint>();
        FarmLand farmLand = other.GetComponent<FarmLand>();
        if (seedPoint != null)
        {
            if(seedPoint.isPrePlant)
            {
                seedPoint.SetTransparency(0.6f);
            }
            else
            {
                seedPoint.SetTransparency(0f);
            }
            
        }
    }
}
