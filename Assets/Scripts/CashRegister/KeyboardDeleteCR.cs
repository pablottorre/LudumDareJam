using UnityEngine;

public class KeyboardDeleteCR : MonoBehaviour
{
    [SerializeField] private GeneralCR _generalCR;

    private bool playerOnTop = false;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._PressButton, PressButonRegistery);
    }

    public void PressButonRegistery(params object[] parameters)
    {
        if (playerOnTop)
        {
            _generalCR.RemoveLastLetter();
        }
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
