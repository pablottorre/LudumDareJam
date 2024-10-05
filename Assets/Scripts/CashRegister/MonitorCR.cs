using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class MonitorCR : MonoBehaviour
{
    [SerializeField] private GeneralCR _generalCR;
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private TMP_Text bgCodeText;
    private string _correctCode;
    private string _currentCode;

    public void ItemTipedSuccess(string value)
    {
        bgCodeText.text = "";
        _correctCode = "";
        _currentCode = "";
        codeText.text = "+$" + value;
        StartCoroutine(CleanMonitor());
    }

    public void ItemTipedUnsuccess()
    {
        codeText.text = "X";
        _currentCode = "";
        StartCoroutine(CleanMonitor());
    }

    private IEnumerator CleanMonitor()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        codeText.text = "";
        bgCodeText.text = "";
    }

    public void InputFromPlayer(string value)
    {
        string result = "";

        _currentCode = value;


        if (value.Length > 1)
        {
            result += value.Substring(0, value.Length - 1);
        }

        if (value.Length > 0)
        {
            int lastCharacterIndex = value.Length - 1;

            if (lastCharacterIndex < _correctCode.Length && value[lastCharacterIndex] == _correctCode[lastCharacterIndex])
            {
                result += $"<color=white>{value[lastCharacterIndex]}</color>";
            }
            else
            {
                result += $"<color=red>{value[lastCharacterIndex]}</color>";
            }
        }
        codeText.text = result;
    }

    public void RemoveLastInput(string value)
    {        
        _currentCode= value;
        codeText.text = _currentCode;
    }

    public void ItemToType(string value)
    {
        _correctCode = value;
        bgCodeText.text = _correctCode;
    }

}
