using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : EventCollider
{
    private Material outlineMaterial;
    protected override void Start()
    {
        base.Start();
        outlineMaterial = GetComponent<SpriteRenderer>()?.material;
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && GameManager.instance.isGameBegin)
        {
            ShowOutline();
            nowPlayer = collision.GetComponent<Player>();
            interactIcon.SetActive(true);
            if (!isHold)
            {
                InputManager.instance.inputControl.GamePlay.Interact.started += Interact;
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && GameManager.instance.isGameBegin)
        {
            CloseOutline();
            nowPlayer = null;
            interactIcon.SetActive(false);
            if (!isHold)
            {
                InputManager.instance.inputControl.GamePlay.Interact.started -= Interact;
            }
            else
            {
                ResetHoldSlider();
            }
        }
    }
    public override void BeforeInteract()
    {
        base.BeforeInteract();

        if (itemData.name == "手电筒" && !GameManager.instance.isGameBegin)
        {
            return;
        }

        if (GetComponent<SpriteRenderer>() != null )
        {
            GetComponent<SpriteRenderer>().color = Color.clear;
        }

    }
    public override void InteractLogic()
    {
        base.InteractLogic();
        if (itemData.name == "手电筒")
        {
            GameManager.instance.player.GetFlashLight();
        }
        Destroy(gameObject);
    }

    public override void SetItemData(ItemData _itemData)
    {
        itemData= _itemData;
        GetComponent<SpriteRenderer>().sprite = itemData.itemIcon;
        outlineMaterial = GetComponent<SpriteRenderer>()?.material;
        outlineMaterial.SetTexture("_MainTex", itemData.itemIcon.texture);
    }

    public void ShowOutline() => outlineMaterial.SetFloat("_Thickness", .06f);

    public void CloseOutline() => outlineMaterial.SetFloat("_Thickness", 0);
}
