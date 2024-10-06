using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup introPanel;
    [SerializeField] private CanvasGroup endDayPanel;

    [SerializeField] private float timerIntro;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._OnEndNewDay, EndDayScreen);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    public void ButtonStartGame()
    {
        LeanTween.alphaCanvas(introPanel, 0, timerIntro)
            .setOnComplete(()=> {
                introPanel.interactable = false;
                introPanel.blocksRaycasts = false;
                });
        EventManager.TriggerEvent(EventNames._OnStartNewDay);
        Cursor.visible = false;
    }


    public void ButtonStartNewDay()
    {
        LeanTween.alphaCanvas(endDayPanel, 0, timerIntro)
                        .setOnComplete(() => {
                            endDayPanel.interactable = false;
                            endDayPanel.blocksRaycasts = false;
                        });
        Cursor.visible = false;
        EventManager.TriggerEvent(EventNames._OnStartNewDay);
    }

    private void EndDayScreen(params object[] parameters)
    {
        LeanTween.alphaCanvas(endDayPanel, 1, timerIntro)
                                    .setOnComplete(() => {
                                        endDayPanel.interactable = true;
                                        endDayPanel.blocksRaycasts = true;
                                    });
        Cursor.visible = true;
    }




}
