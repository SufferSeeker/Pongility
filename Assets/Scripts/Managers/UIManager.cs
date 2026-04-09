using UnityEngine;

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

    private void Awake()
    {
        MainMenuPanel = GameObject.Find("Main Menu Panel");
        AchievementsPanel = GameObject.Find("Achievements Panel");
        OptionsPanel = GameObject.Find("Options Panel");
        ExtrasPanel = GameObject.Find("Extras Panel");
        ModeSelectionPanel = GameObject.Find("Mode Selection Panel");
        GameSelectionPanel = GameObject.Find("Game Selection Panel");
        MatchSettingsPanel = GameObject.Find("Match Settings Panel");
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

    public void OnGameSelectionButtonPressed()
    {
        Debug.Log("Game Selection button'a basýldý.");

        ModeSelectionPanel.SetActive(false);
        GameSelectionPanel.SetActive(true);
    }

    public void OnMatchSettingButtonPressed()
    {
        Debug.Log("Match Setting button'a basýldý.");

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
}