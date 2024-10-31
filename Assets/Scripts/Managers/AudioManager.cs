using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;



public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    //用于管理音频的对象池
    [Header("音频对象池设定")]
    private ObjectPool<GameObject> audioPool;
    [SerializeField]private GameObject audioPrefab;
    [SerializeField]private int defaultSize = 10;
    [SerializeField] private int maxSize = 20;

    private Dictionary<string, AudioInfo> hasLoadAudioInfoDic=new Dictionary<string, AudioInfo>();
    private Dictionary<string, GameObject> nowPlayingAudioDic=new Dictionary<string, GameObject>();
    

    [Header("BGM音频")]
    [SerializeField] private AudioSource bgmAudioSource;
    private string nowBgm;

    [Header("音频管理输出")]
    [SerializeField] private AudioMixerGroup sfx;
    [SerializeField] private AudioMixerGroup bgm;

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
        audioPool = new ObjectPool<GameObject>(OnCreateAudio, OnGetAudio, OnRealseAudio, OnDestoryAudio, true, defaultSize, maxSize);
    }

    public void PlaySFX(string path,Transform pos)
    {
        GameObject newAudio=audioPool.Get();
        if(pos!=null)
        {
            newAudio.transform.parent = pos;
            newAudio.transform.localPosition = Vector2.zero;
        }
        else
        {
            newAudio.transform.parent = transform;
            newAudio.transform.localPosition= Vector2.zero;
        }


        AudioSource audioSource = newAudio.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup= sfx;

        //判断是否加载过了该AudioClip,避免多次调用Resource加载
        AudioInfo audioInfo;
        if (hasLoadAudioInfoDic.TryGetValue(path,out AudioInfo value))
        {
            audioInfo = value;
        }
        else
        {
            audioInfo= Resources.Load<AudioInfo>("Audios/SFX/" + path);
            hasLoadAudioInfoDic.Add(path, audioInfo);
        }

        AudioClip clip = audioInfo.clip;
        audioSource.loop = audioInfo.isLoop;
        audioSource.volume= audioInfo.volume;

        if (clip != null)
        {
            audioSource.clip = clip;
        }
        audioSource.Play();
        if (!nowPlayingAudioDic.ContainsKey(path))
        {
            nowPlayingAudioDic.Add(path, newAudio);
        }
        if(!audioSource.loop)
        {
            StartCoroutine(WaitPlayAudio(newAudio,path));
        }
    }

    public void PlaySFX(string path, Transform pos,float canHearDistance,float maxIntensity,bool _vis)
    {
        GameObject newAudio = audioPool.Get();
        if (pos != null)
        {
            newAudio.transform.parent = pos;
            newAudio.transform.localPosition = Vector2.zero;
        }
        else
        {
            newAudio.transform.parent = transform;
            newAudio.transform.localPosition = Vector2.zero;
        }


        AudioSource audioSource = newAudio.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfx;

        //判断是否加载过了该AudioClip,避免多次调用Resource加载
        AudioInfo audioInfo;
        if (hasLoadAudioInfoDic.TryGetValue(path, out AudioInfo value))
        {
            audioInfo = value;
        }
        else
        {
            audioInfo = Resources.Load<AudioInfo>("Audios/SFX/" + path);
            hasLoadAudioInfoDic.Add(path, audioInfo);
        }

        AudioClip clip = audioInfo.clip;
        audioSource.loop = audioInfo.isLoop;
        audioSource.volume = audioInfo.volume;

        if(newAudio.transform.parent!=transform)
        {
            newAudio.GetComponent<AudioController>().Set(canHearDistance, maxIntensity);
        }

        if (clip != null)
        {
            audioSource.clip = clip;
        }
        audioSource.Play();
        if (!nowPlayingAudioDic.ContainsKey(path))
        {
            nowPlayingAudioDic.Add(path, newAudio);
        }
        if (!audioSource.loop)
        {
            StartCoroutine(WaitPlayAudio(newAudio, path));
        }
    }

    public void PlaySFX(string path, Transform pos,float volume,float speed)
    {
        GameObject newAudio = audioPool.Get();
        if (pos != null)
        {
            newAudio.transform.parent = pos;
            newAudio.transform.localPosition = Vector2.zero;
        }
        else
        {
            newAudio.transform.parent = transform;
            newAudio.transform.localPosition = Vector2.zero;
        }


        AudioSource audioSource = newAudio.GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = sfx;

        //判断是否加载过了该AudioClip,避免多次调用Resource加载
        AudioInfo audioInfo;
        if (hasLoadAudioInfoDic.TryGetValue(path, out AudioInfo value))
        {
            audioInfo = value;
        }
        else
        {
            audioInfo = Resources.Load<AudioInfo>("Audios/SFX/" + path);
            hasLoadAudioInfoDic.Add(path, audioInfo);
        }

        AudioClip clip = audioInfo.clip;
        audioSource.loop = audioInfo.isLoop;

        audioSource.volume = volume;
        audioSource.pitch = speed;

        if (clip != null)
        {
            audioSource.clip = clip;
        }
        audioSource.Play();
        if (!nowPlayingAudioDic.ContainsKey(path))
        {
            nowPlayingAudioDic.Add(path, newAudio);
        }
        else
        {
            StopSFX(path);
            nowPlayingAudioDic.Add(path,newAudio);
        }
        if (!audioSource.loop)
        {
            StartCoroutine(WaitPlayAudio(newAudio, path));
        }
    }

    public void StopSFX(string path)
    {
        if (nowPlayingAudioDic.TryGetValue(path, out GameObject newAudio))
        {
            if(newAudio?.GetComponent<AudioSource>()?.loop==true)
            {
                nowPlayingAudioDic.Remove(path);
                audioPool.Release(newAudio);
            }
            else
            {
                newAudio?.GetComponent<AudioSource>()?.Stop();
            }

        }
    }

    public void StopSFX_Short(string path)
    {
        if(nowPlayingAudioDic.TryGetValue(path,out GameObject newAudio))
        {
            nowPlayingAudioDic[path].GetComponent<AudioSource>().Stop();
        }
    }

    public void PlaySFX_Short(string path)
    {
        if (nowPlayingAudioDic.TryGetValue(path, out GameObject newAudio))
        {
            nowPlayingAudioDic[path].GetComponent<AudioSource>().Play();
        }
    }

    public void StopAllSFX()
    {
        List<string> strs= new List<string>();
        foreach(var path in nowPlayingAudioDic.Keys)
        {
            strs.Add(path);
        }
        for (int i = 0; i < strs.Count; i++)
        {
            StopSFX (strs[i]);
        }

    }

    public void PlayBGM(string path)
    {
        if (nowPlayingAudioDic.ContainsKey(path))
        {
            return;
        }

        AudioInfo audioInfo = Resources.Load<AudioInfo>("Audios/BGM/" + path);
        bgmAudioSource.outputAudioMixerGroup = bgm;
        bgmAudioSource.clip = audioInfo.clip;
        bgmAudioSource.volume= audioInfo.volume;
        nowBgm = path;
        nowPlayingAudioDic.Add(path, bgmAudioSource.gameObject);
        bgmAudioSource.Play();
    }

    public void StopBGM(string path)
    {
        if (nowBgm!=null && nowPlayingAudioDic.ContainsKey(path))
        {
            nowPlayingAudioDic.Remove(path);
        }
        bgmAudioSource.clip = null;
        nowBgm = null;

    }

    public void StopNowBGM()
    {
        StopBGM(nowBgm);
    }

    public bool CheckIsPlaying(string path)
    {
        return nowPlayingAudioDic.ContainsKey(path);
    }

    IEnumerator WaitPlayAudio(GameObject audio,string path)
    {
        AudioSource audioSource=audio.GetComponent<AudioSource>();
        while(audioSource.isPlaying)
        {
            yield return null;
        };
        audioPool.Release(audio);
        nowPlayingAudioDic.Remove(path);

    }

    #region 对象池相关

    private GameObject OnCreateAudio()
    {
        GameObject audio = Instantiate(audioPrefab);

        return audio;
    }

    private void OnGetAudio(GameObject obj)
    {
        obj.gameObject.SetActive(true);
    }

    private void OnRealseAudio(GameObject obj)
    {
        obj.transform.parent = transform;
        //重置逻辑
        obj?.GetComponent<AudioController>()?.ResetSetting();
        AudioSource audioSource = obj.GetComponent<AudioSource>();
        audioSource.clip = null;
        audioSource.outputAudioMixerGroup = sfx;
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.volume = 1;
        audioSource.pitch = 1;
        audioSource.panStereo = 0;
        obj.SetActive(false);
    }

    private void OnDestoryAudio(GameObject obj)
    {
        Destroy (obj);
    }

    #endregion


}

