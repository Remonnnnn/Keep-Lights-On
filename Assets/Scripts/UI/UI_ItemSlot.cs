using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class UI_ItemSlot : MonoBehaviour,IPointerEnterHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemNum;

    public Color defaultColor;
    public ItemObject item;

    void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => OnPointerClick());
    }

    public void UpdateSlot(ItemObject itemObject)
    {
        item = itemObject;

        if (item != null)
        {
            itemImage.color = Color.white;
            itemImage.sprite = item.itemData.itemIcon;
            if(itemObject.num>1)
            {
                itemNum.text = itemObject.num.ToString();
            }
            else
            {
                itemNum.text = "";
            }

        }
    }

    public void CleanSlot()
    {
        itemImage.sprite = null;
        itemImage.color = defaultColor;
        itemNum.text = "";
        item = null;
    }

    public void OnPointerClick()
    {
        if(item==null)
        {
            return;
        }

        AudioManager.instance.PlaySFX("UI_ButtonClick",null);
        UIManager.instance.ui.itemToolTip.ShowToolTip(item.itemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (item != null)
        //{
        //    AudioManager.instance.PlaySFX_UI(0);
        //}


    }
}
