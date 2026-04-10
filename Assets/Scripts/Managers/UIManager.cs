using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject AchievementsPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject ExtrasPanel;
    [SerializeField] private GameObject ModeSelectionPanel;
    [SerializeField] private GameObject GameSelectionPanel;
    [SerializeField] private GameObject MatchSettingsPanel;
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;

    [Header("Match Settings Texts")]
    [SerializeField] private TextMeshProUGUI EnemyDifficultyText;
    [SerializeField] private TextMeshProUGUI ScoreText;
    [SerializeField] private TextMeshProUGUI TimeText;
    [SerializeField] private Button EnemyDifficultyButton;
    [SerializeField] private TextMeshProUGUI EnemyDifficultyInfoText;

    private void Awake()
    {
        MainMenuPanel = GameObject.Find("Main Menu Panel");
        AchievementsPanel = GameObject.Find("Achievements Panel");
        OptionsPanel = GameObject.Find("Options Panel");
        ExtrasPanel = GameObject.Find("Extras Panel");
        ModeSelectionPanel = GameObject.Find("Mode Selection Panel");
        GameSelectionPanel = GameObject.Find("Game Selection Panel");
        MatchSettingsPanel = GameObject.Find("Match Settings Panel");

        EnemyDifficultyButton = GameObject.Find("Enemy Difficulty Button").GetComponent<Button>();
        EnemyDifficultyInfoText = GameObject.Find("Enemy Difficulty Info Text").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        if (MainMenuPanel) MainMenuPanel.SetActive(true);
        if (AchievementsPanel) AchievementsPanel.SetActive(false);
        if (OptionsPanel) OptionsPanel.SetActive(false);
        if (ExtrasPanel) ExtrasPanel.SetActive(false);
        if (ModeSelectionPanel) ModeSelectionPanel.SetActive(false);
        if (GameSelectionPanel) GameSelectionPanel.SetActive(false);
        if (MatchSettingsPanel) MatchSettingsPanel.SetActive(false);

        SelectedMatchSettings = FindObjectOfType<SelectedMatchSettings>();

        UpdateEnemyDifficultyText();
        UpdateEnemyDifficultyAvailability();
        UpdateScoreText();
        UpdateTimeText();
    }

    public void OnPlayButtonPressed()
    {
        Debug.Log("Play button'a basýldý.");

        MainMenuPanel.SetActive(false);
        ModeSelectionPanel.SetActive(true);
    }

    public void OnAchievementsButtonPressed()
    {
        Debug.Log("Achievements button'a basýldý.");

        MainMenuPanel.SetActive(false);
        AchievementsPanel.SetActive(true);
    }

    public void OnOptionsButtonPressed()
    {
        Debug.Log("Options button'a basýldý.");

        MainMenuPanel.SetActive(false);
        OptionsPanel.SetActive(true);
    }

    public void OnExtrasButtonPressed()
    {
        Debug.Log("Extras button'a basýldý.");

        MainMenuPanel.SetActive(false);
        ExtrasPanel.SetActive(true);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }

    public void OnSingleplayerButtonPressed()
    {
        Debug.Log("Singleplayer button'a basýldý.");

        SelectedMatchSettings.GameMode = GameMode.Singleplayer;

        UpdateEnemyDifficultyAvailability();

        ModeSelectionPanel.SetActive(false);
        GameSelectionPanel.SetActive(true);
    }

    public void OnMultiplayerButtonPressed()
    {
        Debug.Log("Multiplayer button'a basýldý.");

        SelectedMatchSettings.GameMode = GameMode.Multiplayer;

        UpdateEnemyDifficultyAvailability();

        ModeSelectionPanel.SetActive(false);
        GameSelectionPanel.SetActive(true);
    }

    public void OnPongilityButtonPressed()
    {
        Debug.Log("Pongility button'a basýldý.");

        SelectedMatchSettings.GameType = GameType.Pongility;

        GameSelectionPanel.SetActive(false);
        MatchSettingsPanel.SetActive(true);
    }

    public void OnClassicButtonPressed()
    {
        Debug.Log("Classic button'a basýldý.");

        SelectedMatchSettings.GameType = GameType.Classic;

        GameSelectionPanel.SetActive(false);
        MatchSettingsPanel.SetActive(true);
    }

    public void OnStartButtonPressed()
    {
        Debug.Log("Start button'a basýldý.");

        MatchSettingsPanel.SetActive(false);

        GameManager.Instance.StartGame();
    }

    public void OnBackFromAchievementsPressed()
    {
        Debug.Log("Achievements back button'a basýldý.");

        AchievementsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void OnBackFromOptionsPressed()
    {
        Debug.Log("Options back button'a basýldý.");

        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void OnBackFromExtrasPressed()
    {
        Debug.Log("Extras back button'a basýldý.");

        ExtrasPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void OnBackFromModeSelectionPressed()
    {
        Debug.Log("Mode Selection back button'a basýldý.");

        ModeSelectionPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void OnBackFromGameSelectionPressed()
    {
        Debug.Log("Game Selection back button'a basýldý.");

        GameSelectionPanel.SetActive(false);
        ModeSelectionPanel.SetActive(true);
    }

    public void OnBackFromMatchSettingsPressed()
    {
        Debug.Log("Match Settings back button'a basýldý.");

        MatchSettingsPanel.SetActive(false);
        GameSelectionPanel.SetActive(true);
    }

    public void OnEnemyDifficultyButtonPressed()
    {
        Debug.Log("EnemyDifficulty button'a basýldý.");

        switch (SelectedMatchSettings.EnemyDifficulty)
        {
            case EnemyDifficulty.Easy:
                SelectedMatchSettings.EnemyDifficulty = EnemyDifficulty.Medium;
                break;

            case EnemyDifficulty.Medium:
                SelectedMatchSettings.EnemyDifficulty = EnemyDifficulty.Hard;
                break;

            case EnemyDifficulty.Hard:
                SelectedMatchSettings.EnemyDifficulty = EnemyDifficulty.VeryHard;
                break;

            case EnemyDifficulty.VeryHard:
                SelectedMatchSettings.EnemyDifficulty = EnemyDifficulty.Insane;
                break;

            case EnemyDifficulty.Insane:
                SelectedMatchSettings.EnemyDifficulty = EnemyDifficulty.Easy;
                break;
        }

        UpdateEnemyDifficultyText();
    }

    public void OnScoreButtonPressed()
    {
        Debug.Log("Score button'a basýldý.");

        if (SelectedMatchSettings.TargetScore == 5)
        {
            SelectedMatchSettings.TargetScore = 10;
        }

        else if (SelectedMatchSettings.TargetScore == 10)
        {
            SelectedMatchSettings.TargetScore = 15;
        }

        else if (SelectedMatchSettings.TargetScore == 15)
        {
            SelectedMatchSettings.TargetScore = 20;
        }

        else if (SelectedMatchSettings.TargetScore == 20)
        {
            SelectedMatchSettings.TargetScore = 0;
        }

        else
        {
            SelectedMatchSettings.TargetScore = 5;
        }

        UpdateScoreText();
    }

    public void OnTimeButtonPressed()
    {
        Debug.Log("Time button'a basýldý.");

        if (SelectedMatchSettings.MatchDurationSeconds == 300f)
        {
            SelectedMatchSettings.MatchDurationSeconds = 600f;
        }

        else if (SelectedMatchSettings.MatchDurationSeconds == 600f)
        {
            SelectedMatchSettings.MatchDurationSeconds = 900f;
        }

        else if (SelectedMatchSettings.MatchDurationSeconds == 900f)
        {
            SelectedMatchSettings.MatchDurationSeconds = 1200f;
        }

        else if (SelectedMatchSettings.MatchDurationSeconds == 1200f)
        {
            SelectedMatchSettings.MatchDurationSeconds = 0f;
        }

        else
        {
            SelectedMatchSettings.MatchDurationSeconds = 300f;
        }

        UpdateTimeText();
    }

    private void UpdateEnemyDifficultyText()
    {
        EnemyDifficultyText.text = "Enemy Difficulty: " + GetFormattedEnemyDifficultyText();
    }

    private string GetFormattedEnemyDifficultyText()
    {
        switch (SelectedMatchSettings.EnemyDifficulty)
        {
            case EnemyDifficulty.Easy:
                return "Easy";

            case EnemyDifficulty.Medium:
                return "Medium";

            case EnemyDifficulty.Hard:
                return "Hard";

            case EnemyDifficulty.VeryHard:
                return "Very Hard";

            case EnemyDifficulty.Insane:
                return "Insane";
        }

        return "Unknown";
    }

    private void UpdateScoreText()
    {
        if (SelectedMatchSettings.TargetScore == 0)
        {
            ScoreText.text = "Score: No Limit";
        }

        else
        {
            ScoreText.text = "Score: " + SelectedMatchSettings.TargetScore.ToString();
        }
    }

    private void UpdateTimeText()
    {
        if (SelectedMatchSettings.MatchDurationSeconds == 0f)
        {
            TimeText.text = "Time: No Limit";
        }

        else
        {
            int Minutes = (int)(SelectedMatchSettings.MatchDurationSeconds / 60f);

            TimeText.text = "Time: " + Minutes.ToString() + " Min";
        }
    }

    private void UpdateEnemyDifficultyAvailability()
    {
        if (SelectedMatchSettings.GameMode == GameMode.Multiplayer)
        {
            EnemyDifficultyButton.interactable = false;

            EnemyDifficultyText.text = "Enemy Difficulty: Disabled In Multiplayer";

            EnemyDifficultyInfoText.gameObject.SetActive(true);
            EnemyDifficultyInfoText.text = "Trying to set your rival's difficulty?";
        }

        else
        {
            EnemyDifficultyButton.interactable = true;

            EnemyDifficultyInfoText.gameObject.SetActive(false);

            UpdateEnemyDifficultyText();
        }
    }
}