using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour, IPoolObject<Item>
{
    [SerializeField] private SO_Item _soItem;

    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Collider _collider;

    [SerializeField] private int _minCodeLength, _maxCodeLength;
    [SerializeField] private string _actualCode;
    [SerializeField] private List<GameObject> _registerStickers =  new List<GameObject>();

    private Action<Item> _returnFunction;

    public float Cost => _soItem.cost;
    public string Name => _soItem.itemName;

    public ItemType Type => _soItem.type;

    public bool canBeScanned;

    public bool hasBeenCashed;

    private Transform _followPoint;

    [SerializeField] private float _draggingVelocity;
    [SerializeField] private float _draggingDistance;

    [SerializeField] private List<GameObject> listOfSkins = new List<GameObject>();

    private int numberRandom;

    private void LateUpdate()
    {
        if (_followPoint == null) return;

        if (canBeScanned)
        {
            transform.position = _followPoint.position;
        }
        else
        {
            //_collider.bounds.extents.x
            transform.position = new Vector3(_followPoint.position.x, transform.position.y, _followPoint.position.z);
        }
    }

    #region Pool Region

    public void OnCreateObject(Action<Item> returnFunction)
    {
        _returnFunction = returnFunction;
        gameObject.SetActive(false);
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

        if (listOfSkins.Count == 1)
        {
            listOfSkins[0].SetActive(true);
            _registerStickers[0].SetActive(true);
        }
        else
        {
            numberRandom = Random.Range(0, listOfSkins.Count);
            listOfSkins[numberRandom].SetActive(true);
        }

        gameObject.SetActive(true);

        hasBeenCashed = false;

        EventManager.SubscribeToEvent(EventNames._OnFinishBuy, OnFinishBuyEvent);
        EventManager.SubscribeToEvent(EventNames._OnCheckCode, OnCheckCode);
        EventManager.SubscribeToEvent(EventNames._OnCashItem, OnCashed);
    }

    public string GetterActualCode()
    {
        return _actualCode;
    }

    public void OnDisableSetUp()
    {
        EventManager.UnsuscribeToEvent(EventNames._OnFinishBuy, OnFinishBuyEvent);
        EventManager.UnsuscribeToEvent(EventNames._OnCheckCode, OnCheckCode);
        EventManager.UnsuscribeToEvent(EventNames._OnCashItem, OnCashed);
        hasBeenCashed = false;
        _followPoint = null;
        gameObject.SetActive(false);

        for (int i = 0; i < listOfSkins.Count; i++)
        {
            listOfSkins[i].SetActive(false);
        }

        for (int i = 0; i < _registerStickers.Count; i++)
        {
            _registerStickers[i].SetActive(false);
        }
    }

    #endregion

    private void OnFinishBuyEvent(params object[] parameters)
    {
        _returnFunction(this);
    }

    private void OnCheckCode(params object[] parameters)
    {
        if (!_registerStickers[numberRandom].activeSelf && _actualCode.Equals((string)parameters[0]))
        {
            _registerStickers[numberRandom].SetActive(true);
            hasBeenCashed = true;
            EventManager.TriggerEvent(EventNames._OnSuccessCheckCode, Cost, Type);
        }
    }

    public Item Interact(bool interactState, bool isDrag = false, Transform root = null)
    {
        if (interactState)
        {
            _rb.Sleep();
            _rb.useGravity = false;
            _followPoint = root;
            _collider.isTrigger = true;
            if (!isDrag)
            {
                transform.parent = root;
                transform.localPosition = Vector3.zero;
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            transform.parent = null;
            _followPoint = null;
            _rb.useGravity = true;
            _rb.WakeUp();
            _collider.isTrigger = false;
        }

        return this;
    }

    private void OnCashed(params object[] parameters)
    {
        if (_followPoint!=null)
        {
            hasBeenCashed = true;
        }
    }
}