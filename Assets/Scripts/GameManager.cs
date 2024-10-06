using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;


    [SerializeField,
     Tooltip(
         "The queue nodes for the clients to follow to get into the CR, this will also limit the max of client at the same time")]
    private Transform[] _queueNodes;
    [SerializeField] private Transform _originalSpawnPoint;


    private readonly List<Client> _clients = new List<Client>();
    [SerializeField] private float _timerForNewClient = 8;
    private SimplePool<Client> _clientsPool;


    private readonly Dictionary<ItemType, SimplePool<Item>> _itemsPool = new Dictionary<ItemType, SimplePool<Item>>();

    [SerializeField, Tooltip("Spawn Points for items just ahead of the CR, so can just be dropped by physics")]
    private Transform[] _spawnPoints;



    private bool _crIsBusy = false;

    [SerializeField, Tooltip("Each prefab of every item")]
    private Item[] _itemsPrefabs;

    [SerializeField, Tooltip("Client Prefab")]
    private Client _clientPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        EventManager.SubscribeToEvent(EventNames._OnFinishBuy, OnFinishBuy);

        foreach (var item in _itemsPrefabs)
        {
            if (_itemsPool.ContainsKey(item.Type)) continue;
            
            _itemsPool.Add(item.Type,
                new SimplePool<Item>(() => Instantiate(item)));
        }

        _clientsPool = new SimplePool<Client>(() => Instantiate(_clientPrefab));
    }

    private void Update()
    {
        _timerForNewClient -= Time.deltaTime;
        if (_timerForNewClient < 0 && _clients.Count < _queueNodes.Length)
        {
            _clients.Add(_clientsPool.EnableObject(_queueNodes[_clients.Count]));
            _timerForNewClient = 8;

            if (!_crIsBusy)
            {
                AttendNextClient();
            }
        }
    }

    private void AttendNextClient()
    {
        if (_clients.Count <= 0) return;

        foreach (var item in _clients[0].itemsInBag)
        {
            _itemsPool[item].EnableObject(_spawnPoints[Random.Range(0, _spawnPoints.Length)]);
        }

        _crIsBusy = true;
        EventManager.TriggerEvent(EventNames._OnClientEnterRegister, _clients[0].itemsInBag);
    }

    private void OnFinishBuy(params object[] parameters)
    {
        _clients[0].OnFinishBuy();
        _clients.RemoveAt(0);

        _crIsBusy = false;

        if (!_clients.Any()) return;

        for (var i = 0; i < _clients.Count; i++)
        {
            _clients[i].transform.position = _queueNodes[i].transform.position;
            _clients[i].transform.rotation = _queueNodes[i].transform.rotation;
        }

        AttendNextClient();
    }
}