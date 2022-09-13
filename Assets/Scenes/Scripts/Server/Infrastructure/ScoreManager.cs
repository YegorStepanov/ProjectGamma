public sealed class ScoreManager : Mirror.NetworkBehaviour
{
    public bool IsPlayerWon(IPlayer player) =>
        player.Score >= 1;

    public void IncreaseScore(IPlayer player) =>
        player.Score++;
}
