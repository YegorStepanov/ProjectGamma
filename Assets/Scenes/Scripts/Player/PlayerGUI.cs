using TMPro;
using UnityEngine;

public sealed class PlayerGUI : MonoBehaviour
{
    [SerializeField] private TextMeshPro _nameText;
    [SerializeField] private TextMeshPro _scoreText;

    private void Awake()
    {
        var player = GetComponent<IPlayer>();
        player.InfoChanged += OnInfoChanged;
    }

    private void OnInfoChanged(Player player)
    {
        _nameText.text = player.Name;
        _scoreText.text = player.Score.ToString();
    }
}
