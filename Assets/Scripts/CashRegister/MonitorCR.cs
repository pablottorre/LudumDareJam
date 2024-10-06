using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class MonitorCR : MonoBehaviour
{
    [SerializeField] private GeneralCR _generalCR;
    [SerializeField] private TMP_Text codeText;
    [SerializeField] private TMP_Text bgCodeText;
    private string _correctCode = string.Empty;
    private string _currentCode = string.Empty;

    public void ItemTipedSuccess(string value, bool isCodeItem = true)
    {
        if (isCodeItem)
        {
            _currentCode = "";
        }

        bgCodeText.text = "";
        _correctCode = "";
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
        
        codeText.text = _currentCode.Equals(string.Empty)? string.Empty : _currentCode;
        
        bgCodeText.text = "";
    }

    public void InputFromPlayer(string value)
    {
        var result = "";

        _currentCode = value;


        if (value.Length > 1)
        {
            result += value.Substring(0, value.Length - 1);
        }

        if (value.Length > 0)
        {
            int lastCharacterIndex = value.Length - 1;

            if (_correctCode == string.Empty || (lastCharacterIndex < _correctCode.Length &&
                                                 value[lastCharacterIndex] == _correctCode[lastCharacterIndex]))
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
        _currentCode = value;
        codeText.text = _currentCode;
    }

    public void ItemToType(string value)
    {
        _correctCode = value;
        bgCodeText.text = _correctCode;
    }
}