using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleCollider : EventCollider
{
    public GameObject Puzzle;
    public override void Interact()
    {
        UIManager.instance.ui.puzzleUI = Puzzle;
        UIManager.instance.ui.OpenPuzzle();
    }

    public void BaseInteract()
    {
        base.Interact();
    }

}
