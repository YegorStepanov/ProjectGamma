using System.Collections;
using Mirror;
using UnityEngine;

public sealed class GameOverManager : NetworkBehaviour
{
    [SerializeField] private ServerRoomManager _serverRoomManager;
    [SerializeField] private GUIManager _guiManager;

    private IEnumerator _gameOverRoutine;

    public void EndGame(IPlayer winner)
    {
        if (_gameOverRoutine != null) return;

        _gameOverRoutine = GameOverRoutine(winner);
        StartCoroutine(_gameOverRoutine);
    }

    private IEnumerator GameOverRoutine(IPlayer winner)
    {
        Debug.Log($"GG name {winner}");
        _guiManager.RpcShowGameOverPanel(winner.Name, 5f);
        _serverRoomManager.Players.SetStatesToNone();

        yield return new WaitForSecondsRealtime(5f);
        _guiManager.RpcHideGameOverPanel();

        Debug.Log("Restarting...");
        _serverRoomManager.RestartGame();

        _serverRoomManager.Players.SetStatesToWalk();
        _gameOverRoutine = null;
    }
}
