using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;

    [SerializeField] private bool IsPaused;
    [SerializeField] private bool CanPause = true;


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

    private void Awake()
    {
        PausePanel = GameObject.Find("Pause Panel");

        PausePanel.SetActive(false);
    }

    private void HandlePause()
    {
        if (CanPause == false) return;
        
        if (!IsPaused)
        {
            PausePanel.SetActive(true);
            Time.timeScale = 0;
            IsPaused = true;
        }

        else
        {
            PausePanel.SetActive(false);
            Time.timeScale = 1;
            IsPaused = false;
        }
    }

    private void HandleMatchEnded()
    {
        CanPause = false;
    }

    public void OnMainMenuButtonPressed()
    {
        IsPaused = false;
        Time.timeScale = 1;

        GameManager.Instance.HandleMainMenuButtonPressed();
    }

    public void OnRestartButtonPressed()
    {
        IsPaused = false;
        Time.timeScale = 1;

        GameManager.Instance.HandleRestartButtonPressed();
    }
}