using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    [Header("开场/结束设置")]
    public string startText;

    public List<string> storyNameList= new List<string>();
    public List<string> storyList= new List<string>();

    public Dictionary<string,string> storyDic=new Dictionary<string,string>();

    public string endText;

    [Header("地图瓦片")]
    public Tilemap tile_background;
    public List<Vector3Int> pos=new List<Vector3Int>();
    public List<TileBase> tiles = new List<TileBase>();

    [Header("故事事件")]
    public GameObject kiiler;
    public Vector3 killerBirthPos;
    public GameObject scissorsObeject;

    public bool isSpeak = false;

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

    private void Start()
    {
        for (int i = 0; i < storyNameList.Count; i++)
        {
            storyDic.Add(storyNameList[i], storyList[i]);
        }

        //故事事件管管理
        EventManager.instance.AddEventListener("00：50", () => AudioManager.instance.PlaySFX("Killer_Knock", null));
        EventManager.instance.AddEventListener("01：00", KillerEvent);
        EventManager.instance.AddEventListener("02：59", () => AudioManager.instance.PlaySFX("ShortCircuit", null));
        EventManager.instance.AddEventListener("03：00", GameManager.instance.Blockout);
        EventManager.instance.AddEventListener("04：00", DarkerNight1);
        EventManager.instance.AddEventListener("05：00", DarkerNight2);
        EventManager.instance.AddEventListener("06：00", () => GameManager.instance.OverGame(null,null));

        //其他变换事件
        EventManager.instance.AddEventListener("电箱_使用螺丝刀", () => tile_background.SetTile(pos[0], tiles[0]));
        EventManager.instance.AddEventListener("电箱_使用保险丝", GameManager.instance.Power);

        EventManager.instance.AddEventListener("打开水龙头", () => tile_background.SetTile(pos[1], tiles[1]));

        GameManager.instance.BeginGame();
    }

    public void Speak(string name)
    {
        if (storyDic.ContainsKey(name))
        {
            StartCoroutine(UIManager.instance.ui.speakTip.StartShowTip(storyDic[name]));
        }
    }



    public IEnumerator DelaySpeak(string name)
    {
        if (storyDic.ContainsKey(name))
        {
            yield return StartCoroutine(UIManager.instance.ui.speakTip.StartShowTip(storyDic[name]));
        }
    }

    public void KillerEvent()
    {
        AudioManager.instance.PlaySFX("Interact_Door", null);
        GameManager.instance.enemy=Instantiate(kiiler, killerBirthPos, Quaternion.Euler(0,180,0)).GetComponent<Enemy>();
        scissorsObeject.SetActive(true);
    }

    public void DarkerNight1()
    {
        GameManager.instance.player.loseSanSpeed = GameManager.instance.player.loseSanSpeed + .25f;
        GameManager.instance.enemy.runSpeed = GameManager.instance.enemy.runSpeed + .1f;
    }

    public void DarkerNight2()
    {
        GameManager.instance.lightTime -= 4;
    }

}
