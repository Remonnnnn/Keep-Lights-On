using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class ItemObject
{
    public ItemData itemData;
    public int num = 1;

    public ItemObject(ItemData itemData)
    {
        this.itemData = itemData;
    }


}
