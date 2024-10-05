using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Item : MonoBehaviour, IPoolObject<Item>
{
    [SerializeField] private SO_Item _soItem;
    
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private int _codeLength;
    [SerializeField] private string _actualCode;
    [SerializeField] private GameObject _codeSticker;

    private Action<Item> _returnFunction;

    public void OnCreateObject(Action<Item> returnFunction)
    {
        _returnFunction = returnFunction;
    }

    public void OnEnableSetUp(Transform from)
    {
        _actualCode = string.Empty;
        for (var i = 0; i < _codeLength; i++)
        {
            _actualCode += Random.Range(0, 10);
        }

        transform.position = from.position;
        transform.rotation = from.rotation;
        
        gameObject.SetActive(true);
        
        //EventManager.SubscribeToEvent();
    }

    public void OnDisableSetUp()
    {
        throw new NotImplementedException();
    }

    private void OnFinishBuyEvent(params object[] parameters)
    {
        
    }
}
