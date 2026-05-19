using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Variables
    [Header("Panel References")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject TrophiesPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject ExtrasPanel;
    [SerializeField] private GameObject ModeSelectionPanel;
    [SerializeField] private GameObject GameSelectionPanel;
    [SerializeField] private GameObject MatchSettingsPanel;

    [Header("Match Settings References")]
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;
    [SerializeField] private Button DifficultyButton;
    [SerializeField] private TextMeshProUGUI DifficultyInfoText;
    [SerializeField] private MatchSettingButtonView TimeButtonView;
    [SerializeField] private MatchSettingButtonView ScoreButtonView;
    [SerializeField] private MatchSettingButtonView DifficultyButtonView;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        FindPanelReferences();
        FindMatchSettingsReferences();
    }

    private void Start()
    {
        SelectedMatchSettings = FindFirstObjectByType<SelectedMatchSettings>();

        ShowPanel(MainMenuPanel);

        UpdateDifficultyButtonVisual();
        UpdateDifficultyAvailability();
        UpdateScoreButtonVisual();
        UpdateTimeButtonVisual();
    }
    #endregion

    #region Main Menu Buttons
    public void OnPlayButtonPressed()
    {
        Debug.Log("Play button pressed.");

        ShowPanel(ModeSelectionPanel);
    }

    public void OnAchievementsButtonPressed()
    {
        Debug.Log("Achievements button pressed.");

        ShowPanel(TrophiesPanel);
    }

    public void OnOptionsButtonPressed()
    {
        Debug.Log("Options button pressed.");

        ShowPanel(OptionsPanel);
    }

    public void OnExtrasButtonPressed()
    {
        Debug.Log("Extras button pressed.");

        ShowPanel(ExtrasPanel);
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
    #endregion

    #region Back Buttons
    public void OnBackFromAchievementsPressed()
    {
        Debug.Log("Achievements back button pressed.");

        ShowPanel(MainMenuPanel);
    }

    public void OnBackFromOptionsPressed()
    {
        Debug.Log("Options back button pressed.");

        ShowPanel(MainMenuPanel);
    }

    public void OnBackFromExtrasPressed()
    {
        Debug.Log("Extras back button pressed.");

        ShowPanel(MainMenuPanel);
    }

    public void OnBackFromModeSelectionPressed()
    {
        Debug.Log("Mode Selection back button pressed.");

        ShowPanel(MainMenuPanel);
    }

    public void OnBackFromGameSelectionPressed()
    {
        Debug.Log("Game Selection back button pressed.");

        ShowPanel(ModeSelectionPanel);
    }

    public void OnBackFromMatchSettingsPressed()
    {
        Debug.Log("Match Settings back button pressed.");

        ShowPanel(GameSelectionPanel);
    }
    #endregion

    #region Match Selection Buttons
    public void OnSingleplayerButtonPressed()
    {
        Debug.Log("Singleplayer button pressed.");

        SelectedMatchSettings.GameMode = GameMode.Singleplayer;

        UpdateDifficultyAvailability();

        ShowPanel(GameSelectionPanel);
    }

    public void OnMultiplayerButtonPressed()
    {
        Debug.Log("Multiplayer button pressed.");

        SelectedMatchSettings.GameMode = GameMode.Multiplayer;

        UpdateDifficultyAvailability();

        ShowPanel(GameSelectionPanel);
    }

    public void OnPongilityButtonPressed()
    {
        Debug.Log("Pongility button pressed.");

        SelectedMatchSettings.GameType = GameType.Pongility;

        ShowPanel(MatchSettingsPanel);
    }

    public void OnClassicButtonPressed()
    {
        Debug.Log("Classic button pressed.");

        SelectedMatchSettings.GameType = GameType.Classic;

        ShowPanel(MatchSettingsPanel);
    }

    public void OnStartButtonPressed()
    {
        Debug.Log("Start button pressed.");

        GameManager.Instance.StartGame();
    }
    #endregion

    #region Match Setting Buttons
    public void OnDifficultyButtonPressed()
    {
        Debug.Log("Difficulty button pressed.");

        switch (SelectedMatchSettings.Difficulty)
        {
            case Difficulty.Easy:
                SelectedMatchSettings.Difficulty = Difficulty.Normal;
                break;

            case Difficulty.Normal:
                SelectedMatchSettings.Difficulty = Difficulty.Hard;
                break;

            case Difficulty.Hard:
                SelectedMatchSettings.Difficulty = Difficulty.Insane;
                break;

            case Difficulty.Insane:
                SelectedMatchSettings.Difficulty = Difficulty.Easy;
                break;
        }

        UpdateDifficultyButtonVisual();
    }

    public void OnScoreButtonPressed()
    {
        Debug.Log("Score button pressed.");

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

        UpdateScoreButtonVisual();
    }

    public void OnTimeButtonPressed()
    {
        Debug.Log("Time button pressed.");

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

        UpdateTimeButtonVisual();
    }
    #endregion

    #region Visual Updates
    private void UpdateDifficultyButtonVisual()
    {
        int DifficultyVisualIndex = GetDifficultyVisualIndex();

        DifficultyButtonView.ApplyVisual(DifficultyVisualIndex);
    }

    private int GetDifficultyVisualIndex()
    {
        if (SelectedMatchSettings.Difficulty == Difficulty.Easy)
        {
            return 0;
        }

        if (SelectedMatchSettings.Difficulty == Difficulty.Normal)
        {
            return 1;
        }

        if (SelectedMatchSettings.Difficulty == Difficulty.Hard)
        {
            return 2;
        }

        if (SelectedMatchSettings.Difficulty == Difficulty.Insane)
        {
            return 3;
        }

        return 0;
    }

    private void UpdateScoreButtonVisual()
    {
        int ScoreVisualIndex = GetScoreVisualIndex();

        ScoreButtonView.ApplyVisual(ScoreVisualIndex);
    }

    private int GetScoreVisualIndex()
    {
        if (SelectedMatchSettings.TargetScore == 0)
        {
            return 0;
        }

        if (SelectedMatchSettings.TargetScore == 5)
        {
            return 1;
        }

        if (SelectedMatchSettings.TargetScore == 10)
        {
            return 2;
        }

        if (SelectedMatchSettings.TargetScore == 15)
        {
            return 3;
        }

        if (SelectedMatchSettings.TargetScore == 20)
        {
            return 4;
        }

        return 0;
    }

    private void UpdateTimeButtonVisual()
    {
        int TimeVisualIndex = GetTimeVisualIndex();

        TimeButtonView.ApplyVisual(TimeVisualIndex);
    }

    private int GetTimeVisualIndex()
    {
        if (SelectedMatchSettings.MatchDurationSeconds == 0f)
        {
            return 0;
        }

        if (SelectedMatchSettings.MatchDurationSeconds == 300f)
        {
            return 1;
        }

        if (SelectedMatchSettings.MatchDurationSeconds == 600f)
        {
            return 2;
        }

        if (SelectedMatchSettings.MatchDurationSeconds == 900f)
        {
            return 3;
        }

        if (SelectedMatchSettings.MatchDurationSeconds == 1200f)
        {
            return 4;
        }

        return 0;
    }

    private void UpdateDifficultyAvailability()
    {
        if (SelectedMatchSettings.GameMode == GameMode.Multiplayer)
        {
            DifficultyButton.interactable = false;
            DifficultyButtonView.SetVisualEnabled(false);

            DifficultyInfoText.gameObject.SetActive(true);
            DifficultyInfoText.text = "Trying to set your rival's difficulty?";
        }

        else
        {
            DifficultyButton.interactable = true;
            DifficultyButtonView.SetVisualEnabled(true);

            DifficultyInfoText.gameObject.SetActive(false);

            UpdateDifficultyButtonVisual();
        }
    }
    #endregion

    #region Helper Methods
    private void FindPanelReferences()
    {
        MainMenuPanel = GameObject.Find("Main Menu Panel");
        TrophiesPanel = GameObject.Find("Trophies Panel");
        OptionsPanel = GameObject.Find("Options Panel");
        ExtrasPanel = GameObject.Find("Extras Panel");
        ModeSelectionPanel = GameObject.Find("Mode Selection Panel");
        GameSelectionPanel = GameObject.Find("Game Selection Panel");
        MatchSettingsPanel = GameObject.Find("Match Settings Panel");
    }

    private void FindMatchSettingsReferences()
    {
        GameObject DifficultyButtonObject = GameObject.Find("Difficulty Button");

        DifficultyButton = DifficultyButtonObject.GetComponent<Button>();
        DifficultyButtonView = DifficultyButtonObject.GetComponent<MatchSettingButtonView>();

        DifficultyInfoText = GameObject.Find("Difficulty Info Text").GetComponent<TextMeshProUGUI>();

        TimeButtonView = GameObject.Find("Time Button").GetComponent<MatchSettingButtonView>();
        ScoreButtonView = GameObject.Find("Score Button").GetComponent<MatchSettingButtonView>();
    }

    private void ShowPanel(GameObject PanelToShow)
    {
        MainMenuPanel.SetActive(false);
        TrophiesPanel.SetActive(false);
        OptionsPanel.SetActive(false);
        ExtrasPanel.SetActive(false);
        ModeSelectionPanel.SetActive(false);
        GameSelectionPanel.SetActive(false);
        MatchSettingsPanel.SetActive(false);

        PanelToShow.SetActive(true);
    }
    #endregion
}