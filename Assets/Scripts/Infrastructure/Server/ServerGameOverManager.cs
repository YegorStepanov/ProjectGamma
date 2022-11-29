using System.Collections;
using Mirror;
using Room;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerGameOverManager : NetworkBehaviour
    {
        [SerializeField] private ServerRoomManager _serverRoomManager;
        [SerializeField] private GUIManager _guiManager;
        [SerializeField] private RoomSettings _roomSettings;

        private bool _isOnGameOver;

        public void EndGame(Player winner)
        {
            if (_isOnGameOver) return;

            StartCoroutine(GameOverRoutine(winner));
        }

        private IEnumerator GameOverRoutine(Player winner)
        {
            _isOnGameOver = true;

            _serverRoomManager.RoomPlayers.DisableMovingForAll();
            _guiManager.RpcShowGameOverPanel(winner.Data.Name, _roomSettings.RestartTimeSeconds);

            yield return new WaitForSecondsRealtime(_roomSettings.RestartTimeSeconds);

            _guiManager.RpcHideGameOverPanel();
            _serverRoomManager.RestartGame();

            _isOnGameOver = false;
        }
    }
}