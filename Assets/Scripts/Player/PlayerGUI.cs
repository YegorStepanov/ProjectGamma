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
        var player = GetComponent<Player>().NotNull();
        player.Data.Changed += OnInfoChanged;
        OnInfoChanged(player);
        // Debug.Log($"PlayerGUI {player.Data.Name}");
    }

    private void OnInfoChanged(Player player)
    {
        // Debug.Log($"OnInfoChanged {player.Data.Name}");

        if (player.Data.Name != _currentName)
        {
            // Debug.Log($"Changing Name {_nameText.text}");
            _currentName = player.Data.Name;
            _nameText.text = _currentName;
        }

        if (player.Data.Score != _currentScore)
        {
            // Debug.Log($"Changing Score {_scoreText.text}");
            _currentScore = player.Data.Score;
            _scoreText.text = _currentScore.ToString();
        }
    }
}