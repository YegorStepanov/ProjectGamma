using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mirror;
using UnityEngine;

public sealed class RoomManager : NetworkBehaviour
{
    [SerializeField] public GameFactory _gameFactory;

    private FreeStartPositions _freeStartPositions;
    private int _botCounts;

    public RoomPlayers RoomPlayers { get; private set; }

    public void InitRoom(List<Transform> startPositions, PlayerSpawnMethod playerSpawnMethod)
    {
        _freeStartPositions = new FreeStartPositions(startPositions, playerSpawnMethod);
        RoomPlayers = new RoomPlayers(startPositions.Count);
        _botCounts = 0;
    }

    #region TODO
    //private IEnumerator Start()
    //{
    //yield return new WaitForSeconds(10f);
    //RestartGame();
    //}
    #endregion

    public void RestartGame()
    {
        _freeStartPositions.Reset();
        PreparePlayersForGame();
    }

    public void PreparePlayersForGame()
    {
        RoomPlayers.PreparePlayersForGame(_freeStartPositions);
    }

    public void PreparePlayerForGame(Player player)
    {
        RoomPlayers.PreparePlayerForGame(player, _freeStartPositions);
    }

    public GameObject CreatePlayer(NetworkConnectionToClient conn, Player playerPrefab)
    {
        return CreatePlayer(playerPrefab).gameObject;

        Player player = CreatePlayer(playerPrefab);

        GameObject go = player.gameObject;
        NetworkServer.Spawn(go, conn);
        // RpcInitializePlayer(player);

        PreparePlayerForGame(player);

        return go;
    }

    #region TODO
    public void CreateBot(Player botPrefab)
    {
        return; //todo
        Player player = CreatePlayer(botPrefab);
        var botName = $"Bot{_botCounts}";
        player.Data.Name = botName;
        player.name = botName;
        GameObject go = player.gameObject;

        player.GetComponent<NetworkTransform>().clientAuthority = false;

        NetworkServer.Spawn(go);
        // RpcInitializePlayer(player);

        PreparePlayerForGame(player);

        StartCoroutine(Move(player));
    }

    private static IEnumerator Move(Player player)
    {
        yield return new WaitForSecondsRealtime(3f);

        for (int i = 0; i < 1000; i++)
        {
            player.Move(player.transform.forward * 0.1f);
            yield return new WaitForEndOfFrame();
        }
    }
    #endregion

    public void RemovePlayer(Player player)
    {
        RoomPlayers.RemovePlayer(player);
    }

    private Player CreatePlayer(Player playerPrefab)
    {
        Player player = _gameFactory.CreatePlayer(playerPrefab);
        RoomPlayers.AddPlayer(player);
        return player;
    }

    [TargetRpc, UsedImplicitly]
    public void TargetConstructPlayer(NetworkConnection conn, GameObject gamePlayer, PlayerSettings settings)
    {
        Player player = gamePlayer.GetComponent<Player>().NotNull();

        CameraController cameraController = _gameFactory.CreateCamera(player.CameraFocusPoint);
        var inputManager = new PointRelativeInputManager(cameraController.transform);

        player.Construct(settings, inputManager);

        Debug.Log($"Target settings {player.Settings != null}");
        ServerSettings(player);
    }

    [Command(requiresAuthority = false)]
    private void ServerSettings(Player player)
    {
        Debug.Log($"Server Target settings {player.Settings != null}");
    }
}