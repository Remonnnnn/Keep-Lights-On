using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricBox : EventCollider
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if(Inventory.instance.Check(needItemData))
        {
            isHold = true;
        }
        base.OnTriggerEnter2D(collision);
    }


}
