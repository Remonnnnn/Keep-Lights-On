using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button_Menu : MonoBehaviour, IPointerEnterHandler,IPointerExitHandler,IPointerDownHandler,IPointerClickHandler
{
    public Color highlightColor;
    public Color pressColor;
    public Color defaultColor;
    public string clickAudioPath = "UI_Click";
    private void Start()
    {
        defaultColor = text.color;
    }
    public TextMeshProUGUI text;
    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = highlightColor;
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Ì§Æð");
        text.color = defaultColor;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("°´ÏÂ");
        AudioManager.instance.PlaySFX(clickAudioPath, null);
        text.color = pressColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = defaultColor;
    }



}
