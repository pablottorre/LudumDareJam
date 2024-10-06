using UnityEngine;

public class KeyboardEnterCR : Keyboard
{

    protected void Start()
    {
        base.Start();
    }

    protected override void PressButonRegistery(params object[] parameters)
    {
        if (playerOnTop)
        {
            _generalCR.PlayerEndedTipying();
            LeanTween.move(buttonToMove, pressedPos.position, timerAnimation).setOnComplete(() => ReturnToOriginalPos());
        }
    }

}
