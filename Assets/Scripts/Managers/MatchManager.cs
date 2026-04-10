using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;

    [SerializeField] private GameObject PlayerRacket2;
    [SerializeField] private GameObject EnemyRacket;

    [Header("UI")]
    [SerializeField] private GameObject MatchResultPanel;

    [SerializeField] private TextMeshProUGUI Player1ResultText;
    [SerializeField] private TextMeshProUGUI Player2ResultText;
    [SerializeField] private TextMeshProUGUI MatchDurationText;
    [SerializeField] private TextMeshProUGUI WinnerText;

    [SerializeField] private TextMeshProUGUI Player1ScoreText;
    [SerializeField] private TextMeshProUGUI Player2ScoreText;
    [SerializeField] private TextMeshProUGUI TimeText;

    [Header("Match Data")]
    [SerializeField] private int Player1Score;
    [SerializeField] private int Player2Score;
    [SerializeField] private int TargetScore;
    [SerializeField] private float RemainingTime;
    [SerializeField] private EnemyDifficulty SelectedEnemyDifficulty;

    [Header("Match State")]
    [SerializeField] private bool IsMatchFinished;
    [SerializeField] private float ElapsedMatchTime;
    public static Action OnMatchEnded;

    [Header("Other Settings")]
    [SerializeField] private float MatchEndDelay = 1f;

    private void OnEnable()
    {
        GoalZone.OnGoalScored += HandleGoalScored;
    }

    private void OnDisable()
    {
        GoalZone.OnGoalScored -= HandleGoalScored;
    }

    private void Awake()
    {
        SelectedMatchSettings = FindObjectOfType<SelectedMatchSettings>();

        PlayerRacket2 = GameObject.Find("Player Racket 2");
        EnemyRacket = GameObject.Find("Enemy Racket");

        MatchResultPanel = GameObject.Find("Match Result Panel");

        Player1ResultText = GameObject.Find("Player 1 Result Text").GetComponent<TextMeshProUGUI>();
        Player2ResultText = GameObject.Find("Player 2 Result Text").GetComponent<TextMeshProUGUI>();
        MatchDurationText = GameObject.Find("Match Duration Text").GetComponent<TextMeshProUGUI>();
        WinnerText = GameObject.Find("Winner Text").GetComponent<TextMeshProUGUI>();

        Player1ScoreText = GameObject.Find("Player 1 Score Text").GetComponent<TextMeshProUGUI>();
        Player2ScoreText = GameObject.Find("Player 2 Score Text").GetComponent<TextMeshProUGUI>();
        TimeText = GameObject.Find("Time Text").GetComponent<TextMeshProUGUI>();

        MatchResultPanel.SetActive(false);
        PlayerRacket2.SetActive(false);
        EnemyRacket.SetActive(false);
    }

    private void Start()
    {
        ApplyMatchSettings();
        InitializeMatchData();
        UpdateScoreTexts();
        UpdateTimeText();
        DebugMatchSettings();
    }

    private void Update()
    {
        if (IsMatchFinished) return;

        UpdateTimer();
    }

    private void HandleGoalScored(MatchSide ScoringSide)
    {
        if (IsMatchFinished) return;

        if (ScoringSide == MatchSide.Player1)
        {
            Player1Score++;
        }

        else if (ScoringSide == MatchSide.Player2)
        {
            Player2Score++;
        }

        UpdateScoreTexts();
        CheckMatchEndByScore();

        Debug.Log("Goal scored by: " + ScoringSide);
    }

    private void UpdateTimer()
    {
        ElapsedMatchTime += Time.deltaTime;

        if (RemainingTime == 0f && SelectedMatchSettings.MatchDurationSeconds == 0f)
        {
            UpdateTimeText();
            return;
        }

        RemainingTime -= Time.deltaTime;

        if (RemainingTime < 0f)
        {
            RemainingTime = 0f;
        }

        UpdateTimeText();
        CheckMatchEndByTime();
    }

    private void CheckMatchEndByScore()
    {
        if (TargetScore == 0) return;

        if (Player1Score >= TargetScore || Player2Score >= TargetScore)
        {
            EndMatch();
        }
    }

    private void CheckMatchEndByTime()
    {
        if (SelectedMatchSettings.MatchDurationSeconds == 0f) return;

        if (RemainingTime <= 0f)
        {
            EndMatch();
        }
    }

    private void EndMatch()
    {
        if (IsMatchFinished) return;

        IsMatchFinished = true;
        OnMatchEnded?.Invoke();

        StartCoroutine(ShowMatchResultPanelWithDelay());

        Debug.Log("Match ended.");
    }

    private void ApplyMatchSettings()
    {
        if (SelectedMatchSettings == null) return;

        if (SelectedMatchSettings.GameMode == GameMode.Singleplayer)
        {
            EnemyRacket.SetActive(true);
            PlayerRacket2.SetActive(false);
        }

        else if (SelectedMatchSettings.GameMode == GameMode.Multiplayer)
        {
            EnemyRacket.SetActive(false);
            PlayerRacket2.SetActive(true);
        }
    }

    private void InitializeMatchData()
    {
        if (SelectedMatchSettings == null) return;

        IsMatchFinished = false;
        ElapsedMatchTime = 0f;

        Player1Score = 0;
        Player2Score = 0;

        TargetScore = SelectedMatchSettings.TargetScore;
        RemainingTime = SelectedMatchSettings.MatchDurationSeconds;
        SelectedEnemyDifficulty = SelectedMatchSettings.EnemyDifficulty;
    }

    private void UpdateScoreTexts()
    {
        if (TargetScore == 0)
        {
            Player1ScoreText.text = "P1 SCORE: " + Player1Score + " / NO LIMIT";
            Player2ScoreText.text = "P2 SCORE: " + Player2Score + " / NO LIMIT";
        }

        else
        {
            Player1ScoreText.text = "P1 SCORE: " + Player1Score + " / " + TargetScore;
            Player2ScoreText.text = "P2 SCORE: " + Player2Score + " / " + TargetScore;
        }
    }

    private void UpdateTimeText()
    {
        if (SelectedMatchSettings.MatchDurationSeconds == 0f)
        {
            int TotalElapsedSeconds = Mathf.FloorToInt(ElapsedMatchTime);
            int ElapsedMinutes = TotalElapsedSeconds / 60;
            int ElapsedSeconds = TotalElapsedSeconds % 60;

            TimeText.text = "Time: " + ElapsedMinutes.ToString("00") + ":" + ElapsedSeconds.ToString("00");
            return;
        }

        int TotalSeconds = Mathf.CeilToInt(RemainingTime);
        int Minutes = TotalSeconds / 60;
        int Seconds = TotalSeconds % 60;

        TimeText.text = "Time: " + Minutes.ToString("00") + ":" + Seconds.ToString("00");
    }

    private void DebugMatchSettings()
    {
        if (SelectedMatchSettings == null) return;

        Debug.Log("Game Mode: " + SelectedMatchSettings.GameMode);
        Debug.Log("Game Type: " + SelectedMatchSettings.GameType);
        Debug.Log("Enemy Difficulty: " + GetFormattedEnemyDifficultyText());
        Debug.Log("Target Score: " + SelectedMatchSettings.TargetScore);
        Debug.Log("Match Duration Seconds: " + SelectedMatchSettings.MatchDurationSeconds);
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

    private IEnumerator ShowMatchResultPanelWithDelay()
    {
        yield return new WaitForSeconds(MatchEndDelay);

        UpdateMatchResultTexts();

        MatchResultPanel.SetActive(true);
    }

    private void UpdateMatchResultTexts()
    {
        Player1ResultText.text = "Player 1 Score: " + Player1Score;
        Player2ResultText.text = "Player 2 Score: " + Player2Score;
        MatchDurationText.text = "Match Duration: " + GetFormattedElapsedMatchTime();
        WinnerText.text = "WINNER: " + GetWinnerText();
    }

    private string GetFormattedElapsedMatchTime()
    {
        int TotalSeconds = Mathf.FloorToInt(ElapsedMatchTime);
        int Minutes = TotalSeconds / 60;
        int Seconds = TotalSeconds % 60;

        return Minutes.ToString("00") + ":" + Seconds.ToString("00");
    }

    private string GetWinnerText()
    {
        if (Player1Score > Player2Score)
        {
            return "PLAYER 1";
        }

        if (Player2Score > Player1Score)
        {
            return "PLAYER 2";
        }

        return "DRAW";
    }

    public void OnMatchResultMainMenuButtonPressed()
    {
        GameManager.Instance.HandleMainMenuButtonPressed();
    }

    public void OnMatchResultRestartButtonPressed()
    {
        GameManager.Instance.HandleRestartButtonPressed();
    }
}