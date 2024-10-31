using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : Entity
{
    [Header("输入设置")]
    public Vector2 InputDirection;

    [Header("移动设置")]
    public float moveSpeed = 10f;
    public float runSpeed = 15f;
    public float energy;
    public float maxEnergy;

    [Header("San值设置")]
    public float maxSan = 100;
    public float nowSan = 100;
    public float loseSanSpeed = 2;
    public float recoverSanSpeed = 1;
    public System.Action onSanChange;
    public bool isBeginSanDead;
    public float sanDeadDuration = 5;
    Coroutine sanDeadCor;

    public ItemData recoverItemData;
    public float pillCanRecover = 50;

    [Header("特殊状态")]
    public bool isLight = false;
    public bool isOver;
    public bool isHide;
    public bool isDark = false;

    [Header("手电状态")]
    public AnimatorOverrideController anim_Flashlight;
    public GameObject flashlight;
    public bool isOpenFlashlight;
    public float electrictyTime = 5;
    public float electrictyTimer = 0;
    public int electrictyNum = 8;
    public Image batteryImage;
    public float batteryTime = 3f;
    public ItemData battery;

    [Header("其他道具设置")]
    public GameObject clock;
    public ItemData clockItemData;

    #region States

    public PlayerStateMachine stateMachine {  get; private set; }

    public PlayerIdleState idleState;
    public PlayerMoveState moveState;
    public PlayerRunState runState;

    #endregion
    protected override void Awake()
    {
        base.Awake();

        stateMachine=new PlayerStateMachine();

        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        runState = new PlayerRunState(this, stateMachine, "Run");
    }

    protected override void Start()
    {
        base.Start();

        stateMachine.Initialize(idleState);
        onFlipped += FlipBatterySlider;
        onSanChange += CheckSanDead;
    }


    protected override void Update()
    {
        base.Update();

        if(isOver)
        {
            SetZeroVelocity();
            return;
        }

        if (GameManager.instance.isGameBegin)
        {

            if (!isLight && !isOpenFlashlight)
            {
                if (!isDark)
                {
                    isDark = true;
                    GameManager.instance.ChangeDarkVolume(true);
                }
                LoseSan();
            }
            else
            {
                if (isDark)
                {
                    isDark = false;
                    GameManager.instance.ChangeDarkVolume(false);
                }
                if (isLight)
                {
                    RecoverSan();
                }

            }
            if (isOpenFlashlight)
            {
                FlashlightUseLogic();
            }
        }
        stateMachine.currentState.Update();
    }

    public void RecoverSan(float san)
    {
        if(nowSan==100)
        {
            return;
        }

        nowSan += san;
        nowSan=Mathf.Min(nowSan, 100);
        onSanChange();
    }

    public void LoseSan(float san)
    {
        if (nowSan == 0)
        {
            return;
        }
        //在黑暗中，随时间流失San值
        nowSan -= san;
        nowSan = Mathf.Max(nowSan, 0);
        onSanChange();
    }
    private void RecoverSan()
    {
        if(nowSan==100)
        {
            return;
        }
        nowSan += recoverSanSpeed * Time.deltaTime;
        nowSan = Mathf.Min(nowSan, 100);
        onSanChange();
    }

    private void LoseSan()
    {
        if(nowSan==0)
        {
            return;
        }
        //在黑暗中，随时间流失San值
        nowSan -= loseSanSpeed * Time.deltaTime;
        nowSan = Mathf.Max(nowSan, 0);
        onSanChange();
    }

    public void CheckSanDead()
    {
        if(nowSan==0 && !isBeginSanDead)
        {
            isBeginSanDead = true;
            sanDeadCor=StartCoroutine(BeginSanDead());
        }
        else if(nowSan>=5 && isBeginSanDead)
        {
            isBeginSanDead= false;
            StopCoroutine(sanDeadCor);
            sanDeadCor = null;
            GameManager.instance.StopSanDead();
        }
    }

    public IEnumerator BeginSanDead()
    {
        float Timer = 0;
        while(Timer<sanDeadDuration)
        {
            Timer += Time.deltaTime;
            GameManager.instance.ChangeSanDead(Timer / sanDeadDuration);
            yield return null;
        }
        GameManager.instance.OverGame("理智崩溃", "Kill_SanDead");
    }

    private void FlashlightUseLogic()
    {
        electrictyTimer += Time.deltaTime;
        if (electrictyTimer >= electrictyTime)
        {
            electrictyTimer = 0;
            electrictyNum--;
            if (electrictyNum == 0)
            {
                CloseFlashlight();
            }
            EventManager.instance.EventTrigger<int>("ElectricityNumChange", electrictyNum);
        }
    }

    public void RecoverFlashlight()
    {
        Inventory.instance.Remove(battery);
        electrictyTimer = 0;
        electrictyNum = 8;
        EventManager.instance.EventTrigger<int>("ElectricityNumChange", electrictyNum);
    }

    public void FlipBatterySlider()
    {
        batteryImage.transform.Rotate(0, 180, 0);
    }

    public override void Die()
    {
        base.Die();
        anim.speed = 0;
        isOver = true;
        Debug.Log("Die");

    }
    public void GetFlashLight()
    {
        anim.runtimeAnimatorController = anim_Flashlight;
        InputManager.instance.inputControl.GamePlay.Flashlight.started += UseFlashlight;
        UIManager.instance.ui.ShowUI_Flashlight();
        UIManager.instance.ShowTip(2);
    }

    public void UseFlashlight(InputAction.CallbackContext obj)
    {
        if(flashlight.activeSelf)
        {
            CloseFlashlight();
        }
        else if(electrictyNum>0)
        {
            OpenFlashlight();
        }
    }

    public void CloseFlashlight()
    {
        AudioManager.instance.PlaySFX("UseFlashlight", transform);
        flashlight.SetActive(false);
        isOpenFlashlight = false;
    }

    public void OpenFlashlight()
    {
        AudioManager.instance.PlaySFX("UseFlashlight", transform);
        flashlight.SetActive(true);
        isOpenFlashlight = true;
    }

    public void SetClock()
    {
        Instantiate(clock, this.transform.position-new Vector3(0,.75f),Quaternion.identity);
        Inventory.instance.Remove(clockItemData);
        GameManager.instance.isBusy = false;
    }

    public void Hide()
    {
        if (!isHide)
        {
            CloseFlashlight();
            isHide = true;
            sr.color = Color.clear;
            InputManager.instance.inputControl.GamePlay.Move.Disable();
            InputManager.instance.inputControl.GamePlay.Flashlight.Disable();
            GameManager.instance.ChangeHideVolume(.6f);
        }
        else
        {
            isHide = false;
            sr.color = Color.white;
            InputManager.instance.inputControl.GamePlay.Move.Enable();
            InputManager.instance.inputControl.GamePlay.Flashlight.Enable();
            GameManager.instance.ChangeHideVolume(GameManager.instance.defaultHideIntensity);
        }

    }

}
