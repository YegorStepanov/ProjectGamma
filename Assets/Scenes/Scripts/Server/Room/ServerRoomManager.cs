using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mirror;
using UnityEngine;

public sealed class ServerRoomManager : Mirror.NetworkBehaviour //rename?
{
    [SerializeField] private GameFactory _gameFactory;
    [SerializeField] private RoomManager _roomManager;

    // exclude the point after creation so that another player cannot be spawned on the same point
    private FreeStartPositions _freeStartPositions;
    private RoomPlayers _roomPlayers;
    private int _botCounts;

    public RoomPlayers RoomPlayers => _roomPlayers;

    [Server]
    public void InitRoom(List<Transform> startPositions, PlayerSpawnMethod playerSpawnMethod)
    {
        _freeStartPositions = new FreeStartPositions(startPositions, playerSpawnMethod);
        _roomPlayers = new RoomPlayers(startPositions.Count);
        _botCounts = 0;
    }

    [ServerCallback]
    private IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(10f);
        RestartGame();
    }

    [Server]
    public void RestartGame()
    {
        _roomPlayers.SetStatesToNone();

        _freeStartPositions.Reset();
        _roomPlayers.ResetScores();
        // _players.ResetPositions(_freeStartPositions);

        foreach (IPlayer player in _roomPlayers)
        {
            Vector3 position = _freeStartPositions.Pop();
            Vector3 lookRotation = GameFactory.LookToSceneCenter(-position);
            player.SetPosition(position);
            player.SetRotation(Quaternion.LookRotation(lookRotation));
        }

        _roomPlayers.SetStatesToWalk();
    }

    [Server]
    public GameObject CreatePlayer(NetworkConnectionToClient conn, Player playerPrefab)
    {
        Player player = CreatePlayer(playerPrefab);

        GameObject go = player.gameObject;
        NetworkServer.Spawn(go, conn);
        RpcInitializePlayer(player, $"P {_roomPlayers.Count()}");
        return go;
    }

    [Server]
    public void CreateBot(Player botPrefab)
    {
        Player player = CreatePlayer(botPrefab);
        GameObject go = player.gameObject;

        player.GetComponent<NetworkTransform>().clientAuthority = false;

        NetworkServer.Spawn(go);
        // InitializePlayer(player, $"Bot{_botCounts}");
        RpcInitializePlayer(player, $"Bot{_botCounts}");

        StartCoroutine(Move(player));
    }

    private static IEnumerator Move(Player player)
    {
        yield return new WaitForSecondsRealtime(3f);

        for (int i = 0; i < 5000; i++)
        {
            Vector3 position = player.GetComponent<Transform>().position;
            position.x += Time.deltaTime / 10f;
            player.SetPosition(position);

            yield return new WaitForEndOfFrame();
        }
    }

    [Server]
    private Player CreatePlayer(Player playerPrefab)
    {
        Vector3 position = _freeStartPositions.Pop();

        Player player = _gameFactory.CreatePlayer(playerPrefab, position);
        _roomPlayers.AddPlayer(player);
        return player;
    }

    [ClientRpc]
    public void RpcInitializePlayer(Player player, string playerName)
    {
        _gameFactory.ConstructPlayer(player, playerName);
    }

    [Server]
    private void InitializePlayer(Player player, string playerName)
    {
        _gameFactory.ConstructPlayer(player, playerName);
    }
}
