using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public bool isNeed2D = false;
    private AudioSource audioSource;
    public float canHearDistance;
    private float defaultIntensity;
    public float maxIntensity;
    private float changeSpeed;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!isNeed2D)
        {
            return;
        }

        float d = Vector2.Distance(GameManager.instance.player.transform.position, transform.position);
        float p = Mathf.Min(d / canHearDistance, 1);
        if(GameManager.instance.player.transform.position.x>transform.position.x)
        {
            audioSource.panStereo = -p;
        }
        else
        {
            audioSource.panStereo = p;
        }

        audioSource.volume = Mathf.Min(maxIntensity,(defaultIntensity + (1 - d / canHearDistance) * changeSpeed));

    }

    public void Set(float _canHearDistance, float _maxIntensity)
    {
        defaultIntensity = GetComponent<AudioSource>().volume;
        canHearDistance= _canHearDistance;
        maxIntensity= _maxIntensity;
        isNeed2D = true;
        defaultIntensity = GetComponent<AudioSource>().volume;
        changeSpeed = maxIntensity - defaultIntensity / canHearDistance;
    }

    public void ResetSetting()
    {
        isNeed2D = false;
        defaultIntensity = 1;
        maxIntensity = 0;
        canHearDistance = 0;
    }
}
