using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class House : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public static House Instance;
    bool isClick;
    public bool isShrink;
    public GameObject Dialog;
    public Animator anim;
    public bool onDrag;
    public SpriteRenderer houseRender;
    public Sprite water;
    public Sprite farm;
    public Sprite plant;
    public Sprite harvest;
    public GameObject button;
    public GameObject Pannel;
    //×ó¼ü´ò¿ªÃæ°å
    //ÓÒ¼üÕÛµþ
    //ÕÛµþ×´Ì¬ÏÂ×ó¼üÒÆ¶¯£¬ÓÒ¼üÕ¹¿ª
    private void Awake()
    {
        // µ¥ÀýÄ£Ê½
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Update()
    {
        if (onDrag)
        {
            Vector3 _worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _worldPosition.z = 0;
            gameObject.transform.position = new Vector2(_worldPosition.x, _worldPosition.y);
        }
    }
        public void OnPointerDown(PointerEventData eventData)
        {
        if (LandManager.Instance.holdingSeed)
        {
            return;
        }
        SetTransparency(1);
        if (eventData.button == PointerEventData.InputButton.Left &&!isShrink)
        {
            isClick = !isClick;
            LandManager.Instance.CalculateTotal();
            Dialog.SetActive(isClick);
            button.SetActive(isClick);
            Pannel.SetActive(isClick);
        }
        else if(eventData.button == PointerEventData.InputButton.Right &&!isShrink)
        {
          
            anim.Play("houseClose");    
            isShrink = true;
            isClick = false;
            Dialog.SetActive(isClick);
            button.SetActive(isClick);
            Pannel.SetActive(isClick);
        }
        else if (eventData.button == PointerEventData.InputButton.Right && isShrink)
        {
            anim.Play("houseOpen");
            isShrink = false;
            onDrag = false;
            houseRender.sprite = null;
        }
        else if(eventData.button == PointerEventData.InputButton.Left && isShrink)
        {
            onDrag = true;     
        }
        else if (eventData.button == PointerEventData.InputButton.Left && isShrink && onDrag)
        {
            onDrag = false;
        }
    }
    public void OnDrag()
    {
        if (isShrink)
        {
            onDrag = true;
        }
        
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        onDrag = false;
    }
    public void SetTransparency(float alpha)
    {
        Color color = houseRender.color;
        color.a = Mathf.Clamp01(alpha);
        houseRender.color = color;
    }
    
}
