using UnityEngine;

public class RacketSideIdentifier : MonoBehaviour
{
    [SerializeField] private MatchSide RacketSide;
    
    public MatchSide GetRacketSide()
    {
        return RacketSide;
    }
}