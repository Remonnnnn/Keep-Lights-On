using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public enum BlinkState
{
    Quick,
    Slow,
    None
}

public class BlinkLight_Ground : MonoBehaviour
{
    public BlinkState state = BlinkState.Quick;
    public string audioPath = "BlinkLight";
    public float canHearDistance;
    public float maxVolume;

    private Light2D light2d = null;

    [Header("快速变换设置")]
    public float distance;
    private float defaultIntensity;
    private bool isLight = true;
    private float Timer = 0;
    public GameObject lightMan;

    [Header("渐变设置")]
    public float minIntensity;
    public float loseSpeed;
    private bool isLose = true;


    void Start()
    {
        if(GetComponent<Light2D>()!=null)
        {
            light2d = GetComponent<Light2D>();
            defaultIntensity = light2d.intensity;
        }
        EventManager.instance?.AddEventListener("Blockout", ChangeState);

    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.player!=null && GameManager.instance.player.isOver)
        {
            return;
        }

        if (state==BlinkState.None)
        {
            if (Vector2.Distance(GameManager.instance.player.transform.position, transform.position) < canHearDistance)
            {
                if (!AudioManager.instance.CheckIsPlaying(audioPath))
                {
                    AudioManager.instance.PlaySFX(audioPath, transform, canHearDistance, maxVolume, true);
                }
            }
            else
            {
                AudioManager.instance.StopSFX(audioPath);
            }
            return;
        }

        if (state == BlinkState.Quick)
        {
            BlinkQucik();
        }
        else
        {
            if(GameManager.instance.player != null)
            {
                if (Vector2.Distance(GameManager.instance.player.transform.position, transform.position) < canHearDistance)
                {
                    if (!AudioManager.instance.CheckIsPlaying(audioPath))
                    {
                        AudioManager.instance.PlaySFX(audioPath, transform, canHearDistance, maxVolume, true);
                    }
                }
                else
                {
                    AudioManager.instance.StopSFX(audioPath);
                }
            }

            BlinkSlow();
        }

    }

    private void BlinkSlow()
    {
        float d = loseSpeed * Time.deltaTime;
        if (isLose)
        {
            light2d.intensity -= d;
            light2d.intensity = Mathf.Max(light2d.intensity, minIntensity);
            if (light2d.intensity <= minIntensity)
            {
                isLose = false;
            }
        }
        else
        {
            light2d.intensity += d;
            light2d.intensity = Mathf.Min(light2d.intensity, defaultIntensity);
            if (light2d.intensity >= defaultIntensity)
            {
                isLose = true;
            }
        }
    }

    private void BlinkQucik()
    {
        Timer += Time.deltaTime;

        if (Timer >= distance)
        {
            Timer = 0;
            Change();
        }
    }

    public void Change()
    {
        if (isLight)
        {
            light2d.intensity = 0;
            isLight = false;
            if(Vector2.Distance(GameManager.instance.player.transform.position, transform.position)<canHearDistance)
            {
                AudioManager.instance.PlaySFX(audioPath, transform, canHearDistance, maxVolume,true);
            }
            if (lightMan != null && lightMan.activeSelf)
            {
                lightMan?.SetActive(false);
            }
        }
        else
        {
            light2d.intensity = defaultIntensity;
            isLight = true;
            if (GameManager.instance.player.nowSan <= 60 && Random.Range(0, 100) < 30)
            {
                if (lightMan == null)
                {
                    return;
                }
                lightMan.SetActive(true);
                if (GameManager.instance.player.transform.position.x < transform.position.x)
                {
                    lightMan.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
                else
                {
                    lightMan.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
    }

    public void ChangeState()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, canHearDistance);
    }
}
