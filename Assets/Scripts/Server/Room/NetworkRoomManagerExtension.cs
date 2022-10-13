using Mirror;
using UnityEngine;

public sealed class NetworkRoomManagerExtension : NetworkRoomManager
{
    [Header("Dependencies")]
    [SerializeField] private ServerRoomManager _serverRoomManager;
    [SerializeField] private ClientRoomManager _clientRoomManager;
    [SerializeField] private GUIManager _guiManager;

    private Player PlayerPrefab => playerPrefab.GetComponent<Player>();

    // on the client _serverRoomManager is null and vice versa
    private bool IsServerOnly => _serverRoomManager == null
        ? _clientRoomManager.isServerOnly
        : _serverRoomManager.isServerOnly; //or isServer?

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
        {
            _serverRoomManager.InitRoom(startPositions, playerSpawnMethod);
            _guiManager.RpcShowInGamePlayersScore();
            // button: create bot
            // _roomManager.CreateBot(PlayerPrefab);
        }
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        return _serverRoomManager.CreatePlayer(PlayerPrefab).gameObject;
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
            _guiManager.ShowStartGameButton(onClick: () =>
            {
                _guiManager.HideStartGameButton();
                ServerChangeScene(GameplayScene);
            });
        }
    }

    public override void OnRoomServerPlayersNotReady()
    {
        _guiManager.HideStartGameButton();
    }
}