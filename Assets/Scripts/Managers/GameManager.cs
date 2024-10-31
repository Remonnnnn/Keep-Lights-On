using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using static UnityEngine.Rendering.DebugUI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //[Header("初始相关")]
    //public PlayableAsset beginAsset;

    [Header("玩家相关")]
    public Player player;
    public bool isBusy = false;
    public bool isGameBegin=false;



    [Header("敌人相关")]
    public Enemy enemy;

    [Header("游戏内事件设置")]
    public Transform holdEventColliderParent;
    public Transform interactObjectParent;
    public List<EventCollider> holdEventColliderList = new List<EventCollider>();
    public List<GameObject> interactObjectList= new List<GameObject>();

    public List<ItemData> itemDataList=new List<ItemData>();
    public List<int> itemDataNum=new List<int>();
    public Dictionary<ItemData,int> itemDic=new Dictionary<ItemData,int>();

    public GameObject lightParent;
    public List<LightCollider> lightColliders = new List<LightCollider>();
    public List<LightCollider> closeLightColliders=new List<LightCollider>();
    private float lightTimer = 0;
    public float lightTime = 10;
    public bool canLight = true;
    public GameObject electricalBox;
    public GameObject TV;



    [Header("画面设置")]
    public Volume volume;
    public float defaultHideIntensity = .34f;

    public float minSanIntensity;

    public float defaultCRTIntensity = .2f;
    public float maxCRTIntensity = .4f;

    public PolygonCollider2D beginCollider;
    public CinemachineConfiner2D nowconfiner;

    public CinemachineCollisionImpulseSource inpulse;

    public bool isPaused;
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

    }
    void Start()
    {

    }


    void Update()
    {
        if(player==null)
        {
            return;
        }

        if(player.isDark)
        {
            UpdateDarkVolume();
        }
        if(isGameBegin && canLight)
        {
            lightTimer += Time.deltaTime;
            if (lightTimer > lightTime)
            {
                lightTimer = 0;
                RandomCloseLight();
            }
        }
    }



    public bool isPlayerBusy() => isBusy;
    public void SetBeginConfiner() => nowconfiner.m_BoundingShape2D = beginCollider;
    public void SetConfiner(PolygonCollider2D collider)=>nowconfiner.m_BoundingShape2D=collider;

    public void BeginGame()
    {
        Debug.Log("BeginGame");
        SetBeginConfiner();

        //画面设置初始
        ResetSetting();
        player.onSanChange += UpdateSanVolume;

        //游戏事件设置
        EventManager.instance.AddEventListener("厨房留言", BeginNight);

        LightCollider[] lights = lightParent.GetComponentsInChildren<LightCollider>();
        foreach (LightCollider light in lights)
        {
            lightColliders.Add(light);
        }

        InputManager.instance.BanGamePlay();
        InputManager.instance.BanUI();
        InitItem();
        UIManager.instance.BeginGame();
    }


    #region 游戏初始化
    public void BeginNight()
    {
        isGameBegin = true;
        UIManager.instance.ShowTip(1);
    }

    public void InitItem()
    {
        for(int i = 0; i < itemDataList.Count;i++)
        {
            itemDic.Add(itemDataList[i], itemDataNum[i]);
        }
        for(int i=0;i<interactObjectParent.childCount;i++)
        {
            interactObjectList.Add(interactObjectParent.GetChild(i).gameObject);
        }
        for (int i = 0; i < holdEventColliderParent.childCount; i++)
        {
            holdEventColliderList.Add(holdEventColliderParent.GetChild(i).GetComponent<EventCollider>());
        }

        for (int i = 0; i < interactObjectList.Count; i++)
        {
            int index = Random.Range(0, itemDataList.Count);
            while (itemDataNum[index]<=0)
            {
                index= Random.Range(0, itemDataList.Count);
            }
            itemDataNum[index]--;
            interactObjectList[i].GetComponent<InteractObject>().SetItemData(itemDataList[index]);
        }
        for (int i = 0; i < holdEventColliderList.Count; i++)
        {
            int index = Random.Range(0, itemDataList.Count);
            while (itemDataNum[index] <= 0)
            {
                index = Random.Range(0, itemDataList.Count);
            }
            itemDataNum[index]--;
            holdEventColliderList[i].SetItemData(itemDataList[index]);
        }
    }



    public void RandomCloseLight()
    {
        if (lightColliders.Count==0)
        {
            return;
        }
        int index = Random.Range(0, lightColliders.Count - 1);
        lightColliders[index].CloseLight();
        closeLightColliders.Add(lightColliders[index]);
        lightColliders.RemoveAt(index);
    }

    #endregion


    public void OverGame(string endingReason,string audioPath)
    {
        AudioManager.instance.StopAllSFX();

        GameManager.instance.player.Die();
        InputManager.instance.BanUI();
        InputManager.instance.BanGamePlay();

        AudioManager.instance.StopNowBGM();
        if(endingReason!=null)
        {
            AudioManager.instance.PlaySFX("Kill_SFX", null);
        }
        if (audioPath != null)
        {
            AudioManager.instance.PlaySFX(audioPath, null);
        }

        UIManager.instance.OverGame(endingReason);
    }


    public void PauseGame(bool _pause)
    {
        if (_pause)
        {
            Time.timeScale = 0;
            if (!StoryManager.instance.isSpeak && !player.isHide)
            {
                InputManager.instance.BanGamePlay();
            }
            if(StoryManager.instance.isSpeak)
            {
                AudioManager.instance.StopSFX_Short("Speak");
            }
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;
            if(!StoryManager.instance.isSpeak && !player.isHide)
            {
                InputManager.instance.ActiveGamePlay();
            }
            if(StoryManager.instance.isSpeak)
            {
                AudioManager.instance.PlaySFX_Short("Speak");
            }
            isPaused = false;
        }
    }

    //仅在主菜单调用的方法

    public void StartGame()
    {
        StartCoroutine(StartGameCor());
    }

    IEnumerator StartGameCor()
    {
        AudioManager.instance.StopNowBGM();
        AudioManager.instance.PlaySFX("UI_GameStart",null);
        //UIManager.instance.MenuUI.SetActive(false);
        //UIManager.instance.directer.playableAsset = beginAsset;
        //UIManager.instance.directer.Play();
        UIManager.instance.FadeIn();
        yield return new WaitForSeconds(3);
        SceneLoader.instance.Load("MainGame");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    //回到主菜单
    public void ReturnToMenu()
    {
        ResetSetting();
        SceneLoader.instance.Load("MainMenu");
    }




    #region 画面视觉效果
    public void ResetSetting()
    {
        ChangeHideVolume(defaultHideIntensity);
        ChanageSanVolume(0);
        ChangeSanDead(0);
    }

    //开关CRT

    public bool CheckCRT()
    {
        if (volume.sharedProfile.TryGet<FilmGrain>(out var film))
        {
            if (film.active)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        return false;

    }


    public void SetCRT(bool value)
    {
        if (volume.sharedProfile.TryGet<FilmGrain>(out var film) && volume.sharedProfile.TryGet<PaniniProjection>(out var panini))
        {
            Debug.Log("find");
            if (film.active)
            {
                AudioManager.instance.PlaySFX("UI_Click", null);
                film.active = false;
                panini.active = false;
            }
            else
            {
                AudioManager.instance.PlaySFX("UI_Click", null);
                film.active = true;
                panini.active = true;
            }
        }
    }

    public void ChangeHideVolume(float value)
    {
        if (volume.sharedProfile.TryGet<Vignette>(out var vignette))
        {
            Debug.Log("find");
            vignette.intensity.value = value;
        }
    }

    public void UpdateSanVolume()
    {
        ChanageSanVolume(1 - player.nowSan / player.maxSan);
    }
    public void ChanageSanVolume(float value)
    {
        if (volume.sharedProfile.TryGet<ChromaticAberration>(out var chromaticAberration))
        {
            Debug.Log("find");
            chromaticAberration.intensity.value = minSanIntensity + value * (1 - minSanIntensity);
        }
    }

    public void ChangeSanDead(float value)
    {
        if (volume.sharedProfile.TryGet<DepthOfField>(out var depthOfField))
        {
            Debug.Log("find");

            depthOfField.focalLength.value = 1 + value * 200;
        }
    }

    public void StopSanDead()
    {
        if (volume.sharedProfile.TryGet<DepthOfField>(out var depthOfField))
        {
            Debug.Log("find");

            depthOfField.focalLength.value = 1;
        }
    }

    public void ChangeDarkVolume(bool _vis)
    {
        if (volume.sharedProfile.TryGet<FilmGrain>(out var filmGrain))
        {
            if (_vis)
            {
                filmGrain.intensity.value = maxCRTIntensity;
            }
            else
            {
                filmGrain.intensity.value = defaultCRTIntensity;
            }

        }
    }

    public void UpdateDarkVolume()
    {
        inpulse.GenerateImpulse();
        UIManager.instance.ui.ShakeSanUI();
    }


    #endregion


    public void ChanagePos(Vector3 pos, PolygonCollider2D confiner)
    {
        player.transform.position = pos;
        nowconfiner.m_BoundingShape2D = confiner;
    }


    public void Blockout()
    {
        canLight = false;
        for (int i = 0; i < lightColliders.Count; i++)
        {
            closeLightColliders.Add(lightColliders[i]);
        }
        for (int i=0;i<lightColliders.Count;i++)
        {
            lightColliders[i].CloseLight();
        }
        electricalBox.SetActive(true);
        TV.SetActive(false);
        EventManager.instance.EventTrigger("Blockout");
    }

    public void Power()
    {
        canLight = true;
        EventManager.instance.EventTrigger("Blockout");
    }

}
