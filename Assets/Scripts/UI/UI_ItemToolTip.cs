using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private void OnEnable()
    {
        ClearToolTip();
    }
    public void ShowToolTip(ItemData item)
    {
        itemNameText.text = item.itemName;
        itemDescription.text = item.itemDescription;
    }

    public void ClearToolTip()
    {
        itemNameText.text = "";
        itemDescription.text = "";
    }

}
