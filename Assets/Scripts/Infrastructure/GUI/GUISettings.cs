using UnityEngine;

[CreateAssetMenu(menuName = "Settings/GUI", fileName = "GUI Settings")]
public sealed class GUISettings : ScriptableObject
{
    [SerializeField] private int _gameOverPanelWidth = 160;
    [SerializeField] private int _gameOverPanelHeight = 100;

    public int GameOverPanelWidth => _gameOverPanelWidth;
    public int GameOverPanelHeight => _gameOverPanelHeight;
}