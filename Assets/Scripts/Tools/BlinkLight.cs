using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BlinkLight : MonoBehaviour
{
    public  Light2D[] light2Ds;
    public List<float> defaultIntensity=new List<float>();
    public float cnt = 0;
    public float changeSpeed;
    private bool canIncrease;
    void Start()
    {
        for(int i=0;i<light2Ds.Length; i++)
        {
            defaultIntensity.Add(light2Ds[i].intensity);
        }
    }

    void Update()
    {
        if (cnt>0)
        {
            if(!AudioManager.instance.CheckIsPlaying("FlashlightBlink"))
            {
                AudioManager.instance.PlaySFX("FlashlightBlink", transform);
            }
            if(!canIncrease)
            {
                bool isAllZero = true;
                for (int i = 0; i < light2Ds.Length; i++)
                {
                    if (light2Ds[i].intensity > 0)
                    {
                        light2Ds[i].intensity -= changeSpeed * Time.deltaTime;
                        light2Ds[i].intensity = Mathf.Max(0, light2Ds[i].intensity);
                        isAllZero = false;
                    }
                }
                if (isAllZero)
                {
                    canIncrease = true;
                }
            }
            else
            {
                bool isAllMax = true;
                for (int i = 0; i < light2Ds.Length; i++)
                {
                    if (light2Ds[i].intensity < defaultIntensity[i])
                    {
                        light2Ds[i].intensity += changeSpeed * Time.deltaTime;
                        light2Ds[i].intensity = Mathf.Min(defaultIntensity[i], light2Ds[i].intensity);
                        isAllMax = false;
                    }
                }
                if (isAllMax)
                {
                    canIncrease = false;
                    cnt--;
                }
            }
        }
        else
        {
            for (int i = 0; i < light2Ds.Length; i++)
            {
                light2Ds[i].intensity = defaultIntensity[i];
            }
        }

    }

    public void OnDisable()
    {
        for (int i = 0; i < light2Ds.Length; i++)
        {
            light2Ds[i].intensity = defaultIntensity[i];
        }
        cnt = 0;
        AudioManager.instance.StopSFX("FlashlightBlink");
    }

    public void ControllBlink(int Count)
    {
        cnt = Count;
    }

}
