using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class GeneralCR : MonoBehaviour
{
    private List<ItemType> itemsFromClient = new List<ItemType>();


    [SerializeField] private List<KeyboardCR> keys = new List<KeyboardCR>();
    [SerializeField] private MonitorCR monitor;

    private string barcodeInput = string.Empty;

    private bool checkForItemsExist = false;

    private float totalAmount;
    private float amountToAdd;

    [SerializeField] private TMP_Text totalCashMde;

    [SerializeField] private int _maxChars;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._CheckForLaser, PlayerCheckedLaser);
        EventManager.SubscribeToEvent(EventNames._EndInputOfBarcode, PlayerEndedTipying);
        EventManager.SubscribeToEvent(EventNames._OnSuccessCheckCode, SetCheckItem);
        EventManager.SubscribeToEvent(EventNames._OnClientEnterRegister, OnPlayerGiveItems);
    }

    private void PlayerCheckedLaser(params object[] parameters)
    {
        if ((bool)parameters[5])
            return;


        if ((bool)parameters[2])
        {
            Debug.Log("Can be scanned");
            amountToAdd = (float)parameters[0];
            totalAmount += amountToAdd;
            EventManager.TriggerEvent(EventNames._OnCashItem);
            OnItemSuccessufullyInput((ItemType)parameters[3], amountToAdd.ToString());
        }
        else
        {
            Debug.Log("Cannot be scanned");
            OnItemCannotBeScanned((string)parameters[4]);
        }
    }

    public void AddKeyboardInput(string value)
    {
        if (barcodeInput.Length > _maxChars) return;
        
        barcodeInput += value;
        monitor.InputFromPlayer(barcodeInput);
    }

    public void PlayerEndedTipying(params object[] parameters)
    {
        EventManager.TriggerEvent(EventNames._OnCheckCode, barcodeInput);
        barcodeInput = string.Empty;

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
            OnItemSuccessufullyInput((ItemType)parameters[1], amountToAdd.ToString());
        }
    }

    private void OnItemSuccessufullyInput(ItemType itemScanned, string moneyToGain)
    {
        monitor.ItemTipedSuccess(moneyToGain);
        totalCashMde.text = "$" + totalAmount;
        itemsFromClient.Remove(itemScanned);

        if (itemsFromClient.Count == 0)
        {
            Debug.Log("Complete el cliente");
            EventManager.TriggerEvent(EventNames._OnFinishBuy);
        }
        else
        {
            

        }
    }

    public void RemoveLastLetter()
    {
        if (string.IsNullOrEmpty(barcodeInput))
        {
            return;
        }

        barcodeInput = barcodeInput.Substring(0, barcodeInput.Length - 1);

        monitor.RemoveLastInput(barcodeInput);
    }

    private void OnItemUnuccessufullyInput()
    {
        monitor.ItemTipedUnsuccess();
    }

    private void OnItemCannotBeScanned(string barcodeToDisplay)
    {
        monitor.ItemToType(barcodeToDisplay);
    }

    private void OnPlayerGiveItems(params object[] parameters)
    {
        itemsFromClient = (List<ItemType>)parameters[0];
    }

}
