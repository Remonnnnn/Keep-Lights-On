using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public enum EventState
{
    Interact,
    Hide
}


public class EventCollider : MonoBehaviour
{
    public EventState eventState=EventState.Interact;

    [Header("交互设置")]
    public string interactSFX;
    public GameObject interactIcon;
    public string InteractSpeak;

    [Header("获得物品设置")]
    public ItemData itemData;

    [Header("需要物品触发")]
    public ItemData needItemData;
    public string changeSpeak;

    [Header("关联事件设置")]
    public List<GameObject> nextColliders=new List<GameObject>();

    [Header("其他设置")]
    public bool isOnce;
    public bool isHold;
    public float Timer = 0;
    private float FinishTime = 3f;
    public Image FinishSlider;

    protected Player nowPlayer;

    protected virtual void Start()
    {
        interactIcon.transform.localScale = new Vector3(0.5f / transform.localScale.x, 0.5f / transform.localScale.y, 0.5f / transform.localScale.z);
    }


    private void Update()
    {
        if (nowPlayer != null && isHold)
        {
            if (InputManager.instance.inputControl.GamePlay.Interact.ReadValue<float>() == 1f && !GameManager.instance.isBusy)
            {
                if(!AudioManager.instance.CheckIsPlaying("Interact_Hold"))
                {
                    AudioManager.instance.PlaySFX("Interact_Hold", null);
                }
                interactIcon.SetActive(false);
                Timer += Time.deltaTime;
                FinishSlider.fillAmount = Timer / FinishTime;
            }
            else if (Timer != 0)
            {
                interactIcon.SetActive(true);
                ResetHoldSlider();
            }

            if (Timer >= FinishTime)
            {
                Interact();
                isOnce = true;
                ResetHoldSlider();
            }
        }
    }

    public void ResetHoldSlider()
    {
        if (FinishSlider != null)
        {
            Timer = 0;
            FinishSlider.fillAmount = 0;
            AudioManager.instance.StopSFX("Interact_Hold");
        }
    }

    public void Interact(InputAction.CallbackContext obj) => Interact();
    public virtual void Interact()
    {
        if(GameManager.instance.isBusy)
        {
            return;
        }

        GameManager.instance.isBusy = true;
        if(eventState==EventState.Interact)
        {
            BeforeInteract();
            StartCoroutine(IneteractIE());
        }
        else if(eventState==EventState.Hide)
        {
            if (interactSFX != "")
            {
                Debug.Log("播放sfx");
                AudioManager.instance.PlaySFX(interactSFX, null);
            }
            else
            {
                AudioManager.instance.PlaySFX("Interact", null);
            }
            nowPlayer.Hide();
            GameManager.instance.isBusy = false;
        }

    }

    public virtual void BeforeInteract()
    {
        if (interactSFX != "")
        {
            Debug.Log("播放sfx");
            AudioManager.instance.PlaySFX(interactSFX, null);
        }
        else
        {
            AudioManager.instance.PlaySFX("Interact", null);
        }
        InputManager.instance.BanGamePlay();
        interactIcon.SetActive(false);

    }

    public IEnumerator IneteractIE()
    {
        string str = "";
        bool canGet = false;
        if(needItemData!=null && Inventory.instance.Check(needItemData))
        {
            Inventory.instance.Remove(needItemData);
            InteractSpeak = changeSpeak;
            canGet = true;
            isOnce = true;
        }
        if(needItemData==null)
        {
            canGet = true;
        }
        if (InteractSpeak != null)
        {
            if (itemData != null && canGet)
            {
                str = StoryManager.instance.storyDic[InteractSpeak];
                StoryManager.instance.storyDic[InteractSpeak] += "获得： " + "【 " + itemData.itemName + " 】/";
            }
            yield return StartCoroutine(StoryManager.instance.DelaySpeak(InteractSpeak));
        }
        if (itemData != null && canGet)
        {
            StoryManager.instance.storyDic[InteractSpeak] = str;
            Inventory.instance.Add(itemData);
        }
        if(canGet)
        {
            if (nextColliders.Count > 0)
            {
                for (int i = 0; i < nextColliders.Count; i++)
                {
                    nextColliders[i].SetActive(true);
                }
            }
        }
        InteractLogic();
    }

    public virtual void InteractLogic()
    {
        InputManager.instance.ActiveGamePlay();

        if(InteractSpeak!=null)
        {
            EventManager.instance.EventTrigger(InteractSpeak);
        }
        if (isOnce)
        {
            Destroy(gameObject);
        }
        else
        {
            interactIcon.SetActive(true);
        }
        GameManager.instance.isBusy = false;
    }

    public virtual void SetItemData(ItemData _itemData)
    {
        itemData=_itemData;
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            nowPlayer=collision.GetComponent<Player>();
            interactIcon.SetActive(true);
            if (!isHold)
            {
                InputManager.instance.inputControl.GamePlay.Interact.started += Interact;
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
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
}
