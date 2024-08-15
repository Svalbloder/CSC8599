using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconAnim : MonoBehaviour
{
    public FarmLand farmland;
    public void IconFinish()
    {
        farmland.needIconRenderer.sprite = null;
    }
}
