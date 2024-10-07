using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSystem : MonoBehaviour
{
    public static TimeSystem instance;
    [SerializeField]bool startCounting;
    float minutes;
    [SerializeField] private float maxTimer;


    private void Awake()
    {
        if (TimeSystem.instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._OnStartNewDay, KeepCountingTime);
        EventManager.SubscribeToEvent(EventNames._OnEndWorkNewDay, StopCountingTime);
    }

    private void Update()
    {

        if (startCounting)
        {
            minutes += Time.deltaTime;
            if (minutes > maxTimer)
            {
                minutes = 0;
                EventManager.TriggerEvent(EventNames._OnEndWorkNewDay);
            }
        }
    }


    public string GetCurrentFullTime()
    {
        return minutes.ToString("F0");
    }
    
    public string GetCurrentFullTimeInversed()
    {
        return (maxTimer - minutes).ToString("F0");
    } 
    
    public float GetCurrentMinutesTime()
    {
        return minutes;
    }

    public float GetterMaxTimer()
    {
        return maxTimer;
    }
        
    public void SetterMaxTimer(float value)
    {
        maxTimer = value;
    }

    public void KeepCountingTime(params object[] parameters)
    {
        startCounting = true;

    }

    private void StopCountingTime(params object[] parameters)
    {
        startCounting = false;
    }
}
