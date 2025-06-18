using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class KingFeedbackUI : MonoBehaviour
{
    [SerializeField] private GameObject kingFeedbackPanel;
    [SerializeField] private Image kingIcon;
    [SerializeField] private Sprite happyIcon;
    [SerializeField] private Sprite angryIcon;
    [SerializeField] private TextMeshProUGUI kingFeedbackText;
    [SerializeField] private Button kingContinueButton;
    [SerializeField] private Button kingTryAgainButton;

    public void ShowFeedback(ProgressionManager.KingReactionType reaction, bool isEndOfWeek, int totalScore)
    {
        kingFeedbackPanel.SetActive(true);
        kingContinueButton.gameObject.SetActive(false);
        kingTryAgainButton.gameObject.SetActive(false);

        if (isEndOfWeek)
        {
            if (ProgressionManager.Instance.ShouldRestartWeek(totalScore))
            {
                kingIcon.sprite = angryIcon;
                kingFeedbackText.text = "The King is disappointed. Try again!";
                kingTryAgainButton.onClick.RemoveAllListeners();
                kingTryAgainButton.onClick.AddListener(() =>
                {
                    PlayerPrefs.DeleteKey("CurrentDay");
                    PlayerPrefs.DeleteKey("CurrentWeek");
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                });
                kingTryAgainButton.gameObject.SetActive(true);
            }
            else
            {
                kingIcon.sprite = happyIcon;
                kingFeedbackText.text = "Great work this week!";
                kingContinueButton.onClick.RemoveAllListeners();
                kingContinueButton.onClick.AddListener(() =>
                {
                    UIController.Instance.AdvanceDay(1); // Start new week
                    kingFeedbackPanel.SetActive(false);
                    GameController.Instance.StartLevel();
                });
                kingContinueButton.gameObject.SetActive(true);
            }
            return;
        }

        switch (reaction)
        {
            case ProgressionManager.KingReactionType.Angry:
                kingIcon.sprite = angryIcon;
                kingFeedbackText.text = "The King is angry with the Warlock!";
                break;
            case ProgressionManager.KingReactionType.Happy:
                kingIcon.sprite = happyIcon;
                kingFeedbackText.text = "The King is pleased today.";
                break;
            case ProgressionManager.KingReactionType.None:
                kingFeedbackPanel.SetActive(false);
                GameController.Instance.StartLevel();
                return;
        }

        kingContinueButton.onClick.RemoveAllListeners();
        kingContinueButton.onClick.AddListener(() =>
        {
            kingFeedbackPanel.SetActive(false);
            GameController.Instance.StartLevel();
        });
        kingContinueButton.gameObject.SetActive(true);
    }
}
