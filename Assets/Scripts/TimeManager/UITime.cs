using UnityEngine;
using UnityEngine.UI;

public class UITime : MonoBehaviour
{
    [SerializeField] RectTransform parentHandler;
    private float currentRotation = 0;

    [Header("ClockAnimation")]
    [SerializeField] Image clockFill;
    [SerializeField] private float timerMax;

    bool canStart = false;


    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._OnStartNewDay, StartDay);
        EventManager.SubscribeToEvent(EventNames._OnEndWorkNewDay, EndTimer);

        
        
    }

    private void Update()
    {
        if (!canStart) return;

        clockFill.fillAmount = TimeSystem.instance.GetCurrentMinutesTime() / timerMax;

        float rotationSpeed = 360 / timerMax;

        float angleToRotate = -rotationSpeed * Time.deltaTime;
        currentRotation += angleToRotate;

        parentHandler.transform.Rotate(Vector3.forward, angleToRotate);
    }

    private void StartDay(params object[] parameters)
    {
        canStart = true;
        timerMax = TimeSystem.instance.GetterMaxTimer();
        currentRotation = 0;
    }

    private void EndTimer(params object[] parameters)
    {
        canStart = false;
    }
}
