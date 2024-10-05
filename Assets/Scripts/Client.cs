using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Client : MonoBehaviour, IPoolObject<Client>
{
    public List<ItemType> itemsInBag { get; private set; } = new List<ItemType>();

    private Action<Client> _returnFunction;

    [SerializeField] private int _minItemsAmount, _maxItemsAmount;

    public void OnCreateObject(Action<Client> returnFunction)
    {
        _returnFunction = returnFunction;
        gameObject.SetActive(false);
    }

    public void OnEnableSetUp(Transform enablePoint)
    {
        itemsInBag.Clear();
        var itemsAmount = Random.Range(_minItemsAmount > 0 ? _minItemsAmount : 0,
            _maxItemsAmount > _minItemsAmount ? _maxItemsAmount : _minItemsAmount + 1);

        for (var i = 0; i < itemsAmount; i++)
        {
            itemsInBag.Add((ItemType)Random.Range(0, Enum.GetValues(typeof(ItemType)).Length));
        }

        transform.position = enablePoint.position;
        transform.rotation = enablePoint.rotation;
        gameObject.SetActive(true);
    }

    public void OnDisableSetUp()
    {
        gameObject.SetActive(false);
    }

    public void OnFinishBuy()
    {
        _returnFunction(this);
    }
}