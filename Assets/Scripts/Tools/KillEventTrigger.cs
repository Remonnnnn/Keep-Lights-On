using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEventTrigger : MonoBehaviour
{
    public GameObject prefab;
    public Vector3 initPos;

    public void KillTrigger()
    {
        if(GameManager.instance.player.nowSan<50 && Random.Range(0,100)<50)
        {
            Instantiate(prefab, initPos, Quaternion.identity);
            GameManager.instance.player.flashlight.GetComponent<BlinkLight>().ControllBlink(3);
        }
    }
}
