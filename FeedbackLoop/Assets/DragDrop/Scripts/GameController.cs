using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
public class GameController : MonoBehaviour
{

    public static GameController Instance;

    public float levelDuration = 150f; // 2.5 minutes
    private float timer;
    private int score = 0;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject endScreen;

    [SerializeField] private TaskSlot[] slots;
    [SerializeField] private TextMeshProUGUI warlockTaskText;
    [SerializeField] private TextMeshProUGUI clericTaskText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        timer = levelDuration;
        ActivateRandomTasks();

        StartCoroutine(LevelTimer());
        UpdateUI();
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
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            UpdateUI();
            yield return null;
        }

        EndLevel();
    }

    private void UpdateUI()
    {
        timerText.text = $"Time: {Mathf.CeilToInt(timer)}s";
        scoreText.text = $"Score: {score}";
    }

    public void AddScore(int value)
    {
        score += value;
        UpdateUI();
    }

    private void EndLevel()
    {
        foreach (var slot in slots)
        {
            slot.PenalizeUnassigned();
        }

        endScreen.SetActive(true);
        endScreen.GetComponentInChildren<Text>().text = "Final Score: " + score;
    }
    public void SetTaskAssignment(string characterName, string taskName)
    {
        Debug.Log(characterName + " " + taskName);
        if (characterName == "Warlock")
            warlockTaskText.text = $"Warlock: {taskName}";
        else if (characterName == "Cleric")
            clericTaskText.text = $"Cleric: {taskName}";
    }

}
