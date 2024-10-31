using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance;

    public bool isTip;
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
        if(!isTip)
        {
            DontDestroyOnLoad(this.gameObject.transform.root);
            UIManager.instance.FadeInQucik();
            UIManager.instance.ShowPlayGameTip();
        }

    }


    public void Load(string name)
    {
        DOTween.Clear(true);
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(name,LoadSceneMode.Single);
    }

   
}
