using System.Collections;
using JetBrains.Annotations;
using Mirror;
using Room;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerGameOverManager : NetworkBehaviour
    {
        [SerializeField] private ServerRoomManager _serverRoomManager;
        [SerializeField] private ServerGUIManager _serverGUIManager;
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

            SetStateToAllPlayers(PlayerState.None);

            _serverGUIManager.RpcShowGameOverPanel(winner.Data.Name, _roomSettings.RestartTimeSeconds);

            yield return new WaitForSecondsRealtime(_roomSettings.RestartTimeSeconds);

            _serverGUIManager.RpcHideGameOverPanel();
            _serverRoomManager.RestartGame();

            _isOnGameOver = false;
        }

        private void SetStateToAllPlayers(PlayerState state)
        {
            foreach (Player player in _serverRoomManager.RoomPlayers.Players)
            {
                player.StateMachine.SetState(state);
                TargetSetState(player.connectionToClient, player, state);
            }
        }

        [TargetRpc]
        private void TargetSetState([UsedImplicitly] NetworkConnectionToClient target, Player player, PlayerState state)
        {
            player.StateMachine.SetState(state);
        }
    }
}