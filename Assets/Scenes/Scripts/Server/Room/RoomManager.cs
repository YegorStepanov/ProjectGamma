using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public sealed class RoomManager : NetworkBehaviour
{
    [SerializeField] private GameFactory _gameFactory;

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
        Player player = CreatePlayer(playerPrefab);

        GameObject go = player.gameObject;
        NetworkServer.Spawn(go, conn);
        RpcInitializePlayer(player);

        PreparePlayerForGame(player);

        return go;
    }

    public void CreateBot(Player botPrefab)
    {
        Player player = CreatePlayer(botPrefab);
        var botName = $"Bot{_botCounts}";
        player.Name = botName;
        player.name = botName;
        GameObject go = player.gameObject;

        player.GetComponent<NetworkTransform>().clientAuthority = false;

        NetworkServer.Spawn(go);
        RpcInitializePlayer(player);

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

    private Player CreatePlayer(Player playerPrefab)
    {
        Player player = _gameFactory.CreatePlayer(playerPrefab);
        RoomPlayers.AddPlayer(player);
        return player;
    }

    [ClientRpc]
    public void RpcInitializePlayer(Player player)
    {
        player.Construct(player.Data, EmptyInputManager.Instance);
    }

    [Server]
    private void InitializePlayer(Player player)
    {
        player.Construct(player.Data, EmptyInputManager.Instance);
    }
}
