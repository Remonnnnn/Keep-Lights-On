using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    //当前背包物品
    public List<ItemObject> inventory = new List<ItemObject>();
    public Dictionary<ItemData, ItemObject> inventoryDic = new Dictionary<ItemData, ItemObject>();

    [Header("背包UI")]
    [SerializeField] private Transform InventorySlotParent;
    private UI_ItemSlot[] inventoryItemSlot;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        inventoryItemSlot = InventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();

        UpdateSlotUI();
    }

    public void Add(ItemData itemData)
    {
        if(Check(itemData))
        {
            inventoryDic[itemData].num++;
        }
        else
        {
            ItemObject item = new ItemObject(itemData);
            inventory.Add(item);
            inventoryDic.Add(itemData, item);
        }
        UpdateSlotUI();
    }

    public void Remove(ItemData itemData)
    {
        if (Check(itemData))
        {
            if (inventoryDic[itemData].num>1)
            {
                inventoryDic[itemData].num--;
            }
            else
            {
                inventory.Remove(inventoryDic[itemData]);
                inventoryDic.Remove(itemData);
            }
        }
        UpdateSlotUI();
    }

    public bool Check(ItemData itemData)
    {
        return inventoryDic.ContainsKey(itemData);
    }

    public void UpdateSlotUI()
    {
        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            inventoryItemSlot[i].CleanSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
    }

    public void ClearInventory()
    {
        inventory.Clear();
        inventoryDic.Clear();
        UpdateSlotUI();
    }
}
