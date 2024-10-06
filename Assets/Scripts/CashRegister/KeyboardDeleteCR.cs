using UnityEngine;

public class KeyboardDeleteCR : Keyboard
{

    private void Start()
    {
        base.Start();
    }


    protected override void PressButonRegistery(params object[] parameters)
    {
        if (playerOnTop)
        {
            _generalCR.RemoveLastLetter();
            LeanTween.move(buttonToMove, pressedPos.position, timerAnimation).setOnComplete(() => ReturnToOriginalPos());
        }
    }

}
