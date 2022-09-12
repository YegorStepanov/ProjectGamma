using Mirror;
using UnityEngine;

public sealed class RoomManager : NetworkRoomManager
{
    [Header("Dependencies")]
    [SerializeField] private ServerRoomManager _server;
    [SerializeField] private GUIManager _guiManager;

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
        {
            _server.InitRoom(startPositions, playerSpawnMethod);
            _guiManager.RpcShowInGamePlayersScore();
        }
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        int index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        string playerName = $"Player{index}";

        var player = gamePlayer.GetComponent<Player>();
        _server.RpcInitializePlayer(player, playerName);

        Debug.Log("OnSceneLoadedForPlayer");
        return true;
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        Player player = _server.CreatePlayer(playerPrefab.GetComponent<Player>());
        GameObject go = player.gameObject;
        NetworkServer.Spawn(go, conn);
        return go;
    }

    public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        Debug.Log("Disconnect");
    }

    public override void OnRoomServerPlayersReady()
    {
#if UNITY_SERVER
        base.OnRoomServerPlayersReady();
#else
        _guiManager.ShowStartGameButton(onClick: () =>
        {
            _guiManager.HideStartGameButton();
            _guiManager.RpcHideRoomGUI();

            base.OnRoomServerPlayersReady();
        });
#endif
    }

    public override void OnRoomServerPlayersNotReady()
    {
        _guiManager.HideStartGameButton();
    }
}
