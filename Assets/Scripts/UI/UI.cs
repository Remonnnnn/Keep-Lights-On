using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [Header("�Ի���UI")]
    public UI_SpeakTip speakTip;

    [Header("����UI")]
    public GameObject sanUI;
    Tweener nowSantween;

    [Header("�ֵ�ͲUI")]
    public GameObject flashlightUI;

    [Header("����UI")]
    public GameObject inventoryUI;
    public UI_ItemToolTip itemToolTip;

    [Header("����UI")]
    public GameObject optionUI;
    public Toggle CRTToggle;

    [Header("ʱ��UI")]
    public UI_Time timeUI;

    [Header("����UI")]
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
            Debug.Log("�˳�");
            isInPuzzle = false;
            GameManager.instance.PauseGame(false);
            puzzleUI.SetActive(false);
            Debug.Log("�˳��ɹ�");
        }
    }
}
