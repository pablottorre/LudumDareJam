using UnityEngine;
using UnityEngine.UI;

public class UITime : MonoBehaviour
{
    [Header("ClockAnimation")]
    [SerializeField] Image clockFill;
    [SerializeField] private float timerMax;

    private void Start()
    {
        timerMax = TimeSystem.instance.GetterMaxTimer();
    }

    private void Update()
    {
        clockFill.fillAmount = TimeSystem.instance.GetCurrentMinutesTime() / timerMax;
    }
}
