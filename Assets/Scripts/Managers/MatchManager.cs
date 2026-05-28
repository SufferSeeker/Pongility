using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class MatchManager : MonoBehaviour
{
    #region Variables
    [Header("Core References")]
    [SerializeField] private SelectedMatchSettings SelectedMatchSettings;
    [SerializeField] private BallController BallController;

    [Header("Racket References")]
    [SerializeField] private GameObject PlayerRacket1;
    [SerializeField] private GameObject PlayerRacket2;

    [SerializeField] private PlayerRacketController Player1ManualController;
    [SerializeField] private PlayerRacketController Player2ManualController;
    [SerializeField] private EnemyRacketController Player2AIController;

    [Header("Health References")]
    [SerializeField] private DamageableTarget Player1DamageableTarget;
    [SerializeField] private DamageableTarget Player2DamageableTarget;

    [Header("Ability References")]
    [SerializeField] private PlayerAbilityInventory Player1AbilityInventory;
    [SerializeField] private PlayerAbilityInventory Player2AbilityInventory;

    [Header("Result UI")]
    [SerializeField] private GameObject MatchResultPanel;

    [SerializeField] private TextMeshProUGUI Player1ResultText;
    [SerializeField] private TextMeshProUGUI Player2ResultText;
    [SerializeField] private TextMeshProUGUI MatchDurationText;
    [SerializeField] private TextMeshProUGUI WinnerText;

    [Header("Match UI")]
    [SerializeField] private TextMeshProUGUI Player1ScoreText;
    [SerializeField] private TextMeshProUGUI Player2ScoreText;
    [SerializeField] private TextMeshProUGUI TimeText;

    [Header("Match Data")]
    [SerializeField] private int Player1Score;
    [SerializeField] private int Player2Score;
    [SerializeField] private int TargetScore;
    [SerializeField] private float RemainingTime;
    [SerializeField] private Difficulty SelectedEnemyDifficulty;

    [Header("Match State")]
    [SerializeField] private bool IsMatchFinished;
    [SerializeField] private bool IsRoundResetting;
    [SerializeField] private float ElapsedMatchTime;

    [Header("Events")]
    public static Action OnMatchEnded;
    public static Action OnRoundEndFreezeStarted;
    public static Action OnRoundCleanupRequested;
    public static Action OnRoundGameplayStarted;

    [Header("Other Settings")]
    [SerializeField] private float MatchEndDelay = 1f;
    [SerializeField] private float RoundResetDelay = 1f;
    #endregion

    #region Unity Methods
    private void Awake()
    {

        FindCoreReferences();
        FindRacketReferences();
        FindHealthReferences();
        FindAbilityInventories();
        FindUIReferences();

        MatchResultPanel.SetActive(false);
        PlayerRacket2.SetActive(true);
    }

    private void OnEnable()
    {

        GoalZone.OnGoalScored += HandleGoalScored;
        DamageableTarget.OnTargetDied += HandleTargetDied;
    }

    private void Start()
    {
        ApplyMatchSettings();
        InitializeMatchData();

        UpdateScoreTexts();
        UpdateTimeText();

        DebugMatchSettings();

        StartCoroutine(InitialRoundStartRoutine());
    }

    private void Update()
    {
        if (IsMatchFinished == true) return;
        if (IsRoundResetting == true) return;

        UpdateTimer();
    }

    private void OnDisable()
    {
        GoalZone.OnGoalScored -= HandleGoalScored;
        DamageableTarget.OnTargetDied -= HandleTargetDied;
    }
    #endregion

    #region Event Handlers
    private void HandleGoalScored(MatchSide ScoringSide)
    {
        if (IsMatchFinished == true) return;
        if (IsRoundResetting == true) return;

        AddScore(ScoringSide);

        UpdateScoreTexts();
        CheckMatchEndByScore();

        Debug.Log("Goal scored by: " + ScoringSide);

        if (IsMatchFinished == true) return;

        StartCoroutine(RoundResetRoutine(false));
    }

    private void HandleTargetDied(MatchSide DeadSide, MatchSide KillerSide)
    {
        if (IsMatchFinished == true) return;
        if (IsRoundResetting == true) return;

        StartCoroutine(HandleTargetDiedRoutine(DeadSide, KillerSide));
    }
    #endregion

    #region Round Flow
    private IEnumerator InitialRoundStartRoutine()
    {
        IsRoundResetting = true;

        SetRacketsCanMove(false);
        SetAbilityControlsEnabled(false);

        BallController.ResetBallForRound();

        yield return new WaitForSeconds(RoundResetDelay);

        SetRacketsCanMove(true);
        SetAbilityControlsEnabled(true);

        BallController.LaunchBall();

        OnRoundGameplayStarted?.Invoke();

        IsRoundResetting = false;
    }

    private IEnumerator HandleTargetDiedRoutine(MatchSide DeadSide, MatchSide KillerSide)
    {
        AddScore(KillerSide);

        UpdateScoreTexts();
        CheckMatchEndByScore();

        Debug.Log(DeadSide + " died. Score rewarded to: " + KillerSide);

        if (IsMatchFinished == true) yield break;

        yield return RoundResetRoutine(true);
    }

    private IEnumerator RoundResetRoutine(bool ShouldRestoreHealth)
    {
        IsRoundResetting = true;

        SetRacketsCanMove(false);
        SetAbilityControlsEnabled(false);

        BallController.StopBallForRound();
        OnRoundEndFreezeStarted?.Invoke();

        yield return new WaitForSeconds(RoundResetDelay);

        OnRoundCleanupRequested?.Invoke();

        ResetRacketPositions();
        BallController.ResetBallForRound();

        if (ShouldRestoreHealth == true)
        {
            RestoreAllHealth();
        }

        yield return new WaitForSeconds(RoundResetDelay);

        SetRacketsCanMove(true);
        SetAbilityControlsEnabled(true);

        BallController.LaunchBall();

        OnRoundGameplayStarted?.Invoke();

        IsRoundResetting = false;
    }

    private void ResetRacketPositions()
    {
        PlayerRacket1.transform.position = new Vector3(0f, -4f, 0f);
        PlayerRacket2.transform.position = new Vector3(0f, 4f, 0f);
    }

    private void RestoreAllHealth()
    {
        Player1DamageableTarget.RestoreFullHealth();
        Player2DamageableTarget.RestoreFullHealth();
    }

    private void SetRacketsCanMove(bool CanMove)
    {
        Player1ManualController.SetCanMove(CanMove);
        Player2ManualController.SetCanMove(CanMove);
        Player2AIController.SetCanFollow(CanMove);
    }

    private void SetAbilityControlsEnabled(bool CanUseAbilities)
    {
        Player1AbilityInventory.SetCanUseAbilities(CanUseAbilities);
        Player2AbilityInventory.SetCanUseAbilities(CanUseAbilities);
    }
    #endregion

    #region Match Setup
    private void ApplyMatchSettings()
    {
        if (SelectedMatchSettings == null) return;

        PlayerRacket2.SetActive(true);

        if (SelectedMatchSettings.GameMode == GameMode.Singleplayer)
        {
            Player2ManualController.enabled = false;
            Player2AIController.enabled = true;
        }

        else if (SelectedMatchSettings.GameMode == GameMode.Multiplayer)
        {
            Player2ManualController.enabled = true;
            Player2AIController.enabled = false;
        }
    }

    private void InitializeMatchData()
    {
        if (SelectedMatchSettings == null) return;

        IsMatchFinished = false;
        IsRoundResetting = false;
        ElapsedMatchTime = 0f;

        Player1Score = 0;
        Player2Score = 0;

        TargetScore = SelectedMatchSettings.TargetScore;
        RemainingTime = SelectedMatchSettings.MatchDurationSeconds;
        SelectedEnemyDifficulty = SelectedMatchSettings.Difficulty;
    }
    #endregion

    #region Score And Timer
    private void AddScore(MatchSide ScoringSide)
    {
        if (ScoringSide == MatchSide.Player1)
        {
            Player1Score++;
        }

        else if (ScoringSide == MatchSide.Player2)
        {
            Player2Score++;
        }
    }

    private void UpdateTimer()
    {
        ElapsedMatchTime += Time.deltaTime;

        if (SelectedMatchSettings.MatchDurationSeconds == 0f)
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
        if (IsMatchFinished == true) return;

        IsMatchFinished = true;
        OnMatchEnded?.Invoke();

        StartCoroutine(ShowMatchResultPanelWithDelay());

        Debug.Log("Match ended.");
    }
    #endregion

    #region UI Updates
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
    #endregion

    #region Button Methods
    public void OnMatchResultMainMenuButtonPressed()
    {
        GameManager.Instance.HandleMainMenuButtonPressed();
    }

    public void OnMatchResultRestartButtonPressed()
    {
        GameManager.Instance.HandleRestartButtonPressed();
    }
    #endregion

    #region Debug Methods
    public void DebugClearAbilityObjects()
    {
        OnRoundCleanupRequested?.Invoke();

        Debug.Log("Debug requested ability object cleanup.");
    }
    #endregion

    #region Reference Setup
    private void FindCoreReferences()
    {
        SelectedMatchSettings = FindObjectOfType<SelectedMatchSettings>();
        BallController = GameObject.Find("Ball").GetComponent<BallController>();
    }

    private void FindRacketReferences()
    {
        PlayerRacket1 = GameObject.Find("Player Racket 1");
        PlayerRacket2 = GameObject.Find("Player Racket 2");

        Player1ManualController = PlayerRacket1.GetComponent<PlayerRacketController>();
        Player2ManualController = PlayerRacket2.GetComponent<PlayerRacketController>();
        Player2AIController = PlayerRacket2.GetComponent<EnemyRacketController>();
    }

    private void FindHealthReferences()
    {
        Player1DamageableTarget = PlayerRacket1.GetComponent<DamageableTarget>();
        Player2DamageableTarget = PlayerRacket2.GetComponent<DamageableTarget>();
    }

    private void FindAbilityInventories()
    {
        PlayerAbilityInventory[] Inventories = FindObjectsOfType<PlayerAbilityInventory>();

        for (int i = 0; i < Inventories.Length; i++)
        {
            if (Inventories[i].GetPlayerSide() == MatchSide.Player1)
            {
                Player1AbilityInventory = Inventories[i];
            }

            else if (Inventories[i].GetPlayerSide() == MatchSide.Player2)
            {
                Player2AbilityInventory = Inventories[i];
            }
        }
    }

    private void FindUIReferences()
    {
        MatchResultPanel = GameObject.Find("Match Result Panel");

        Player1ResultText = GameObject.Find("Player 1 Result Text").GetComponent<TextMeshProUGUI>();
        Player2ResultText = GameObject.Find("Player 2 Result Text").GetComponent<TextMeshProUGUI>();
        MatchDurationText = GameObject.Find("Match Duration Text").GetComponent<TextMeshProUGUI>();
        WinnerText = GameObject.Find("Winner Text").GetComponent<TextMeshProUGUI>();

        Player1ScoreText = GameObject.Find("Player 1 Score Text").GetComponent<TextMeshProUGUI>();
        Player2ScoreText = GameObject.Find("Player 2 Score Text").GetComponent<TextMeshProUGUI>();
        TimeText = GameObject.Find("Time Text").GetComponent<TextMeshProUGUI>();
    }
    #endregion

    #region Helper Methods
    private void DebugMatchSettings()
    {
        if (SelectedMatchSettings == null) return;

        Debug.Log("Game Mode: " + SelectedMatchSettings.GameMode);
        Debug.Log("Game Type: " + SelectedMatchSettings.GameType);
        Debug.Log("Enemy Difficulty: " + GetFormattedEnemyDifficultyText());
        Debug.Log("Target Score: " + SelectedMatchSettings.TargetScore);
        Debug.Log("Match Duration Seconds: " + SelectedMatchSettings.MatchDurationSeconds);
        Debug.Log("Ball Speed Mode: " + SelectedMatchSettings.BallSpeedMode);
    }

    private string GetFormattedEnemyDifficultyText()
    {
        switch (SelectedEnemyDifficulty)
        {
            case Difficulty.Easy:
                return "Easy";

            case Difficulty.Normal:
                return "Normal";

            case Difficulty.Hard:
                return "Hard";

            case Difficulty.Insane:
                return "Insane";
        }

        return "Unknown";
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
    #endregion
}