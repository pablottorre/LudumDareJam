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


    private List<Client> _clients = new List<Client>();
    [SerializeField] private float _maxTimerForNewClient = 8;
    private float _timerForNewClient;
    private SimplePool<Client> _clientsPool;


    private readonly Dictionary<ItemType, SimplePool<Item>> _itemsPool = new Dictionary<ItemType, SimplePool<Item>>();

    [SerializeField, Tooltip("Spawn Points for items just ahead of the CR, so can just be dropped by physics")]
    private Transform[] _spawnPoints;

    private bool isWokingHour;

    private bool _crIsBusy = false;

    [SerializeField, Tooltip("Each prefab of every item")]
    private Item[] _itemsPrefabs;

    [SerializeField, Tooltip("Client Prefab")]
    private Client _clientPrefab;


    [Header("General Game Settings")]
    [SerializeField] private int numberDay;
    private int savings;
    private int earnings;
    [SerializeField] private int costs;
    [SerializeField] private List<int> listOfCosts = new List<int>();
    [SerializeField] private List<float> listOfTimers = new List<float>();
    public bool playerLoseTheGame = false;

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

        _timerForNewClient = _maxTimerForNewClient;
    }

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._OnEndWorkNewDay, EndOfTimerDay);
        EventManager.SubscribeToEvent(EventNames._OnStartNewDay, StartOfNewDay);
    }

    private void Update()
    {
        if (!isWokingHour)
            return;

        _timerForNewClient -= Time.deltaTime;
        if (_timerForNewClient < 0 && _clients.Count < _queueNodes.Length)
        {
            _clients.Add(_clientsPool.EnableObject(_queueNodes[_clients.Count]));
            _timerForNewClient = _maxTimerForNewClient;

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

        earnings += (int)parameters[0];

        if (!isWokingHour) EndOfFullDay();

        if (!_clients.Any()) return;

        for (var i = 0; i < _clients.Count; i++)
        {
            _clients[i].transform.position = _queueNodes[i].transform.position;
            _clients[i].transform.rotation = _queueNodes[i].transform.rotation;
        }

        AttendNextClient();
    }


    private void EndOfTimerDay(params object[] parameters)
    {
        for (int i = 1; i < _clients.Count; i++)
        {
            _clients[i].OnFinishBuy();
        }

        _clients = new List<Client>() { _clients[0]};
        isWokingHour = false;
    }

    private void EndOfFullDay()
    {

        if (numberDay >= listOfCosts.Count )
            costs = listOfCosts[listOfCosts.Count];
        else
            costs = listOfCosts[numberDay];

        if ((earnings + savings) >= costs)
            playerLoseTheGame = false;
        else
            playerLoseTheGame = true;


        EventManager.TriggerEvent(EventNames._OnEndNewDay);


        
        if (numberDay >= listOfTimers.Count )
            TimeSystem.instance.SetterMaxTimer(listOfTimers[listOfTimers.Count]);
        else
            TimeSystem.instance.SetterMaxTimer(numberDay);
        
    }


    public int GetterNumberDay()
    {
        return numberDay;
    }

    public int GetterSaving()
    {
        return savings;
    }

    public int GetterEarnings()
    {
        return earnings;
    } 
    
    public int GetterCosts()
    {
        return costs;
    }

    private void StartOfNewDay(params object[] parameters)
    {
        savings = (savings + earnings - costs);
        numberDay++;
        isWokingHour = true;
        earnings = 0;
    } 

}