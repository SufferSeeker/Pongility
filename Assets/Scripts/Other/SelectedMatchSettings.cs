using UnityEngine;

public enum GameMode
{
    Singleplayer,
    Multiplayer
}

public enum GameType
{
    Pongility,
    Classic
}

public enum EnemyDifficulty
{
    Easy,
    Medium,
    Hard,
    VeryHard,
    Insane
}

public enum MatchSide
{
    None,
    Player1,
    Player2
}

public class SelectedMatchSettings : MonoBehaviour
{
    [Header("Game Settings")]
    public GameMode GameMode = GameMode.Singleplayer;
    public GameType GameType = GameType.Classic;
    public EnemyDifficulty EnemyDifficulty = EnemyDifficulty.Medium;
    
    [Header("Match Rules")]
    public int TargetScore = 10;
    public float MatchDurationSeconds = 300f;
}