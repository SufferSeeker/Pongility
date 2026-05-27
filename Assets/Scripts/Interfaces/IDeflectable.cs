public interface IDeflectable
{
    MatchSide GetOwnerSide();

    void Deflect(MatchSide NewOwnerSide);
}