using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class KillObject : MonoBehaviour
{
    private Animator anim;
    private float stayTimer=0;
    [SerializeField] private float sanTrigeer = 30;
    [SerializeField] private string deadReason;
    [SerializeField] private string audioPath;
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        stayTimer = 0;
        if(collision.tag=="Player" && GameManager.instance.player.nowSan<sanTrigeer)
        {
            GameManager.instance.player.flashlight.GetComponent<BlinkLight>().ControllBlink(3);
        }
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        stayTimer += Time.deltaTime;
        if(stayTimer>=3 && GameManager.instance.player.nowSan < 30)
        {
            GameManager.instance.player.gameObject.SetActive(false);
            anim.enabled = true;
            GameManager.instance.OverGame(deadReason, audioPath);
        }
    }
}
