using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject MainMenuPanel;
    [SerializeField] private GameObject TrophiesPanel;
    [SerializeField] private GameObject OptionsPanel;
    [SerializeField] private GameObject ExtrasPanel;
    [SerializeField] private GameObject ModeSelectionPanel;
    [SerializeField] private GameObject GameSelectionPanel;
    [SerializeField] private GameObject MatchSettingsPanel;
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;

    [Header("Match Settings References")]
    [SerializeField] private Button DifficultyButton;
    [SerializeField] private TextMeshProUGUI DifficultyInfoText;
    [SerializeField] private MatchSettingButtonView TimeButtonView;
    [SerializeField] private MatchSettingButtonView ScoreButtonView;
    [SerializeField] private MatchSettingButtonView DifficultyButtonView;

    private void Awake()
    {
        MainMenuPanel = GameObject.Find("Main Menu Panel");
        TrophiesPanel = GameObject.Find("Trophies Panel");
        OptionsPanel = GameObject.Find("Options Panel");
        ExtrasPanel = GameObject.Find("Extras Panel");
        ModeSelectionPanel = GameObject.Find("Mode Selection Panel");
        GameSelectionPanel = GameObject.Find("Game Selection Panel");
        MatchSettingsPanel = GameObject.Find("Match Settings Panel");
        
        DifficultyButton = GameObject.Find("Difficulty Button").GetComponent<Button>();
        DifficultyInfoText = GameObject.Find("Difficulty Info Text").GetComponent<TextMeshProUGUI>();

        TimeButtonView = GameObject.Find("Time Button").GetComponent<MatchSettingButtonView>();
        ScoreButtonView = GameObject.Find("Score Button").GetComponent<MatchSettingButtonView>();
        DifficultyButtonView = GameObject.Find("Difficulty Button").GetComponent<MatchSettingButtonView>();
    }

    void Start()
    {
        if (MainMenuPanel) MainMenuPanel.SetActive(true);
        if (TrophiesPanel) TrophiesPanel.SetActive(false);
        if (OptionsPanel) OptionsPanel.SetActive(false);
        if (ExtrasPanel) ExtrasPanel.SetActive(false);
        if (ModeSelectionPanel) ModeSelectionPanel.SetActive(false);
        if (GameSelectionPanel) GameSelectionPanel.SetActive(false);
        if (MatchSettingsPanel) MatchSettingsPanel.SetActive(false);

        SelectedMatchSettings = FindObjectOfType<SelectedMatchSettings>();

        UpdateDifficultyButtonVisual();
        UpdateDifficultyAvailability();
        UpdateScoreButtonVisual();
        UpdateTimeButtonVisual();
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
        TrophiesPanel.SetActive(true);
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

        UpdateDifficultyAvailability();

        ModeSelectionPanel.SetActive(false);
        GameSelectionPanel.SetActive(true);
    }

    public void OnMultiplayerButtonPressed()
    {
        Debug.Log("Multiplayer button'a basýldý.");

        SelectedMatchSettings.GameMode = GameMode.Multiplayer;

        UpdateDifficultyAvailability();

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

        TrophiesPanel.SetActive(false);
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

    public void OnDifficultyButtonPressed()
    {
        Debug.Log("Difficulty button'a basýldý.");

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

        UpdateScoreButtonVisual();
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

        UpdateTimeButtonVisual();
    }

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
}