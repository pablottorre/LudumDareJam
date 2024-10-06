using UnityEngine;

public class KeyboardDeleteCR : MonoBehaviour
{
    [SerializeField] private GeneralCR _generalCR;

    private bool playerOnTop = false;

    [SerializeField] private GameObject buttonToMove;
    [SerializeField] private Transform originalPos;
    [SerializeField] private Transform pressedPos;

    [SerializeField] private float timerAnimation;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._PressButton, PressButonRegistery);
    }

    public void PressButonRegistery(params object[] parameters)
    {
        if (playerOnTop)
        {
            _generalCR.RemoveLastLetter();
            LeanTween.move(buttonToMove, pressedPos.position, timerAnimation).setOnComplete(() => ReturnToOriginalPos());
        }
    }

    public void ReturnToOriginalPos()
    {
        LeanTween.move(buttonToMove, originalPos.position, timerAnimation);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            playerOnTop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            playerOnTop = false;
        }
    }
}
