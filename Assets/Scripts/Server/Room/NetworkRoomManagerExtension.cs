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

    public override void OnRoomClientAddPlayerFailed()
    {
        base.OnRoomClientAddPlayerFailed();
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        GameObject player = _roomManager.CreatePlayer(conn, PlayerPrefab);
        return player;
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        Player playerComp = gamePlayer.GetComponent<Player>().NotNull();
        // _roomManager._gameFactory.TargetInitializePlayer(conn, gamePlayer); //mb pass gameObject?
        _roomManager.PreparePlayerForGame(playerComp);

        int index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        IPlayer player = gamePlayer.GetComponent<IPlayer>().NotNull();
        player.Data.Name = $"Player {index + 1}";

        NetworkServer.ReplacePlayerForConnection(conn, gamePlayer, true);

        var settings = _roomManager._gameFactory.Settings;
        _roomManager.TargetConstructPlayer(conn, gamePlayer, settings);
        return false; //did ReplacePlayerForConnection manually
    }

    public override void OnServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Disconnecting1");
        foreach (NetworkIdentity id in conn.clientOwnedObjects)
        {
            if (id.TryGetComponent(out Player player))
            {
                _roomManager.RemovePlayer(player);
            }
        }

        Debug.Log("Disconnecting2");
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