using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;

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

    public void StartGame()
    {
        switch (SelectedMatchSettings.GameType)
        {
            case GameType.Classic:
                SceneManager.LoadScene("Classic");
                break;

            case GameType.Pongility:
                SceneManager.LoadScene("Pongility");
                break;
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
}