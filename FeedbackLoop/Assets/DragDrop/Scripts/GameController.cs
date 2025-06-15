using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
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
        StartCoroutine(LevelTimer());
        UpdateUI();
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
