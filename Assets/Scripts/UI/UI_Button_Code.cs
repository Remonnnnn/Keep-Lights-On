using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Button_Code : MonoBehaviour
{
    public int nowNum = 0;
    public Button button1;
    public Button button2;
    public TextMeshProUGUI text;

    private void Start()
    {
        UpdateNum();
        button1.onClick.AddListener(() => Increase());
        button2.onClick.AddListener(() => Decrease());
    }

    public void Increase()
    {
        nowNum = (nowNum + 1) % 10;
        UpdateNum();
    }

    public void Decrease()
    {
        nowNum = (nowNum + 9) % 10;
        UpdateNum();
    }

    public void UpdateNum()
    {
        text.text=nowNum.ToString();
    }

    public void ResetNum()
    {
        nowNum=0;
        UpdateNum();
    }
}
