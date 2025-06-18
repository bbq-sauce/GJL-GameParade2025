using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }
    [Header("Panels")]
    [SerializeField] private GameObject mainMenuPanel;
    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject pauseMenuPanel;
    [SerializeField] private GameObject confirmExitPanel;

    [Header("Buttons")]
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button resumeGameButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;

    [SerializeField] private Button pauseButton;
    [SerializeField] private Button hmbMenucontinueButton;
    [SerializeField] private Button hmbMenuOptionsButton;
    [SerializeField] private Button hmbMenuMainMenuButton;
    [SerializeField] private Button optionsBackButton;
    [SerializeField] private Button pauseYesButton;
    [SerializeField] private Button pauseNoButton;

    [Header("Options")]
    [SerializeField] private Slider audioSlider;
    [SerializeField] private TextMeshProUGUI howToPlayText;

    [Header("Game Info")]
    [SerializeField] private TextMeshProUGUI weekText;
    [SerializeField] private TextMeshProUGUI dayText;

    private int currentWeek = 1;
    private int currentDay = 1;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        LoadProgress();
        SetupButtons();

        if (PlayerPrefs.HasKey("CurrentDay"))
        {
            resumeGameButton.gameObject.SetActive(true);
            newGameButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(false);
        }
        else
        {
            playButton.gameObject.SetActive(true);
            newGameButton.gameObject.SetActive(false);
            resumeGameButton.gameObject.SetActive(false);
        }

        ShowMainMenu();
    }

    void SetupButtons()
    {
        playButton.onClick.AddListener(StartNewGame);
        newGameButton.onClick.AddListener(StartNewGame);
        resumeGameButton.onClick.AddListener(ResumeGame);
        quitButton.onClick.AddListener(QuitGame);
        optionsButton.onClick.AddListener(() => optionsPanel.SetActive(true));
        optionsBackButton.onClick.AddListener(()=> { 
            optionsPanel.SetActive(false);
        });

        pauseButton.onClick.AddListener(ShowPauseMenu);
        hmbMenucontinueButton.onClick.AddListener(HidePauseMenu);
        hmbMenuOptionsButton.onClick.AddListener(() => {
            optionsPanel.SetActive(true);
        }); 
        hmbMenuMainMenuButton.onClick.AddListener(() => confirmExitPanel.SetActive(true));

        pauseNoButton.onClick.AddListener(()=>confirmExitPanel.SetActive(false));
        pauseYesButton.onClick.AddListener(ShowMainMenu);
    }

    void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        gamePanel.SetActive(false);
        optionsPanel.SetActive(false);
        pauseMenuPanel.SetActive(false);
        confirmExitPanel.SetActive(false);
    }

    void StartNewGame()
    {
        currentWeek = 1;
        currentDay = 1;
        UpdateGameInfoText();
        SaveProgress();
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
        GameController.Instance.StartLevel();
    }

    void ResumeGame()
    {
        UpdateGameInfoText();
        mainMenuPanel.SetActive(false);
        gamePanel.SetActive(true);
        Time.timeScale = 1f;
        GameController.Instance.StartLevel();
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    void ShowPauseMenu()
    {
        pauseMenuPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void HidePauseMenu()
    {
        pauseMenuPanel.SetActive(false);
        confirmExitPanel.SetActive(false);
        optionsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ConfirmMainMenuExit(bool confirm)
    {
        if (confirm)
        {
            Time.timeScale = 1f;
            PlayerPrefs.DeleteKey("CurrentDay");
            PlayerPrefs.DeleteKey("CurrentWeek");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            confirmExitPanel.SetActive(false);
        }
    }

    void UpdateGameInfoText()
    {
        weekText.text = $"Week: {currentWeek}";
        dayText.text = $"Day: {currentDay}";
    }

    void SaveProgress()
    {
        PlayerPrefs.SetInt("CurrentDay", currentDay);
        PlayerPrefs.SetInt("CurrentWeek", currentWeek);
        PlayerPrefs.Save();
    }

    void LoadProgress()
    {
        currentDay = PlayerPrefs.GetInt("CurrentDay", 1);
        currentWeek = PlayerPrefs.GetInt("CurrentWeek", 1);
    }

    public void AdvanceDay(int dayCount)
    {
        currentDay=dayCount;
        if (currentDay > 7)
        {
            currentDay = 1;
            currentWeek++;
        }
        SaveProgress();
        UpdateGameInfoText();
    }
}
