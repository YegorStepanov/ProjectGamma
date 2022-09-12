public sealed class ScoreManager : Mirror.NetworkBehaviour
{
    public bool IsPlayerWon(IPlayer player) =>
        player.Score > 2;

    public void IncreaseScore(IPlayer player) =>
        player.Score++;
}
