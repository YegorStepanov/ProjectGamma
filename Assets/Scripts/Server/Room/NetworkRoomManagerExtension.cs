using Mirror;
using UnityEngine;

public sealed class NetworkRoomManagerExtension : NetworkRoomManager
{
    [Header("Dependencies")]
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private GUIManager _guiManager;

    private Player PlayerPrefab => playerPrefab.GetComponent<Player>();

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
        {
            _roomManager.InitRoom(startPositions, playerSpawnMethod);
            _guiManager.RpcShowInGamePlayersScore();

            _roomManager.CreateBot(PlayerPrefab);
        }
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        GameObject player = _roomManager.CreatePlayer(conn, PlayerPrefab);
        return player;
    }
    
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        _roomManager._gameFactory.RpcInitializePlayer(gamePlayer.GetComponent<Player>().NotNull());
        _roomManager._gameFactory.InitializePlayer(gamePlayer.GetComponent<Player>().NotNull());
        _roomManager.PreparePlayerForGame(gamePlayer.GetComponent<Player>().NotNull());

        int index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        IPlayer player = gamePlayer.GetComponent<IPlayer>().NotNull();
        player.Data.Name = $"Player {index + 1}";
        return true;
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Disconnecting");
        foreach (NetworkIdentity id in conn.clientOwnedObjects)
        {
            if (id.TryGetComponent(out Player player))
            {
                _roomManager.RemovePlayer(player);
            }
        }
        // NetworkServer.DestroyPlayerForConnection(conn);
        Debug.Log("Disconnect");
        
        base.OnServerDisconnect(conn);
        Debug.Log("Disconnected");
    }
    
    public override void OnRoomServerPlayersReady()
    {
#if UNITY_SERVER
        ServerChangeScene(GameplayScene);
#else
        _guiManager.ShowStartGameButton(onClick: () =>
        {
            _guiManager.HideStartGameButton();
            ServerChangeScene(GameplayScene);
        });
#endif
    }

    public override void OnRoomServerPlayersNotReady()
    {
        _guiManager.HideStartGameButton();
    }
}