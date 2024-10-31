using UnityEngine;

public class EnemyRunState : EnemyState
{
    public int moveDir;
    public EnemyRunState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("×·»÷×´Ì¬");
        AudioManager.instance.PlaySFX("Killer_Run", enemy.transform);
        enemy.isRun = true;
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX("Killer_Run");
        enemy.isRun = false;
    }

    public override void Update()
    {
        base.Update();

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.runTime;

            if (enemy.IsPlayerDetected().distance < enemy.deadDistance)
            {
                AudioManager.instance.StopSFX("Killer_Run");
                GameManager.instance.OverGame(enemy.deadReason,enemy.audioPath);
            }
        }
        else
        {
            if (stateTimer < 0 || Vector2.Distance(GameManager.instance.player.transform.position, enemy.transform.position)>10)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }


        if (GameManager.instance.player.transform.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else if (GameManager.instance.player.transform.position.x < enemy.transform.position.x)
        {
            moveDir = -1;
        }

        enemy.SetVelocity(enemy.runSpeed * moveDir, rb.velocity.y);
    }
}
