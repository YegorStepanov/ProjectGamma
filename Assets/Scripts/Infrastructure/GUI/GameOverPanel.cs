using UnityEngine;

public sealed class GameOverPanel
{
    private const string GameOverPanelTemplate = "Game Over\n\n{0} won\n\nRestart after {1:N0} seconds";

    private readonly string _winningPlayerName;
    private readonly float _width;
    private readonly float _height;

    private float _remainingDuration;

    public GameOverPanel(string winningPlayerName, float width, float height, float durationSeconds)
    {
        _winningPlayerName = winningPlayerName;
        _width = width;
        _height = height;
        _remainingDuration = durationSeconds;
    }

    public void Update()
    {
        _remainingDuration -= Time.unscaledDeltaTime;
        _remainingDuration = Mathf.Max(0f, _remainingDuration);
    }

    public void Draw()
    {
        float x = (Screen.width - _width) / 2f;
        float y = (Screen.height - _height) / 2f;

        var rect = new Rect(x, y, _width, _height);
        string text = string.Format(GameOverPanelTemplate, _winningPlayerName, _remainingDuration);
        GUI.Box(rect, text);
    }
}