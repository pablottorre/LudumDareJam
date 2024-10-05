using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GeneralCR : MonoBehaviour
{
    [SerializeField] private List<ItemType> itemsFromClient = new List<ItemType>();


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
        EventManager.SubscribeToEvent(EventNames._OnClientEnterRegister, OnPlayerGiveItems);
    }

    private void PlayerCheckedLaser(params object[] parameters)
    {
        if ((bool)parameters[2])
        {
            Debug.Log("Can be scanned");
            OnItemSuccessufullyInput();
        }
        else
        {
            Debug.Log("Cannot be scanned");
            OnItemCannotBeScanned();
        }
    }

    public void AddKeyboardInput(string value)
    {
        barcodeInput += value;
    }

    public void PlayerEndedTipying(params object[] parameters)
    {
        Debug.Log("PlayerEndedTipying");
        EventManager.TriggerEvent(EventNames._OnCheckCode, barcodeInput);

        if (!checkForItemsExist)
        {
            OnItemUnuccessufullyInput();
        }
    }

    private void SetCheckItem(params object[] parameters)
    {
        checkForItemsExist = true;
        amountToAdd = (float)parameters[0];
        totalAmount += amountToAdd;

        if (checkForItemsExist)
        {
            OnItemSuccessufullyInput();
        }
    }

    private void OnItemSuccessufullyInput()
    {
        monitor.ItemTipedSuccess();
    }

    private void OnItemUnuccessufullyInput()
    {
        monitor.ItemTipedUnsuccess();
    }

    private void OnItemCannotBeScanned()
    {
        //Aca va feedback de que el producto no se puede escanear
    }

    private void OnPlayerGiveItems(params object[] parameters)
    {        
        itemsFromClient = (List<ItemType>)parameters[0];
    }

}
