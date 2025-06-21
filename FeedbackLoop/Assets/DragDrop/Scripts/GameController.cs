using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public float levelDuration = 150f;
    private float timer;
    private int currentScore = 0;
    private int totalScore = 0;
    private int dayCount = 0; 

    private int warlockFails = 0;
    private int clericFails = 0;
    private int warlockSuccesses = 0;
    private int clericSuccesses = 0;

    [SerializeField] private RectTransform rotatingImage;


    [Header("UI References")]
    public TextMeshProUGUI scoreText;
    public GameObject endScreen;
    public TextMeshProUGUI endScreenText;
    public Button nextDayButton;

    [Header("Character References")]
    public DragAndDrop warlock;
    public DragAndDrop cleric;
    public Transform warlockStartPos;
    public Transform clericStartPos;

    [Header("Task Slots")]
    [SerializeField] private TaskSlot[] slots;
    [SerializeField] private TextMeshProUGUI warlockTaskText;
    [SerializeField] private TextMeshProUGUI clericTaskText;

    private void Awake()
    {
        if (Instance == null) Instance = this;

        nextDayButton.onClick.AddListener(ShowKingReaction);
    }

    private void Start()
    {
        ActivateRandomTasks();
        Time.timeScale = 0f;
    }

    private void ResetFailsAndSuccesses()
    {
        warlockFails = 0;
        clericFails = 0;
        warlockSuccesses = 0;
        clericSuccesses = 0;
    }

    public void StartLevel()
    {
        ResetFailsAndSuccesses();
        ActivateRandomTasks();

        timer = levelDuration;
        currentScore = 0;
        dayCount++;

        warlockTaskText.text = "Warlock: ";
        clericTaskText.text = "Cleric: ";

        ResetCharacters();
        ResetTaskSlots();
        UpdateUI();

        endScreen.SetActive(false);
        StartCoroutine(LevelTimer());
        PrintCharacterStats();  
    }
    private void ActivateRandomTasks()
    {
        // Make sure all are deactivated first
        foreach (var slot in slots)
        {
            slot.SetActiveState(false);
        }

        // Shuffle slots
        List<TaskSlot> shuffled = new List<TaskSlot>(slots);
        for (int i = 0; i < shuffled.Count; i++)
        {
            int randIndex = Random.Range(i, shuffled.Count);
            (shuffled[i], shuffled[randIndex]) = (shuffled[randIndex], shuffled[i]);
        }

        // Activate the first 9
        for (int i = 0; i < 9; i++)
        {
            shuffled[i].SetActiveState(true);
        }
    }

    private IEnumerator LevelTimer()
    {
        float elapsed = 0f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / levelDuration);
            float rotationZ = 360f * progress;
            rotatingImage.localEulerAngles = new Vector3(0, 0, rotationZ); // clockwise

            yield return null;
        }

        rotatingImage.localEulerAngles = Vector3.zero; // Reset to original (optional)
        EndLevel();
    }


    private void UpdateUI()
    {
        scoreText.text = $"Coins: {currentScore}";
    }

    public void AddScore(int value)
    {
        currentScore += value;
        UpdateUI();
    }

    private void EndLevel()
    {
        Debug.Log("Daycount : \n" + dayCount);
        foreach (var slot in slots)
        {
            slot.PenalizeUnassigned();
        }

        totalScore += currentScore;

        endScreenText.text = dayCount >= 5
            ? $"Final Week Score: {totalScore}\n{(totalScore >= 50 ? "Great job!" : "Try again!")}"
            : $"Day {dayCount} Score:\n {currentScore}";

        UIController.Instance.AdvanceDay(dayCount + 1);
        endScreen.SetActive(true);
        nextDayButton.gameObject.SetActive(true);

        if (dayCount == 5)
        {
            ProgressionManager.Instance.ApplyWeeklyProgression();
            dayCount = 0;
        }
            
    }

    private void RestartLevel()
    {
        StartLevel();
    }

    private void ShowKingReaction()
    {
        var kingReaction = ProgressionManager.Instance.GetKingReaction(ProgressionManager.Instance.warlockStats.TasksFailed);

        UIController.Instance.ShowKingFeedbackPanel(kingReaction, dayCount >= 5, totalScore);
    }
    private void ResetCharacters()
    {
        warlock.transform.SetParent(warlockStartPos.parent);
        warlock.GetComponent<RectTransform>().anchoredPosition = warlockStartPos.GetComponent<RectTransform>().anchoredPosition;
        warlock.ShowProgressBar(false);
        warlock.LockDragging(false);

        cleric.transform.SetParent(clericStartPos.parent);
        cleric.GetComponent<RectTransform>().anchoredPosition = clericStartPos.GetComponent<RectTransform>().anchoredPosition;
        cleric.ShowProgressBar(false);
        cleric.LockDragging(false);
    }

    private void ResetTaskSlots()
    {
        foreach (var slot in slots)
        {
            slot.ResetSlot();
        }
    }

    public void SetTaskAssignment(string characterName, string taskName)
    {
        if (characterName == "Warlock")
            warlockTaskText.text = $"Warlock: {taskName}";
        else if (characterName == "Cleric")
            clericTaskText.text = $"Cleric: {taskName}";
    }

    private void PrintCharacterStats()
    {
        Debug.Log($"[Warlock Stats] Success Rate: {ProgressionManager.Instance.warlockStats.CurrentSuccessRate} | Task Time: {ProgressionManager.Instance.warlockStats.CurrentTimeToDoTask}");
        Debug.Log($"[Cleric Stats] Success Rate: {ProgressionManager.Instance.clericStats.CurrentSuccessRate} | Task Time: {ProgressionManager.Instance.clericStats.CurrentTimeToDoTask}");
    }

}
