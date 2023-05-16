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

        private Player PlayerPrefab => playerPrefab.GetComponent<Player>();

                return NetworkClient.connection.identity.isServerOnly;
            }
        }

        public override void Start()
        {
            base.Start();
            _playerPrefab = playerPrefab.GetComponent<Player>().NotNull();
            _isServerOnly = NetworkClient.connection.identity.isServerOnly;
            _ = IsServerOnly;
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
            if (IsServerOnly)
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