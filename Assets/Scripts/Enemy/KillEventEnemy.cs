using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Kind
{
    Move,
    Idle
}

public class KillEventEnemy : MonoBehaviour
{
    public Kind kind;
    public float moveSpeed = 15;
    public int facingDir=1;
    public string deadReason;
    public string audioPath;

    public void Start()
    {
        if(facingDir==-1)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }

    public void Update()
    {
        if (GameManager.instance.player.isOver)
        {
            return;
        }
        if (kind == Kind.Move)
        {
            float d = moveSpeed * Time.deltaTime;
            Vector3 moveDir = new Vector2(facingDir * d, 0);
            transform.position += moveDir;
        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.instance.player.isOver)
        {
            return;
        }
        if(collision.tag=="Player")
        {
            GetComponent<Animator>().speed = 0;

            GameManager.instance.OverGame(deadReason,audioPath);
        }
        else if(collision.tag=="Ground" && kind==Kind.Move)
        {
            Destroy(gameObject);
        }
    }
}
