using System;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    #region Events
    public static Action<MatchSide> OnGoalScored;
    #endregion

    #region Variables
    [Header("Goal Settings")]
    [SerializeField] private MatchSide ScoringSide;
    #endregion

    #region Unity Methods
    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Ball") == false) return;
        
        OnGoalScored?.Invoke(ScoringSide);
    }
    #endregion
}