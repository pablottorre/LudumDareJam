using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GeneralCR cashRegister;

    [SerializeField] private CanvasGroup introPanel;

    [Header("Normal Panel")]
    [SerializeField] private CanvasGroup normalPanel;
    [SerializeField] private TMP_Text dayTextNormalPanel;

    [Header("End Of Day Panel")]
    [SerializeField] private CanvasGroup endDayPanel;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private TMP_Text savingsText;
    [SerializeField] private TMP_Text earningsText;
    [SerializeField] private TMP_Text milkMoneyText;
    [SerializeField] private TMP_Text milkAmountText;
    [SerializeField] private TMP_Text candyMoneyText;
    [SerializeField] private TMP_Text candyAmountText;


    [SerializeField] private TMP_Text costOfDayText;
    [SerializeField] private TMP_Text totalText;

    [SerializeField] private float timerIntro;


    [SerializeField] private GameObject normalButton;
    [SerializeField] private GameObject loseButton;

    [Header("Lose Game Panel")]
    [SerializeField] private CanvasGroup loseGamePanel;
    [SerializeField] private TMP_Text endOfGameDaysText;

    private void Start()
    {
        EventManager.SubscribeToEvent(EventNames._OnStartNewDay, StartNewDay);
        EventManager.SubscribeToEvent(EventNames._OnEndNewDay, EndDayScreen);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ButtonQuitGame()
    {
        Application.Quit();
    }


    public void ButtonStartGame()
    {
        LeanTween.alphaCanvas(introPanel, 0, timerIntro)
            .setOnComplete(() =>
            {
                introPanel.interactable = false;
                introPanel.blocksRaycasts = false;
                LeanTween.alphaCanvas(normalPanel, 1, timerIntro);
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
                            LeanTween.alphaCanvas(normalPanel, 1, timerIntro);
                        });
        Cursor.visible = false;
        EventManager.TriggerEvent(EventNames._OnStartNewDay);
    }

    public void ButtonEndTheGame()
    {
        endOfGameDaysText.text = GameManager.Instance.GetterNumberDay().ToString();

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

        int milkAmount = cashRegister.GetterTotalMilks();
        int candyAmount = cashRegister.GetterTotalCandys();

        milkAmountText.text = "(" + milkAmount + ")";
        candyAmountText.text = "(" + candyAmount + ")";


        milkMoneyText.text = "$" + (milkAmount * 5).ToString();
        candyMoneyText.text = "$" + (candyAmount).ToString();

        costOfDayText.text = "$" + GameManager.Instance.GetterCosts().ToString();
        totalText.text = "$" + GameManager.Instance.GetterNewTotal().ToString();

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

    private void StartNewDay(params object[] parameters)
    {
        dayTextNormalPanel.text = GameManager.Instance.GetterNumberDay().ToString();
    }


}
