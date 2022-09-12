using Mirror;
using UnityEngine;

public sealed class RoomManager : NetworkRoomManager
{
    [Header("Dependencies")]
    [SerializeField] private ServerRoomManager _server;
    [SerializeField] private GUIManager _guiManager;

    // This is called on the server when a networked scene finishes loading.
    public override void OnRoomServerSceneChanged(string sceneName)
    {
        _server.InitRoom(startPositions, playerSpawnMethod);
        _guiManager.ShowInGamePlayersScore();
        Debug.Log("OnSceneChanged");
    }

    // Called just after GamePlayer object is instantiated and just before it replaces RoomPlayer object.
    // This is the ideal point to pass any data like player name, credentials, tokens, colors, etc.
    // into the GamePlayer object as it is about to enter the Online scene.
    public override bool OnRoomServerSceneLoadedForPlayer(NetworkConnectionToClient conn, GameObject roomPlayer, GameObject gamePlayer)
    {
        // _server.PreparePlayer(roomPlayer, gamePlayer);
        
        // var player = gamePlayer.GetComponent<Player>();
        // player.Construct(_playerData, _inputManager, CreateCamera, _heroCollisionManager.HandleColliderHit);
        // player.SetState(PlayerState.Walk);

        Debug.Log("OnSceneLoadedForPlayer");
        return true;
    }
    
    
    public override GameObject OnRoomServerCreateGamePlayer(NetworkConnectionToClient conn, GameObject roomPlayer)
    {
        //_server.CreateTestPlayer(playerPrefab.GetComponent<Player>());

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
            _guiManager.HideRoomGUI();

            base.OnRoomServerPlayersReady();
        });
#endif
    }

    public override void OnRoomServerPlayersNotReady()
    {
        _guiManager.HideStartGameButton();
    }
}