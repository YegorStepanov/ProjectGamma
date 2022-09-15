using UnityEngine;

[CreateAssetMenu(menuName = "Data/GUI", fileName = "GUIData")]
public sealed class GUIData : ScriptableObject
{
    [SerializeField] private int _gameOverPanelWidth = 160;
    [SerializeField] private int _gameOverPanelHeight = 100;

    public int GameOverPanelWidth => _gameOverPanelWidth;
    public int GameOverPanelHeight => _gameOverPanelHeight;
}
