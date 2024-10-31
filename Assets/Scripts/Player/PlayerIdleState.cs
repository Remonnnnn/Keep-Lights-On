using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerIdleState : PlayerState
{
    private float batteryTimer;
    public PlayerIdleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {

    }

    public override void Enter()
    {
        base.Enter();
        InputManager.instance.inputControl.GamePlay.RecoverSan.started += RecoverSan;
        InputManager.instance.inputControl.GamePlay.Clock.started += SetClock;
    }

    public override void Exit()
    {
        base.Exit();
        ResetBattery();
        InputManager.instance.inputControl.GamePlay.RecoverSan.started -= RecoverSan;
        InputManager.instance.inputControl.GamePlay.Clock.started -= SetClock;
    }

    public override void Update()
    {
        base.Update();

        if(player.energy<player.maxEnergy)
        {
            if (!AudioManager.instance.CheckIsPlaying("Player_Breathe"))
            {
                AudioManager.instance.PlaySFX("Player_Breathe", player.transform);
            }
            player.energy += Time.deltaTime / 3;
        }
        else
        {
            if(AudioManager.instance.CheckIsPlaying("Player_Breathe"))
            {
                AudioManager.instance.StopSFX("Player_Breathe");
            }
        }

        if(InputManager.instance.inputControl.GamePlay.Battery.ReadValue<float>()==1f && Inventory.instance.Check(player.battery) && player.electrictyNum<8)
        {
            if(!AudioManager.instance.CheckIsPlaying("RecoverFlashlight"))
            {
                AudioManager.instance.PlaySFX("RecoverFlashlight", player.transform);
            }
            GameManager.instance.isBusy = true;
            batteryTimer += Time.deltaTime;
            player.batteryImage.fillAmount = batteryTimer / player.batteryTime;
            if(batteryTimer>=player.batteryTime)
            {
                player.RecoverFlashlight();
                batteryTimer = 0;
            }
        }
        else if(player.batteryImage.fillAmount!=0)
        {
            ResetBattery();
            GameManager.instance.isBusy = false;
        }

        if (xInput!=0)
        {
            stateMachine.ChangeState(player.moveState);
        }
    }

    private void ResetBattery()
    {
        AudioManager.instance.StopSFX("RecoverFlashlight");
        player.batteryImage.fillAmount = 0;
        batteryTimer = 0;
    }

    private void RecoverSan(InputAction.CallbackContext obj)
    {
        if(Inventory.instance.Check(player.recoverItemData) && !GameManager.instance.isBusy)
        {
            Inventory.instance.Remove(player.recoverItemData);
            AudioManager.instance.PlaySFX("RecoverSan", player.transform);
            player.RecoverSan(player.pillCanRecover);
        }
    }

    private void SetClock(InputAction.CallbackContext obj)
    {
        if(Inventory.instance.Check(player.clockItemData) && !GameManager.instance.isBusy && !player.isHide)
        {
            GameManager.instance.isBusy = true;
            AudioManager.instance.PlaySFX("SetClock", player.transform);
            player.SetClock();
        }
    }
}
