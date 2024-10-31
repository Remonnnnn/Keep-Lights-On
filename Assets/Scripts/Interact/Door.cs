using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : EventCollider
{
    [Header("√≈…Ë÷√")]
    public Door posDoor;
    public Vector3 doorPos;
    public PolygonCollider2D confinerPos;
    public bool canOpen = true;
    public ItemData key;

    public override void BeforeInteract()
    {
        if(!canOpen && key!=null && Inventory.instance.Check(key))
        {
            Inventory.instance.Remove(key);
            UnLock();
        }
        base.BeforeInteract();
    }

    private void UnLock()
    {
        interactSFX = "Interact_Door";
        canOpen = true;
        InteractSpeak = null;
    }

    public override void InteractLogic()
    {
        if (canOpen)
        {
            GameManager.instance.ChanagePos(posDoor.doorPos,posDoor.confinerPos);

            if(!posDoor.canOpen)
            {
                posDoor.UnLock();
            }

            if (GetComponent<KillEventTrigger>() != null)
            {
                GetComponent<KillEventTrigger>().KillTrigger();
            }
        }

        base.InteractLogic();

    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (canOpen && collision.GetComponent<Enemy>()!=null && !collision.GetComponent<Enemy>().isRun && collision.GetComponent<Enemy>().canUseDoor() && RandomCheck())
        {
            collision.GetComponent<Enemy>().SetUseDoor();
            collision.gameObject.transform.position= posDoor.doorPos;
            AudioManager.instance.PlaySFX(InteractSpeak, null);
        }
    }

    public bool RandomCheck()
    {
        int t = Random.Range(0, 100);

        if (t < 50)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
