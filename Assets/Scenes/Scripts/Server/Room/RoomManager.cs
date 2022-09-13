using Mirror;
using UnityEngine;

public sealed class RoomManager : NetworkRoomManager
{
    [Header("Dependencies")]
    [SerializeField] private ServerRoomManager _serverRoomManager;
    [SerializeField] private GUIManager _guiManager;

    public override void OnRoomServerSceneChanged(string sceneName)
    {
        if (sceneName == GameplayScene)
        {
            _serverRoomManager.InitRoom(startPositions, playerSpawnMethod);
            _guiManager.RpcShowInGamePlayersScore();

            _serverRoomManager.CreateBot(playerPrefab.GetComponent<Player>());
        }
    }

    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        GameObject player = _serverRoomManager.CreatePlayer(conn, playerPrefab.GetComponent<Player>());
        return player;
    }

    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        int index = roomPlayer.GetComponent<NetworkRoomPlayer>().index;
        string playerName = $"Player{index}";

        gamePlayer.GetComponent<Player>().name = playerName + "228";
        gamePlayer.GetComponent<Player>().Name = playerName + "322";
        return true;
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
