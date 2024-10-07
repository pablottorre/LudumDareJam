using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup introPanel;

    [Header("End Of Day Panel")]
    [SerializeField] private CanvasGroup endDayPanel;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text savingsText;
    [SerializeField] private TMP_Text earningsText;
    [SerializeField] private TMP_Text costOfDayText;
    [SerializeField] private TMP_Text newSavingsText;

    [SerializeField] private float timerIntro;


    [SerializeField] private GameObject normalButton;
    [SerializeField] private GameObject loseButton;

    [Header("Lose Game Panel")]
    [SerializeField] private CanvasGroup loseGamePanel;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._OnEndNewDay, EndDayScreen);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }


    public void ButtonStartGame()
    {
        LeanTween.alphaCanvas(introPanel, 0, timerIntro)
            .setOnComplete(() =>
            {
                introPanel.interactable = false;
                introPanel.blocksRaycasts = false;
            });
        EventManager.TriggerEvent(EventNames._OnStartNewDay);
        Cursor.visible = false;
    }


    public void ButtonStartNewDay()
    {
        LeanTween.alphaCanvas(endDayPanel, 0, timerIntro)
                        .setOnComplete(() =>
                        {
                            endDayPanel.interactable = false;
                            endDayPanel.blocksRaycasts = false;
                        });
        Cursor.visible = false;
        EventManager.TriggerEvent(EventNames._OnStartNewDay);
    }

    public void ButtonEndTheGame()
    {
        LeanTween.alphaCanvas(loseGamePanel, 1, timerIntro)
                                   .setOnComplete(() =>
                                   {
                                       loseGamePanel.interactable = true;
                                       loseGamePanel.blocksRaycasts = true;
                                   });
        Cursor.visible = true;
    }

    private void EndDayScreen(params object[] parameters)
    {
        dayText.text = "Day " + GameManager.Instance.GetterNumberDay().ToString();
        savingsText.text = "$" + GameManager.Instance.GetterSaving().ToString();
        earningsText.text = "$" + GameManager.Instance.GetterEarnings().ToString();
        costOfDayText.text = "$" + GameManager.Instance.GetterCosts().ToString();
        newSavingsText.text = "$" + GameManager.Instance.GetterSaving().ToString();

        LeanTween.alphaCanvas(endDayPanel, 1, timerIntro)
                                    .setOnComplete(() =>
                                    {
                                        endDayPanel.interactable = true;
                                        endDayPanel.blocksRaycasts = true;
                                    });
        Cursor.visible = true;

        if (GameManager.Instance.playerLoseTheGame)
        {
            loseButton.SetActive(true);
            normalButton.SetActive(false);
        }
        else
        {
            normalButton.SetActive(true);
            loseButton.SetActive(false);
        }

    }




}
