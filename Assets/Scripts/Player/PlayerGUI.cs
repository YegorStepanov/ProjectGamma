using TMPro;
using UnityEngine;

public sealed class PlayerGUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private TextMeshPro _scoreText;

    private string _currentName;
    private uint? _currentScore;

    private void Start()
    {
        var player = GetComponent<IPlayer>().NotNull();
        player.Data.Changed += OnInfoChanged;
        OnInfoChanged(player);
    }

    private void OnInfoChanged(IPlayer player)
    {
        if (player.Data.ScoreData.Name != _currentName)
        {
            // Debug.Log($"Changing Name {_nameText.text}");
            _currentName = player.Data.ScoreData.Name;
            _nameText.text = _currentName;
        }

        if (player.Data.ScoreData.Score != _currentScore)
        {
            // Debug.Log($"Changing Score {_scoreText.text}");
            _currentScore = player.Data.ScoreData.Score;
            _scoreText.text = _currentScore.ToString();
        }
    }
}