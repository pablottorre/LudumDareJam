using System;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour, IPoolObject<Item>
{
    [SerializeField] private SO_Item _soItem;

    [SerializeField] private Rigidbody _rb;

    [SerializeField] private int _minCodeLength, _maxCodeLength;
    [SerializeField] private string _actualCode;
    [SerializeField] private GameObject _codeSticker, _registerSticker;

    private Action<Item> _returnFunction;

    public void OnCreateObject(Action<Item> returnFunction)
    {
        _returnFunction = returnFunction;
    }

    public void OnEnableSetUp(Transform from)
    {
        _actualCode = string.Empty;
        for (var i = 0; i < Random.Range(_minCodeLength, _maxCodeLength); i++)
        {
            _actualCode += Random.Range(0, 10);
        }

        transform.position = from.position;
        transform.rotation = from.rotation;

        gameObject.SetActive(true);

        EventManager.SubscribeToEvent(EventNames._OnFinishBuy, OnFinishBuyEvent);
        EventManager.SubscribeToEvent(EventNames._OnCheckCode, OnCheckCode);
    }

    public void OnDisableSetUp()
    {
        EventManager.UnsuscribeToEvent(EventNames._OnFinishBuy, OnFinishBuyEvent);
        EventManager.UnsuscribeToEvent(EventNames._OnCheckCode, OnCheckCode);
        _registerSticker.SetActive(false);
    }

    private void OnFinishBuyEvent(params object[] parameters)
    {
        _returnFunction(this);
    }

    private void OnCheckCode(params object[] parameters)
    {
        _registerSticker.SetActive(_actualCode.Equals((string)parameters[0]));
    }
}