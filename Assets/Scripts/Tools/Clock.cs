using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Enemy")
        {
            ClockTrigger();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.tag=="Enemy")
        {
            StartCoroutine(StopClockTrigge());
        }
    }

    private void ClockTrigger()
    {
        if(!AudioManager.instance.CheckIsPlaying("ClockTrigger"))
        {
            AudioManager.instance.PlaySFX("ClockTrigger", transform);
        }

    }

    IEnumerator StopClockTrigge()
    {
        yield return new WaitForSeconds(1);
        StopClockTrigger();
    }

    private void StopClockTrigger()
    {
        AudioManager.instance.StopSFX("ClockTrigger");
    }
}
