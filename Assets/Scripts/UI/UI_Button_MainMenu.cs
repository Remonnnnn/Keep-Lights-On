using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button_MainMenu : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerClickHandler
{
    public Color highlightColor;
    public Color pressColor;
    public Color defaultColor;

    public Vector3 defaultPos;
    public float moveDis;
    private void Start()
    {
        defaultColor = text.color;
        defaultPos=text.rectTransform.localPosition;
    }
    public TextMeshProUGUI text;
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highlightColor;
        text.transform.DOLocalMoveX(defaultPos.x+moveDis,.3f);
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ì§Æð");
        text.color = defaultColor;
        text.transform.DOLocalMoveX(defaultPos.x, .3f);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("°´ÏÂ");
        AudioManager.instance.PlaySFX("UI_Click", null);
        text.color = pressColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = defaultColor;
        text.transform.DOLocalMoveX(defaultPos.x, .5f);
    }



}
