using TMPro;
using UnityEngine;

public sealed class PlayerGUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private TextMeshPro _scoreText;

    private Player _player;

    private void Start()
    {
        _player = GetComponent<Player>().NotNull();

        _player.Data.NameChanged += OnNameChanged;
        _player.Data.ScoreChanged += OnScoreChanged;

        OnNameChanged();
        OnScoreChanged();
    }

    private void OnDestroy()
    {
        _player.Data.NameChanged -= OnNameChanged;
        _player.Data.ScoreChanged -= OnScoreChanged;
    }

    private void OnNameChanged(string _ = default) =>
        _nameText.text = _player.Data.Name;

    private void OnScoreChanged(uint _ = default) =>
        _scoreText.text = _player.Data.Score.ToString();
}