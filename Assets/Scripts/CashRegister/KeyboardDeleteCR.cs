using UnityEngine;

public class KeyboardDeleteCR : Keyboard
{
    private void Start()
    {
        base.Start();
    }


    public override void PressButonRegistery(params object[] parameters)
    {
        _generalCR.RemoveLastLetter();
        PlaySound();
        LeanTween.move(buttonToMove, pressedPos.position, timerAnimation).setOnComplete(() => ReturnToOriginalPos());
    }
}