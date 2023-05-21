using Infrastructure.Server;
using Mirror;
using UnityEngine;

namespace Infrastructure
{
    public sealed class NetworkRoomManagerExtension : NetworkRoomManager
    {
        [Header("Dependencies")]
        [SerializeField] private ServerRoomManager _serverRoomManager;
        [SerializeField] private ServerGUIManager _serverGUIManager;
        [SerializeField] private ServerParticleSystem _serverParticleSystem;


        private Player _playerPrefab;

        public override void Start()
        {
            base.Start();
            _playerPrefab = playerPrefab.GetComponent<Player>().NotNull();
        }

        public override void OnRoomServerSceneChanged(string sceneName)
        {
            if (sceneName == GameplayScene)
            {
                _serverRoomManager.InitRoom(startPositions, playerSpawnMethod);
                _serverGUIManager.RpcShowInGamePlayersScore();
                // button: create bot
                // _roomManager.CreateBot(PlayerPrefab);
            }
        }

        public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
        {
            return _serverRoomManager.CreatePlayer(_playerPrefab).gameObject;
        }

        public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
        {
            int playerIndex = roomPlayer.GetComponent<NetworkRoomPlayer>().index;

            _serverRoomManager.ReplaceAndConstructPlayer(conn, gamePlayer, playerIndex);
            // _serverParticleSystem.RpcPlaySpawnEffect(player.Position);

            return false; // ReplacePlayerForConnection already called
        }

        public override void OnServerDisconnect(NetworkConnectionToClient conn)
        {
            foreach (NetworkIdentity id in conn.clientOwnedObjects)
            {
                if (id.TryGetComponent(out Player player))
                {
                    _serverRoomManager.RemovePlayer(player);
                }
            }

            base.OnServerDisconnect(conn);
        }

        public override void OnRoomServerPlayersReady()
        {
            if (NetworkClient.connection.identity.isServerOnly)
            {
                ServerChangeScene(GameplayScene);
            }
            else
            {
                _serverGUIManager.ShowStartGameButton(onClick: () =>
                {
                    _serverGUIManager.HideStartGameButton();
                    ServerChangeScene(GameplayScene);
                });
            }
        }

        public override void OnRoomServerPlayersNotReady()
        {
            _serverGUIManager.HideStartGameButton();
        }
    }
}