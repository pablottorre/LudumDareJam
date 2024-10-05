using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GeneralCR : MonoBehaviour
{
    [SerializeField] private List<KeyboardCR> keys = new List<KeyboardCR>();
    [SerializeField] private MonitorCR monitor;

    private string barcodeInput;

    private bool checkForItemsExist = false;

    private float totalAmount;
    private float amountToAdd;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._CheckForLaser, PlayerCheckedLaser);
        EventManager.SubscribeToEvent(EventNames._EndInputOfBarcode, PlayerEndedTipying);
        EventManager.SubscribeToEvent(EventNames._OnSuccessCheckCode, SetCheckItem);
    }

    private void PlayerCheckedLaser(params object[] parameters)
    {
        Debug.Log("PlayerCheckedLaser");
    }

    private void PlayerEndedTipying(params object[] parameters)
    {
        Debug.Log("PlayerEndedTipying");
        EventManager.TriggerEvent(EventNames._OnCheckCode, barcodeInput);
    }

    private void SetCheckItem(params object[] parameters)
    {
        checkForItemsExist = true;
        amountToAdd = (float)parameters[0];
        totalAmount = +amountToAdd;
    }



}
