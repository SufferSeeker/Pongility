using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SelectedMatchSettings = FindObjectOfType<SelectedMatchSettings>();
    }
    #endregion

    #region Scene Management
    public void StartGame()
    {
        if (SelectedMatchSettings.GameType == GameType.Classic)
        {
            SceneManager.LoadScene("Classic");
        }

        else if (SelectedMatchSettings.GameType == GameType.Pongility)
        {
            SceneManager.LoadScene("Pongility");
        }
    }

    public void HandleMainMenuButtonPressed()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void HandleRestartButtonPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}