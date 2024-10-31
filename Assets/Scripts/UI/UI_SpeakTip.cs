using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class UI_SpeakTip : MonoBehaviour
{
    private TextMeshProUGUI textMeshProUGUI;
    private float SpeakSpeed = .1f;

    private bool isSkip=false;
    private float Timer = 0;
    private void Awake()
    {
        textMeshProUGUI=GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        Timer-=Time.deltaTime;
        if (Input.GetKeyUp(KeyCode.Space) && !isSkip)
        {
            Timer = 0;
            isSkip= true;
        }
    }
    public IEnumerator StartShowTip(string content)
    {
        StoryManager.instance.isSpeak=true;
        gameObject.SetActive(true);
        string str = "";
        for (int i = 0; i < content.Length; i++)
        {
            if (content[i] != '/')
            {
                str += content[i];
            }
            else
            {
                Debug.Log(str);
                isSkip = false;
                AudioManager.instance.PlaySFX("Speak", GameManager.instance.player.transform);
                float duration = str.Length * SpeakSpeed;
                var t = DOTween.To(() => string.Empty, value => textMeshProUGUI.text = value, str, duration).SetEase(Ease.Linear);
                t.SetOptions(true);
                Timer = duration;
                while(Timer>0)
                {
                    yield return null;
                }
                AudioManager.instance.StopSFX("Speak");
                if(!isSkip)
                {
                    Timer = 1;
                    while(Timer>0)
                    {
                        yield return null;
                    }
                }
                else
                {
                    t.TogglePause();
                    textMeshProUGUI.text = "";
                }
                str = "";
            }
        }
        textMeshProUGUI.text = " ";
        gameObject.SetActive(false);
        StoryManager.instance.isSpeak = false;
    }
}
