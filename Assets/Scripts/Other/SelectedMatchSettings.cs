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

public enum Difficulty
{
    Easy,
    Normal,
    Hard,
    Insane
}

public enum BallSpeedMode
{
    Fixed,
    Dynamic
}

public enum MatchSide
{
    None,
    Player1,
    Player2
}

public class SelectedMatchSettings : MonoBehaviour
{

    #region Variables
    [Header("Game Settings")]
    public GameMode GameMode = GameMode.Singleplayer;
    public GameType GameType = GameType.Classic;
    public Difficulty Difficulty = Difficulty.Easy;
    public BallSpeedMode BallSpeedMode = BallSpeedMode.Fixed;


    [Header("Match Rules")]
    public int TargetScore = 10;
    public float MatchDurationSeconds = 300f;
    #endregion
}