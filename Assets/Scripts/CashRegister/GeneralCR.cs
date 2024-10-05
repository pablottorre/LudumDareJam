using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GeneralCR : MonoBehaviour
{
    [SerializeField] private List<KeyboardCR> keys = new List<KeyboardCR>();
    [SerializeField] private MonitorCR monitor;

    private string barcodeInput;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._CheckForLaser, PlayerCheckedLaser);
    }

    private void PlayerCheckedLaser(params object[] parameters)
    {
        Debug.Log("PlayerCheckedLaser");
    }



}
