using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class UI_VolumeSlider : MonoBehaviour,IPointerDownHandler
{
    public Slider slider;
    public string parametr;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multiplier;

    private void Start()
    {
        float volume = 0f;
        if(audioMixer.GetFloat(parametr,out volume))
        {
            slider.value = Mathf.Pow(10, volume / multiplier);
        }

    }

    public void SliderValue(float _value) => audioMixer.SetFloat(parametr, Mathf.Log10(_value) * multiplier);

    public void LoadSlider(float _value)
    {
        if (_value >= 0.001f)
        {
            slider.value = _value;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }
}
