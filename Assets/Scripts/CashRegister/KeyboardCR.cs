using UnityEngine;

public class KeyboardCR : Keyboard
{
    [SerializeField] private string inputToShare;


    protected void Start()
    {
        base.Start();
    }

    public override void PressButonRegistery(params object[] parameters)
    {
        _generalCR.AddKeyboardInput(inputToShare);
        LeanTween.move(buttonToMove, pressedPos.position, timerAnimation).setOnComplete(() => ReturnToOriginalPos());
    }
}