using System.Collections.Generic;
using Mirror;
using UnityEngine;

public sealed class ServerRoomManager : Mirror.NetworkBehaviour //rename?
{
    [SerializeField] private GameFactory _gameFactory;

    // exclude the point after creation so that another player cannot be spawned on the same point
    private FreeStartPositions _freeStartPositions;
    private Players _players;

    public Players Players => _players;

    public void InitRoom(List<Transform> startPositions, PlayerSpawnMethod playerSpawnMethod)
    {
        _freeStartPositions = new FreeStartPositions(startPositions, playerSpawnMethod);
        _players = new Players(startPositions.Count);
    }

    public void RestartGame()
    {
        _freeStartPositions.Reset();
        _players.ResetScores();
        _players.ResetPositions(_freeStartPositions);

        // foreach (IPlayer player in _players)
        {
            // GameFactory.LookToSceneCenter()
        }

        // base.ServerChangeScene(SceneManager.GetActiveScene().name);
    }

    public Player CreatePlayer(Player playerPrefab)
    {
        Vector3 position = _freeStartPositions.Pop();
        Player player = _gameFactory.CreatePlayer(playerPrefab, position);
        _players.AddPlayer(player);
        return player;
    }
    
    [ClientRpc]
    public void RpcInitializePlayer(Player player, string playerName)
    {
        _gameFactory.ConstructPlayer(player);
        player.Name = playerName;
    }
}
