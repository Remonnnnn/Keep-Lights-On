using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Time : MonoBehaviour
{
    public int currentTimeHour = 11;
    public int currentTimeMinute = 0;
    public float minuteDuration = 2f;
    public float minuteTimer = 0;
    public TextMeshProUGUI text;
    private void Update()
    {
        if(GameManager.instance.player.isOver)
        {
            return;
        }

        minuteTimer += Time.deltaTime;
        if(minuteTimer >= minuteDuration)
        {
            minuteTimer = 0;
            currentTimeMinute++;
            if (currentTimeMinute == 60)
            {
                AudioManager.instance.PlaySFX("Time_Hour", null);
                currentTimeMinute = 0;
                currentTimeHour = ((currentTimeHour + 1) % 12);
            }
            else
            {
                AudioManager.instance.PlaySFX("Time_Minute", null);
            }
            UpdateTime();
        }
    }

    public void UpdateTime()
    {
        string hour = currentTimeHour.ToString();
        string minute = currentTimeMinute.ToString();
        if(hour.Length==1)
        {
            hour = "0" + hour;
        }
        if (minute.Length == 1) 
        {
            minute = "0" + minute;
        }
        text.text = hour + "£º" + minute;
        EventManager.instance.EventTrigger(text.text);
    }

    public string GetTime()=>text.text;
}
