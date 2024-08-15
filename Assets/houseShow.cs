using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class houseShow : MonoBehaviour, IPointerDownHandler
{
    public SpriteRenderer houseRender;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            SetTransparency(1f);
        }
    }
    public void SetTransparency(float alpha)
    {
        Color color = houseRender.color;
        color.a = Mathf.Clamp01(alpha);
        houseRender.color = color;
    }
}
