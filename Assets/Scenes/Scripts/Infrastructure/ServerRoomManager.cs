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

        // // spawn the initial batch of Rewards
        // if (sceneName == GameplayScene)
        //     Spawner.InitialSpawn();
    }

    public void RestartGame()
    {
        _freeStartPositions.Reset();
        _players.ResetScores();
        _players.ResetPositions(_freeStartPositions);

        // base.ServerChangeScene(SceneManager.GetActiveScene().name);
    }

    public Player CreatePlayer(Player playerPrefab)
    {
        //rotate to the center!
        Vector3 position = _freeStartPositions.Pop();
        Vector3 lookDirection = LookToSceneCenter(position); //move it too
        Player player = _gameFactory.CreatePlayer(playerPrefab, position, Quaternion.LookRotation(lookDirection));
        player.Name = "Bot 1";
        // player.SetState(PlayerState.Walk);
        _players.AddPlayer(player);
        return player;
    }

    
    public void CreateTestPlayer(Player playerPrefab)
    {
        Vector3 position = _freeStartPositions.Pop();
        Vector3 lookDirection = LookToSceneCenter(position);
        Player player = _gameFactory.CreateTestPlayer(playerPrefab, position, Quaternion.LookRotation(lookDirection));
        player.Name = "Player 1";
        // player.SetState(PlayerState.Walk);
        _players.AddPlayer(player);
    }

    private static Vector3 LookToSceneCenter(Vector3 position)
    {
        Vector3 lookDirection = -position;
        lookDirection.y = 0;
        return lookDirection;
    }

    // public void PreparePlayer(GameObject roomPlayer, GameObject gamePlayer)
    // {
    //     // var playerScore = gamePlayer.GetComponent<Player>();
    //     // playerScore.Name = "Player " + roomPlayer.GetComponent<NetworkRoomPlayer>().index;
    // }
}
