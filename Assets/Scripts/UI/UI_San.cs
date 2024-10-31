using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_San : MonoBehaviour
{
    [SerializeField]private Slider sanSlider;
    private Player player;
    void Start()
    {
        
        player = GameManager.instance.player;
        player.onSanChange += UpdateSanSlider;
    }

    public void UpdateSanSlider()
    {
        sanSlider.maxValue = player.maxSan;
        sanSlider.value = player.nowSan;
        if(player.nowSan<20)
        {
            if(!AudioManager.instance.CheckIsPlaying("San_low2"))
            {
                AudioManager.instance.PlaySFX("San_low2", player.transform);
            }
        }
        else if(player.nowSan<50)
        {
            if(!AudioManager.instance.CheckIsPlaying("San_low"))
            {
                AudioManager.instance.PlaySFX("San_low", player.transform);
            }
        }
        else
        {
            if(AudioManager.instance.CheckIsPlaying("San_low"))
            {
                AudioManager.instance.StopSFX("San_low");
            }

            else if(AudioManager.instance.CheckIsPlaying("San_low2"))
            {
                AudioManager.instance.StopSFX("San_low2");
            }
        }

    }
}
