using UnityEngine;

public class KeyboardCR : MonoBehaviour
{
    [SerializeField] private GeneralCR _generalCR;

    private bool playerOnTop = false;
    [SerializeField] private int inputToShare;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._PressButton, PressButonRegistery);
    }

    public void PressButonRegistery(params object[] parameters)
    {

        if (playerOnTop)
        {        
            Debug.Log(inputToShare);
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
