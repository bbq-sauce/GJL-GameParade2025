using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Collections;

public class PanelSequenceController : MonoBehaviour
{
    [System.Serializable]
    public class PanelData
    {
        public CanvasGroup panelCanvasGroup;
        public TextMeshProUGUI dialogueText;
        [TextArea] public string fullText;
    }

    [SerializeField] private PanelData[] panelDataList;
    [SerializeField] private Button nextButton;

    private int currentPanelIndex = 0;
    private TypeWriterText typeWriter;

    private void Awake()
    {
        typeWriter = gameObject.AddComponent<TypeWriterText>();
    }

    private void OnEnable()
    {
        currentPanelIndex = 0;

        foreach (var panel in panelDataList)
        {
            panel.panelCanvasGroup.alpha = 0;
            panel.panelCanvasGroup.transform.localScale = Vector3.one;
            panel.panelCanvasGroup.gameObject.SetActive(false);
            panel.dialogueText.text = "";
        }

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(ShowNextPanel);

        ShowNextPanel(); // Start with the first panel
    }

    void ShowNextPanel()
    {
        Time.timeScale = 1;
        if (currentPanelIndex >= panelDataList.Length)
        {
            nextButton.gameObject.SetActive(false);

            // Delay and then start the game
            StartCoroutine(EndCinematicAndStartGame());
            return;
        }

        if (currentPanelIndex > 0)
        {
            var prev = panelDataList[currentPanelIndex - 1].panelCanvasGroup;
            prev.DOFade(0, 0.5f).OnComplete(() => prev.gameObject.SetActive(false));
        }

        var panel = panelDataList[currentPanelIndex];
        panel.panelCanvasGroup.alpha = 0;
        panel.panelCanvasGroup.transform.localScale = Vector3.one * 1.1f;
        panel.panelCanvasGroup.gameObject.SetActive(true);

        Sequence seq = DOTween.Sequence();
        seq.Append(panel.panelCanvasGroup.DOFade(1f, 1f));
        seq.Join(panel.panelCanvasGroup.transform.DOScale(Vector3.one, 4f).SetEase(Ease.OutQuad));
        seq.OnComplete(() => StartCoroutine(typeWriter.PlayTypingEffect(panel.dialogueText, panel.fullText)));

        currentPanelIndex++;
    }

    private IEnumerator EndCinematicAndStartGame()
    {
        yield return new WaitForSeconds(1f); // Optional small delay
        this.gameObject.SetActive(false);
        UIController.Instance.StartNewGame();
    }

}
