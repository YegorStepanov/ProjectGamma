using Mirror;
using UnityEngine;

public sealed class NetworkRoomManagerExtension : NetworkRoomManager
{
    [Header("Dependencies")]
    [SerializeField] private RoomManager _roomManager;
    [SerializeField] private GUIManager _guiManager;

    private Player _playerPrefab;

    public override void Awake()
    {
        _playerPrefab = playerPrefab.GetComponent<Player>();
    }

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
        {
            _roomManager.InitRoom(startPositions, playerSpawnMethod);
            _guiManager.RpcShowInGamePlayersScore();

            _roomManager.CreateBot(_playerPrefab);
            Debug.Log("ServerSceneChanged");
        }
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        GameObject player = _roomManager.CreatePlayer(conn, _playerPrefab);
        return player;
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        int index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        IPlayer player = gamePlayer.GetComponent<IPlayer>();
        player.Data.Name = $"Player{index}";
        return true;
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
