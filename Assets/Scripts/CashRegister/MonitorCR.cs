using UnityEngine;
using TMPro;

public class MonitorCR : MonoBehaviour
{
    [SerializeField] private GeneralCR _generalCR;
    [SerializeField] private TMP_Text codeText;

    public void ItemTipedSuccess()
    {

    }
    
    public void ItemTipedUnsuccess()
    {

    }

    public void InputFromPlayer(string value)
    {
        codeText.text = value;
    }

}
