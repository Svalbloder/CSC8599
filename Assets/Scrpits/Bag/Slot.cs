using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
public class Slot : MonoBehaviour, IPointerDownHandler
{
    // 只放能显示的物品
    public CropData slot_item; // 物品
    public Image slot_image; // 图片
    public Text slot_cost; // 花费
    public GameObject prefab; // 预制体
    private bool onDrag = false;
    public Seed seed;
    public SlotText slotText;

    public void Start()
    {
        slotText = GetComponentInParent<SlotText>();
    }
    public void Update()
    {
        if (onDrag)
        {
            if (seed != null) // 检查 seed 是否为空
            {
                LandManager.Instance.holdingSeed = true;
                Vector3 _worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                _worldPosition.z = 0;
                seed.gameObject.transform.position = new Vector2(_worldPosition.x, _worldPosition.y);
                seed.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0f;
            }
        }
        if (onDrag && Input.GetMouseButtonDown(1))
        {
            if (seed != null)
            {
                Destroy(seed.gameObject);
                seed = null;
            }
            onDrag = false;
            LandManager.Instance.holdingSeed = false;
            LandManager.Instance.currentSeed = null;
        }

        if (onDrag && Input.GetMouseButtonDown(0))
        {
            if (seed != null)
            {
                seed.seedCanPlant = true;
            }
        }

        if (onDrag && Input.GetMouseButtonUp(0))
        {
            if (seed != null)
            {
                seed.seedCanPlant = false;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GrabSeed();
        }
    }

    public void GrabSeed()
    {
        if (LandManager.Instance.currentSeed != null)
        {
            Destroy(LandManager.Instance.currentSeed.gameObject); // 销毁当前种子对象
            LandManager.Instance.currentSeed = null; // 清空 currentSeed 引用
        }
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        seed = Instantiate(prefab, worldPosition, Quaternion.identity).GetComponent<Seed>();
        onDrag = true;
        LandManager.Instance.currentSeed = seed;       
        // 初始化种子属性
        seed._index = slot_item._index;
        seed._renderer.sprite = slot_item._seedSprite;
        SetTextAndColor(slotText.fert, slot_item._fert);
        SetTextAndColor(slotText.organic, slot_item._organic);
        SetTextAndColor(slotText.condition, slot_item._condition);
        SetTextAndColor(slotText.water, slot_item._water);
    }
    void SetTextAndColor(TMP_Text textComponent, float value)
    {
        textComponent.text = value.ToString();
        textComponent.color = value >= 0 ? Color.green : Color.red;
    }
}
