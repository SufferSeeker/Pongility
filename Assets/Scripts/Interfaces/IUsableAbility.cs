using UnityEngine;

public interface IUsableAbility
{
    void Initialize(Vector2 Direction, MatchSide OwnerSide, Transform CastParent);
}