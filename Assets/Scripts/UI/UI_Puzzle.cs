using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class UI_Puzzle : MonoBehaviour
{
    public string code;
    public List<UI_Button_Code> buttons;

    public GameObject puzzleCollider;

    public void Check()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (code[i] != buttons[i].nowNum + '0')
            {
                AudioManager.instance.PlaySFX("WrongUnlock", null);
                Debug.Log("WA");
                return;
            }
        }
        Debug.Log("AC");
        Success();
    }

    public void OnDisable()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].ResetNum();
        }
    }

    public void Success()
    {
        AudioManager.instance.PlaySFX("SuccessUnlock", null);
        UIManager.instance.ui.OpenPuzzle();
        puzzleCollider.GetComponent<PuzzleCollider>().isOnce = true;
        puzzleCollider.GetComponent<PuzzleCollider>().BaseInteract();

    }

}


