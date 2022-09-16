using TMPro;
using UnityEngine;

public sealed class PlayerGUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private TextMeshPro _scoreText;

    private string _currentName;
    private uint _currentScore;
    
    private void Start()
    {
        var player = GetComponent<IPlayer>().NotNull();
        player.Data.Changed += OnInfoChanged;
    }

    private void OnInfoChanged(IPlayer player)
    {
        if (player.Data.Name != _currentName)
        {
            _currentName = player.Data.Name;
            _nameText.text = _currentName;
        }

        if (player.Data.Score != _currentScore)
        {
            _currentScore = player.Data.Score;
            _scoreText.text = _currentScore.ToString();
        }
    }
}
