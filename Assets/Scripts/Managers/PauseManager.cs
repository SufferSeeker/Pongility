using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject PausePanel;

    [SerializeField] private bool IsPaused;


    private void OnEnable()
    {
        InputManager.OnPause += HandlePause;
    }

    private void OnDisable()
    {
        InputManager.OnPause -= HandlePause;
    }

    private void Awake()
    {
        PausePanel = GameObject.Find("Pause Panel");

        PausePanel.SetActive(false);
    }

    private void HandlePause()
    {
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