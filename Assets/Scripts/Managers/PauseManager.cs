using UnityEngine;

public class PauseManager : MonoBehaviour
{
    #region Variables
    [Header("Panel References")]
    [SerializeField] private GameObject PausePanel;

    [Header("State")]
    [SerializeField] private bool IsPaused;
    [SerializeField] private bool CanPause = true;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        PausePanel = GameObject.Find("Pause Panel");

        PausePanel.SetActive(false);
    }

    private void OnEnable()
    {
        InputManager.OnPause += HandlePause;
        MatchManager.OnMatchEnded += HandleMatchEnded;
    }

    private void OnDisable()
    {
        InputManager.OnPause -= HandlePause;
        MatchManager.OnMatchEnded -= HandleMatchEnded;
    }
    #endregion

    #region Pause Logic
    private void HandlePause()
    {
        if (CanPause == false) return;

        if (IsPaused == false)
        {
            PauseGame();
        }

        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    private void ResumeGame()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1f;
        IsPaused = false;
    }

    private void HandleMatchEnded()
    {
        CanPause = false;

        if (IsPaused == true)
        {
            ResumeGame();
        }
    }
    #endregion

    #region Button Methods
    public void OnMainMenuButtonPressed()
    {
        ResumeGame();

        GameManager.Instance.HandleMainMenuButtonPressed();
    }

    public void OnRestartButtonPressed()
    {
        ResumeGame();

        GameManager.Instance.HandleRestartButtonPressed();
    }
    #endregion
}