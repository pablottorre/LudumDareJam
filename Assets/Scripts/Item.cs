using System;
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
    [SerializeField] private GameObject _codeSticker, _registerSticker;

    private Action<Item> _returnFunction;

    public float Cost => _soItem.cost;
    public string Name => _soItem.itemName;

    public ItemType Type => _soItem.type;

    public bool canBeScanned;

    private Transform _followPoint;

    private void LateUpdate()
    {
        if (_followPoint == null) return;
       
        if (canBeScanned)
        {
            transform.position = _followPoint.position;
        }
        else
        {
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

        gameObject.SetActive(true);

        EventManager.SubscribeToEvent(EventNames._OnFinishBuy, OnFinishBuyEvent);
        EventManager.SubscribeToEvent(EventNames._OnCheckCode, OnCheckCode);
    }

    public string GetterActualCode()
    {
        return _actualCode;
    }

    public void OnDisableSetUp()
    {
        EventManager.UnsuscribeToEvent(EventNames._OnFinishBuy, OnFinishBuyEvent);
        EventManager.UnsuscribeToEvent(EventNames._OnCheckCode, OnCheckCode);
        _registerSticker.SetActive(false);
        gameObject.SetActive(false);
    }

    #endregion

    private void OnFinishBuyEvent(params object[] parameters)
    {
        _returnFunction(this);
    }

    private void OnCheckCode(params object[] parameters)
    {
        if (!_registerSticker.activeSelf && _actualCode.Equals((string)parameters[0]))
        {
            _registerSticker.SetActive(true);
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
}