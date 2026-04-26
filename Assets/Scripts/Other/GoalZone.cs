using System;
using UnityEngine;

public class GoalZone : MonoBehaviour
{
    public static Action<MatchSide> OnGoalScored;

    [Header("Goal Settings")]
    [SerializeField] private MatchSide ScoringSide;

    private void OnTriggerEnter2D(Collider2D Collision)
    {
        if (Collision.CompareTag("Ball") == false)
        {
            return;
        }

        OnGoalScored?.Invoke(ScoringSide);
    }
}