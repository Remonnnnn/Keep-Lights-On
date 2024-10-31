using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Playables;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("游戏开屏UI")]
    public Image iconImage;
    public Sprite eyeImage;
    public Sprite earImage;

    [Header("主菜单UI")]
    public GameObject MenuUI;
    public PlayableDirector directer;
    private Tweener tweener;

    [Header("UI管理")]
    public UI ui;

    [Header("黑屏UI")]
    public float fadeTime = 3f;
    public Image fadeImage;
    public TextMeshProUGUI pressTip;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI endingText;
    public TextMeshProUGUI endingTop;
    public TextMeshProUGUI thanksText;
    public Image endingPic;

    [Header("游戏内UI")]
    public GameObject tipUI;//提示类
    public bool isShowTip;
    public bool haveShowFlashlight;
    public bool haveShowBag;
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

    //仅在刚进入游戏时调用提示

    public void ShowPlayGameTip()
    {
        StartCoroutine(StartShowPlayGameTip());
    }

    IEnumerator StartShowPlayGameTip()
    {
        SceneLoader.instance.isTip=true;
        iconImage.sprite = eyeImage;
        iconImage.DOFade(1, fadeTime - 1);
        endingText.DOFade(1, fadeTime - 1);
        yield return new WaitForSeconds(fadeTime);
        iconImage.DOFade(0, fadeTime - 1);
        endingText.DOFade(0, fadeTime - 1);
        yield return new WaitForSeconds(fadeTime);

        iconImage.sprite = earImage;
        endingText.text = "建议佩戴耳机游玩";
        iconImage.DOFade(1, fadeTime - 1);
        endingText.DOFade(1, fadeTime - 1);
        yield return new WaitForSeconds(fadeTime);
        iconImage.DOFade(0, fadeTime - 1);
        endingText.DOFade(0, fadeTime - 1);
        yield return new WaitForSeconds(fadeTime);

        FadeOutQucik();
    }






    public void BeginGame()
    {
        StartCoroutine(StartBeginGame());
    }

    IEnumerator StartBeginGame()
    {
        yield return new WaitForSeconds(1);

        AudioManager.instance.PlayBGM("BGM_MainGame");

        endingText.text = StoryManager.instance.startText;
        endingText.DOFade(1, fadeTime);
        yield return new WaitForSeconds(fadeTime + 1);

        isShowTip = true;
        ShowTip(pressTip);
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        isShowTip = false;
        ShowTip(pressTip);

        endingText.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        timeText.DOFade(1,fadeTime);
        AudioManager.instance.PlaySFX("Player_FootStep", null);
        yield return new WaitForSeconds(fadeTime-1);
        AudioManager.instance.StopSFX("Player_FootStep");
        yield return new WaitForSeconds(1);
        AudioManager.instance.PlaySFX("Interact_Door", null);
        yield return new WaitForSeconds(1);
        FadeOut();
        timeText.DOFade(0,fadeTime);
        yield return new WaitForSeconds(fadeTime);

        StoryManager.instance.Speak("开场");
        yield return new WaitForSeconds(.5f);
        while(StoryManager.instance.isSpeak)
        {
            yield return null;
        }
        yield return new WaitForSeconds(.5f);
        yield return StartCoroutine(StartShowTip(0));

    }

    public void OverGame(string endingReason)
    {
        StartCoroutine(StartOverGame(endingReason));
    }

    IEnumerator StartOverGame(string endingReason)
    {
        yield return new WaitForSeconds(2);
        if(endingReason!=null)
        {
            AudioManager.instance.PlaySFX("GameOver_Bad", null);
        }
        else
        {
            AudioManager.instance.PlaySFX("GameOver_Good",null);
        }


        FadeIn();
        yield return new WaitForSeconds(fadeTime+1);
        GameManager.instance.ResetSetting();

        if(endingReason!=null)
        {
            endingTop.text = "你死了";
            endingTop.color = Color.red;
            endingText.text = "在 " + ui.timeUI.GetTime() + " 死于 " + endingReason;
            endingText.rectTransform.localPosition = new Vector3(0, -100, 0);
            thanksText.DOFade(1, fadeTime);
            endingTop.DOFade(1, fadeTime);
            endingText.DOFade(1, fadeTime);
            yield return new WaitForSeconds(fadeTime + 1);
        }
        else
        {
            endingTop.text = "你活到了6点！";
            endingTop.color = Color.yellow;
            endingText.text= StoryManager.instance.endText;
            endingText.rectTransform.localPosition = new Vector3(0, -100, 0);
            thanksText.DOFade(1, fadeTime);
            endingTop.DOFade(1, fadeTime);
            endingText.DOFade(1, fadeTime);
            yield return new WaitForSeconds(fadeTime + 1);  
        }


        isShowTip = true;
        ShowTip(pressTip);
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        isShowTip = false;
        ShowTip(pressTip);

        endingTop.DOFade(0, fadeTime);
        endingText.DOFade(0, fadeTime);
        thanksText.DOFade(0, fadeTime);

        yield return new WaitForSeconds(fadeTime + 1);

        GameManager.instance.ReturnToMenu();

    }



    public void ShowTip(TextMeshProUGUI text)
    {
        if(isShowTip)
        {
            pressTip.DOFade(1, 1).SetLoops(-1, LoopType.Yoyo).SetUpdate(true);
            text.gameObject.SetActive(true);
        }
        else
        {
            pressTip.DOPause();
            pressTip.alpha = 0;
            text.gameObject.SetActive(false);
        }

    }

    public void ShowTip(int index) => StartCoroutine(StartShowTip(index));
    //指引
    IEnumerator StartShowTip(int index)
    {
        InputManager.instance.BanGamePlay();
        InputManager.instance.BanUI();

        AudioManager.instance.PlaySFX("UI_Tip", null);
        isShowTip = true;
        tipUI.transform.localScale = new Vector3(.01f, .01f, .01f);
        tipUI.SetActive(true);
        tipUI.transform.GetChild(index).gameObject.SetActive(true);
        tipUI.transform.DOScale(new Vector3(1, 1, 1), .5f).SetUpdate(true);
        yield return new WaitForSeconds(.5f);
        ShowTip(pressTip);
        GameManager.instance.PauseGame(true);
        while (!Input.GetKeyDown(KeyCode.Space))
        {
            yield return null;
        }
        isShowTip = false;
        ShowTip(pressTip);
        tipUI.transform.GetChild(index).gameObject.SetActive(false);
        tipUI.transform.DOScale(new Vector3(.01f, .01f, .01f), .5f).SetUpdate(true);
        GameManager.instance.PauseGame(false);
        yield return new WaitForSeconds(.5f);
        tipUI.SetActive(false);
        InputManager.instance.ActiveGamePlay();
        InputManager.instance.ActiveUI();
    }
    public void FadeIn()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.DOFade(1, fadeTime);
    }

    public void FadeOut() => StartCoroutine(StratFadeOut());

    public void FadeInQucik()
    {
        fadeImage.gameObject.SetActive(true);
        Color tmp = fadeImage.color;
        tmp.a = 1f;
        fadeImage.color = tmp;
    }
    public void FadeOutQucik()
    {
        Color tmp = fadeImage.color;
        tmp.a = 0f;
        fadeImage.color = tmp;
        fadeImage.gameObject.SetActive(false);
    }

    IEnumerator StratFadeOut()
    {
        fadeImage.DOFade(0, fadeTime);
        yield return new WaitForSeconds(fadeTime);
        fadeImage.gameObject.SetActive(false);
    }

    public void ShowUI(Image image)
    {
        if (!tweener.IsActive())
        {
            tweener = image.transform.DOLocalMoveY(0, .5f);
        }
    }

    public void CloseUI(Image image)
    {
        if (!tweener.IsActive())
        {
            tweener = image.transform.DOLocalMoveY(1080, .5f);
        }
    }

}
