using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightCollider : EventCollider
{
    public GameObject connectedLight;

    public override void InteractLogic()
    {
        base.InteractLogic();

        if(!GameManager.instance.canLight)
        {
            return;
        }

        if(connectedLight.activeSelf)
        {
            connectedLight.SetActive(false);
            if (GameManager.instance.lightColliders.Contains(this))
            {
                GameManager.instance.lightColliders.Remove(this);
            }
            if(!GameManager.instance.closeLightColliders.Contains(this))
            {
                GameManager.instance.closeLightColliders.Add(this);
            }
        }
        else
        {
            connectedLight.SetActive(true);
            if (!GameManager.instance.lightColliders.Contains(this))
            {
                GameManager.instance.lightColliders.Add(this);
            }
            if (GameManager.instance.closeLightColliders.Contains(this))
            {
                GameManager.instance.closeLightColliders.Remove(this);
            }

        }
    }

    public void CloseLight()
    {
        connectedLight.SetActive(false);
    }
}
