using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class MonitorCR : MonoBehaviour
{
    [SerializeField] private GeneralCR _generalCR;
    [SerializeField] private TMP_Text codeText;

    public void ItemTipedSuccess(string value)
    {
        codeText.text = "+$" + value;
        StartCoroutine(CleanMonitor());
    }

    public void ItemTipedUnsuccess()
    {
        codeText.text = "X";
        StartCoroutine(CleanMonitor());
    }

    private IEnumerator CleanMonitor()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        codeText.text = "";
    }

    public void InputFromPlayer(string value)
    {
        codeText.text = value;
    }

}
