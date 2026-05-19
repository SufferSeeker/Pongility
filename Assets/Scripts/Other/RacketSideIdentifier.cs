using UnityEngine;

public class RacketSideIdentifier : MonoBehaviour
{
    #region Variables
    [Header("Racket Settings")]
    [SerializeField] private MatchSide RacketSide;
    #endregion

    #region Getters
    public MatchSide GetRacketSide()
    {
        return RacketSide;
    }
    #endregion
}