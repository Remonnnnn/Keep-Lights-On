using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;

    public PlayerInputController inputControl;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        inputControl = new PlayerInputController();
    }

    private void OnEnable()
    {
        inputControl.Enable();
    }

    public void SetUIInput(bool _vis)
    {
        if (_vis)
        {
            Debug.Log("Active UI_Input");
            inputControl.UI.Enable();
        }
        else
        {
            Debug.Log("Ban UI_Input");
            inputControl.UI.Disable();
        }
    }

    public void ActiveGamePlay()
    {
        inputControl.GamePlay.Enable();
    }

    public void BanUI()
    {
        inputControl.UI.Option.Disable();
        inputControl.UI.Bag.Disable();
    }

    public void ActiveUI()
    {
        inputControl.UI.Option.Enable();
        inputControl.UI.Bag.Enable();
    }

    public void BanGamePlay()
    {
        inputControl.GamePlay.Disable();
    }

    //public void ActiveChoose()
    //{
    //    inputControl.UI.Choose1.Enable();
    //    inputControl.UI.Choose2.Enable();
    //}

    //public void BanChoose()
    //{
    //    inputControl.UI.Choose1.Disable();
    //    inputControl.UI.Choose2.Disable();
    //}


    public void SetMotorGamepad(float _duration, float lowFrequency, float highFrequency)
    {
        if (Gamepad.current == null)
        {
            return;
        }
        Gamepad.current.SetMotorSpeeds(lowFrequency, highFrequency);
        StartCoroutine(StopMotor(_duration));
    }

    IEnumerator StopMotor(float _duration)
    {
        yield return new WaitForSeconds(_duration);
        Gamepad.current.SetMotorSpeeds(0f, 0f);
    }
}
