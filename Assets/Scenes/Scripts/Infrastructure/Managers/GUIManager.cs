using System;
using UnityEngine;

public sealed class GUIManager : Mirror.NetworkBehaviour
{
    [SerializeField]
    private ServerRoomManager _serverRoomManager;

    [SerializeField]
    private RoomManager _roomManager;

    [SerializeField]
    private int width = 160;

    [SerializeField]
    private int height = 100;

    private InGamePlayersScore _inGamePlayersScore;
    private GameOverPanel _gameOverPanel;
    private StartGameButton _startGameButton;

    private void OnGUI()
    {
        _inGamePlayersScore?.Draw();
        _gameOverPanel?.Draw();
        _startGameButton?.Draw();
    }

    private void Update()
    {
        _gameOverPanel?.Update();
    }

    public void ShowGameOverPanel(string winningPlayerName, float durationSeconds) =>
        _gameOverPanel = new GameOverPanel(winningPlayerName, width, height, durationSeconds);

    public void HideGameOverPanel() =>
        _gameOverPanel = null;

    public void ShowInGamePlayersScore() =>
        _inGamePlayersScore = new InGamePlayersScore(_serverRoomManager.Players);

    public void HideInGamePlayersScore() =>
        _inGamePlayersScore = null;

    public void ShowRoomGUI() =>
        _roomManager.showRoomGUI = true;

    public void HideRoomGUI() =>
        _roomManager.showRoomGUI = false; //mb we don't need it?

    public void ShowStartGameButton(Action onClick) =>
        _startGameButton = new StartGameButton(onClick);

    public void HideStartGameButton() =>
        _startGameButton = null;
}
