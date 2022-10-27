using InputManagers;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

namespace Infrastructure
{
    public sealed class ClientRoomManager : NetworkBehaviour
    {
        [SerializeField] public GameFactory _gameFactory;

        public readonly SyncList<PlayerScoreData> PlayerDatas = new SyncList<PlayerScoreData>();

        [Server]
        public void AddPlayer(IPlayer player)
        {
            PlayerDatas.Add(player.Data.ScoreData);
        }

        [Server]
        public void RemovePlayer(IPlayer player)
        {
            PlayerDatas.Remove(player.Data.ScoreData);
        }

        [TargetRpc]
        public void TargetConstructPlayer([UsedImplicitly] NetworkConnection conn, GameObject gamePlayer, PlayerSettings settings)
        {
            IPlayer player = gamePlayer.GetComponent<IPlayer>().NotNull();

            CameraController cameraController = _gameFactory.CreateCamera(player.CameraFocusPoint);
            var inputManager = new TransformRelativeInputManager(cameraController.transform);
            player.Construct(settings, inputManager);

            //todo: the state should be synced
            if (isLocalPlayer)
                player.StateMachine.SetState(PlayerState.Walk);
        }
    }
}