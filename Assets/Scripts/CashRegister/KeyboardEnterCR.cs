using UnityEngine;

public class KeyboardEnterCR : Keyboard
{
    protected void Start()
    {
        base.Start();
    }

    public override void PressButonRegistery(params object[] parameters)
    {
        _generalCR.PlayerEndedTipying();
        LeanTween.move(buttonToMove, pressedPos.position, timerAnimation).setOnComplete(() => ReturnToOriginalPos());
    }
}