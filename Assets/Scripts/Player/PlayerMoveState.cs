using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        InputManager.instance.inputControl.GamePlay.Run.started += Run;
        AudioManager.instance.PlaySFX("Player_FootStep", player.transform);
    }

    public override void Exit()
    {
        base.Exit();

        InputManager.instance.inputControl.GamePlay.Run.started -= Run;
        AudioManager.instance.StopSFX("Player_FootStep");
    }

    public override void Update()
    {
        base.Update();

        if (player.energy < player.maxEnergy)
        {
            if (!AudioManager.instance.CheckIsPlaying("Player_Breathe"))
            {
                AudioManager.instance.PlaySFX("Player_Breathe", player.transform);
            }
            player.energy += Time.deltaTime / 4;
        }
        else
        {
            if (AudioManager.instance.CheckIsPlaying("Player_Breathe"))
            {
                AudioManager.instance.StopSFX("Player_Breathe");
            }
        }

        player.SetVelocity(xInput * player.moveSpeed, 0);

        if (xInput == 0)
        {
            if(stateTimer<0)
            {
                stateTimer = .1f;
            }
            else if(stateTimer<.05f)
            {
                stateMachine.ChangeState(player.idleState);
            }
        }



    }

    public void Run(InputAction.CallbackContext obj)
    {
        if (player.energy > 0)
        {
            stateMachine.ChangeState(player.runState);
        }
    }
}
