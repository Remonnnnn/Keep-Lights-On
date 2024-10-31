using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine, string animBoolName) : base(enemy, stateMachine, animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = enemy.idleTime;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        enemy.SetZeroVelocity();

        if (enemy.IsPlayerDetected() && !GameManager.instance.player.isHide)
        {
            stateMachine.ChangeState(enemy.runState);
        }


        if (stateTimer<0)
        {
            stateMachine.ChangeState(enemy.walkState);
        }
    }
}
