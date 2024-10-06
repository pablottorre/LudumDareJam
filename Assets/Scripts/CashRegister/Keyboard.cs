using UnityEngine;

public class Keyboard : MonoBehaviour
{
    [SerializeField] protected GeneralCR _generalCR;

    protected bool playerOnTop = false;

    [SerializeField] protected GameObject buttonToMove;
    [SerializeField] protected Transform originalPos;
    [SerializeField] protected Transform pressedPos;

    [SerializeField] protected float timerAnimation;

    protected void Start()
    {
        EventManager.SubscribeToEvent(EventNames._PressButton, PressButonRegistery);
    }

    public virtual void PressButonRegistery(params object[] parameters)
    {
        LeanTween.move(buttonToMove, pressedPos.position, timerAnimation).setOnComplete(() => ReturnToOriginalPos());
    }

    public void ReturnToOriginalPos()
    {
        LeanTween.move(buttonToMove, originalPos.position, timerAnimation);
    }
}