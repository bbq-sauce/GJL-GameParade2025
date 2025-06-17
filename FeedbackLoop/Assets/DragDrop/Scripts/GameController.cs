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
    private int dayCount = 0; // Tracks how many times restarted (max 7)
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

        nextDayButton.onClick.AddListener(RestartLevel);
    }

    private void Start()
    {
        ActivateRandomTasks();
        StartLevel();
    }

    private void StartLevel()
    {
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
        scoreText.text = $"Score: {currentScore}";
    }

    public void AddScore(int value)
    {
        currentScore += value;
        UpdateUI();
    }

    private void EndLevel()
    {
        foreach (var slot in slots)
        {
            slot.PenalizeUnassigned();
        }

        totalScore += currentScore;

        endScreenText.text = dayCount >= 7
            ? $"Final Week Score: {totalScore}\n{(totalScore >= 50 ? "Great job!" : "Try again!")}"
            : $"Day {dayCount} Score: {currentScore}";


        nextDayButton.gameObject.SetActive(dayCount < 7); // Only show button if more days left
        endScreen.SetActive(true);
    }

    private void RestartLevel()
    {
        StartLevel();
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
}
