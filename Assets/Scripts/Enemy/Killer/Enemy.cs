using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsGround;


    [Header("碰撞检测")]
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance = 2.5f;

    [Header("移动信息")]
    public float moveSpeed = 1.5f;
    public float runSpeed = 3;
    public float idleTime = 2;
    public float runTime = 7;
    public float SightDistance;
    public float deadDistance;
    private float defaultMoveSpeed;
    public bool isRun;

    private float useDoorTimer = 0;
    public float useDoorCooldown = 1;

    public string deadReason;
    public string audioPath;

    [Header("脚步设置")]
    public string audioFootStep = "Killer_FootStep";
    public float canHearRadius;
    public float runHeartSpeed = 1.5f;

    [Header("语音")]
    public List<string> audioSpeak=new List<string>();

    public EnemyStateMachine stateMachine;
    private float speakTimer = 0;
    public float speakCooldown = 10;

    #region State

    public EnemyIdleState idleState;
    public EnemyWalkState walkState;
    public EnemyRunState runState;

    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine=new EnemyStateMachine();

        defaultMoveSpeed= moveSpeed;

        idleState = new EnemyIdleState(this, stateMachine, "Idle");
        walkState = new EnemyWalkState(this, stateMachine, "Walk");
        runState = new EnemyRunState(this, stateMachine, "Run");

        
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        Init();
    }

    private void Init()
    {

    }

    protected override void Update()
    {
        if (GameManager.instance.player.isOver)
        {
            anim.speed = 0;
            SetZeroVelocity();
            AudioManager.instance.StopSFX(audioFootStep);
            return;
        }

        float distance = Vector2.Distance(GameManager.instance.player.transform.position,transform.position);

        if(distance<=canHearRadius)
        {
            speakTimer-=Time.deltaTime;
            if (!AudioManager.instance.CheckIsPlaying(audioFootStep))
            {
                float d = .4f + (1 - distance / canHearRadius) * .6f;
                if(stateMachine.currentState==runState)
                {
                    AudioManager.instance.PlaySFX(audioFootStep, transform, d, runHeartSpeed);
                }
                else
                {
                    AudioManager.instance.PlaySFX(audioFootStep, transform, d, 1);
                }
            }
            if(speakTimer<0)
            {
                speakTimer = speakCooldown;
                AudioManager.instance.PlaySFX(audioSpeak[Random.Range(0, audioSpeak.Count)], transform);
            }
        }

        useDoorTimer -= Time.deltaTime;

        base.Update();

        stateMachine.currentState.Update();
    }

    public override void Die()
    {
        base.Die();
    }


    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir,SightDistance , whatIsPlayer);//从wallcheck的位置发出一条射线
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, wallCheckDistance, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, canHearRadius);
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));//这里乘以facingDir是为了根据初始朝向调整wallcheck
    }

    public void ResetSelf(Vector2 vector2)
    {
        gameObject.transform.position= vector2;
        anim.speed = 1;
        stateMachine.ChangeState(idleState);
        Init();

    }

    public bool canUseDoor() => useDoorTimer <= 0;

    public void SetUseDoor() => useDoorTimer = useDoorCooldown;
}
