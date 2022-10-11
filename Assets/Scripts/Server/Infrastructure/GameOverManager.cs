using System.Collections;
using Mirror;
using UnityEngine;

public sealed class GameOverManager : NetworkBehaviour
{
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private GUIManager _guiManager;
    [SerializeField] private RoomSettings _roomSettings;

    private bool _isOnGameOver;

    public void EndGame(IPlayer winner)
    {
        if (_isOnGameOver) return;

        StartCoroutine(GameOverRoutine(winner));
    }

    private IEnumerator GameOverRoutine(IPlayer winner)
    {
        _isOnGameOver = true;

        _roomManager.RoomPlayers.DisableMovingForAll();
        _guiManager.RpcShowGameOverPanel(winner.Data.Name, _roomSettings.RestartTimeSeconds);

        yield return new WaitForSecondsRealtime(_roomSettings.RestartTimeSeconds);

        _guiManager.RpcHideGameOverPanel();
        _roomManager.RestartGame();

        _isOnGameOver = false;
    }
}