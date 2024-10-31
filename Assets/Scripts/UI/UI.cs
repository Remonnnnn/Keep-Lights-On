using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("对话框UI")]
    public UI_SpeakTip speakTip;

    [Header("理智UI")]
    public GameObject sanUI;
    Tweener nowSantween;

    [Header("手电筒UI")]
    public GameObject flashlightUI;

    [Header("背包UI")]
    public GameObject inventoryUI;
    public UI_ItemToolTip itemToolTip;

    [Header("设置UI")]
    public GameObject optionUI;
    public Toggle CRTToggle;

    [Header("时间UI")]
    public UI_Time timeUI;

    [Header("谜题UI")]
    public GameObject puzzleUI;
    public bool isInPuzzle;
    public 
    void Start()
    {
        InputManager.instance.inputControl.UI.Bag.started += OpenBag;
        InputManager.instance.inputControl.UI.Option.started += OpenOption;

        if(!GameManager.instance.CheckCRT())
        {
            CRTToggle.isOn = false;
        }

    }

    void Update()
    {

    }

    public void ShakeSanUI()
    {
        if(nowSantween==null || !nowSantween.IsPlaying())
        {
            nowSantween = sanUI.transform.DOShakePosition(1, new Vector3(3, 3, 0));
        }
    }

    public void ShowUI_Flashlight()
    {
        flashlightUI.SetActive(true);
    }

    public void OpenBag(InputAction.CallbackContext obj)
    {
        if (isInPuzzle || optionUI.activeSelf)
        {
            return;
        }

        if (!GameManager.instance.isPaused)
        {
            GameManager.instance.PauseGame(true);
            inventoryUI.SetActive(true);
            AudioManager.instance.PlaySFX("UI_OpenUI", null);
        }
        else
        {
            GameManager.instance.PauseGame(false);
            inventoryUI.SetActive(false);
        }
    }

    public void OpenOption(InputAction.CallbackContext obj)
    {
        if (isInPuzzle || inventoryUI.activeSelf)
        {
            return;
        }

        if (!GameManager.instance.isPaused)
        {
            GameManager.instance.PauseGame(true);
            optionUI.SetActive(true);
            AudioManager.instance.PlaySFX("UI_OpenUI", null);
        }
        else
        {
            GameManager.instance.PauseGame(false);
            optionUI.SetActive(false);
        }
    }

    public void OpenPuzzle()
    {
        if (!GameManager.instance.isPaused)
        {
            isInPuzzle = true;
            GameManager.instance.PauseGame(true);
            puzzleUI.SetActive(true);
            AudioManager.instance.PlaySFX("UI_OpenUI", null);
        }
        else
        {
            Debug.Log("退出");
            isInPuzzle = false;
            GameManager.instance.PauseGame(false);
            puzzleUI.SetActive(false);
            Debug.Log("退出成功");
        }
    }
}
