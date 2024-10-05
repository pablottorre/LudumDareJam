using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SimplePool<T> where T : IPoolObject<T>
{
    private Queue<T> _availableObjects = new Queue<T>();
    private Func<T>  _creationFunc;
    
    public SimplePool(Func<T> creationProcess)
    {
        _creationFunc = creationProcess;

        for (var i = 0; i < 3; i++)
        {
            var newItem = _creationFunc();
            newItem.OnCreateObject(ReturnObject);
            _availableObjects.Enqueue(newItem);
        }
    }

    public T EnableObject(Transform enablePoint)
    {
        T selectedItem;
        
        if (_availableObjects.Any())
        {
            selectedItem = _availableObjects.Dequeue();
            selectedItem.OnEnableSetUp(enablePoint);
        }
        else
        {
            selectedItem = _creationFunc();
            selectedItem.OnCreateObject(ReturnObject);
            selectedItem.OnEnableSetUp(enablePoint);
        }

        return selectedItem;
    }

    private void ReturnObject(T poolObject)
    {
        poolObject.OnDisableSetUp();
        _availableObjects.Enqueue(poolObject);
    }
}

public interface IPoolObject<out T>
{
    public void OnCreateObject(Action<T> returnFunction);
    public void OnEnableSetUp(Transform enablePoint);
    public void OnDisableSetUp();
}