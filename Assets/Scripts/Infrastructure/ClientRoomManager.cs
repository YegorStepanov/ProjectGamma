using InputManagers;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

namespace Infrastructure
{
    public sealed class ClientRoomManager : NetworkBehaviour
    {
        [SerializeField] public GameFactory _gameFactory;

        public readonly SyncList<PlayerData> PlayerDatas = new SyncList<PlayerData>();

        [Server]
        public void AddPlayer(Player player)
        {
            PlayerDatas.Add(player.Data);
        }

        [Server]
        public void RemovePlayer(Player player)
        {
            PlayerDatas.Remove(player.Data);
        }

        [TargetRpc]
        public void TargetConstructPlayer([UsedImplicitly] NetworkConnection conn, GameObject gamePlayer, PlayerSettings settings)
        {
            Player player = gamePlayer.GetComponent<Player>().NotNull();

            CameraController cameraController = _gameFactory.CreateCamera(player.CameraFocusPoint);
            var inputManager = new TransformRelativeInputManager(cameraController.transform);
            player.Construct(settings, inputManager);
        }
    }
}