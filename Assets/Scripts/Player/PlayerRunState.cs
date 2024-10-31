using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : PlayerState
{
    public PlayerRunState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        AudioManager.instance.PlaySFX("Player_FootStep", player.transform);
        AudioManager.instance.StopSFX("Player_Breathe");
    }

    public override void Exit()
    {
        base.Exit();
        AudioManager.instance.StopSFX("Player_FootStep");
    }

    public override void Update()
    {
        base.Update();

        player.energy -= Time.deltaTime;

        player.SetVelocity(xInput * player.runSpeed, 0);


        if (xInput == 0)
        {
            stateMachine.ChangeState(player.idleState);
        }


        if (player.energy<=0 || InputManager.instance.inputControl.GamePlay.Run.ReadValue<float>() != 1f)
        {
            stateMachine.ChangeState(player.moveState);
        }

    }
}
