using System;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public static Action<MatchSide> OnGoalScored;

    [Header("Goal Settings")]
    [SerializeField] private MatchSide GoalOwner;
    
    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Ball") == false)
        {
            return;
        }

        if (GoalOwner == MatchSide.Player1)
        {
            OnGoalScored?.Invoke(MatchSide.Player2);
        }

        else if (GoalOwner == MatchSide.Player2)
        {
            OnGoalScored?.Invoke(MatchSide.Player1);
        }
    }
}