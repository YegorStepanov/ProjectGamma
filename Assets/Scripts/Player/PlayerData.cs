using System;
using Mirror;
using UnityEngine;

public sealed class PlayerData : NetworkBehaviour //, IPlayerData
{
    public event Action<string> NameChanged;
    public event Action<uint> ScoreChanged;
    public event Action<Color32> ColorChanged;

    [SyncVar(hook = nameof(OnNameChanged))]
    private string _name = "";
    [SyncVar(hook = nameof(OnScoreChanged))]
    private uint _score;
    [SyncVar(hook = nameof(OnColorChanged))]
    private Color32 _color;

    public int Layer => gameObject.layer;

    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            NameChanged?.Invoke(value);
        }
    }

    public uint Score
    {
        get => _score;
        set
        {
            if (_score == value) return;
            _score = value;
            ScoreChanged?.Invoke(value);
        }
    }

    public Color32 Color
    {
        get => _color;
        set
        {
            if ((Color)_color == value) return;
            _color = value;
            ColorChanged?.Invoke(value);
        }
    }

    private void OnDestroy()
    {
        NameChanged = null;
        ScoreChanged = null;
        ColorChanged = null;
    }

    private void OnNameChanged(string oldValue, string newValue) =>
        NameChanged?.Invoke(newValue);

    private void OnScoreChanged(uint oldValue, uint newValue) =>
        ScoreChanged?.Invoke(newValue);

    private void OnColorChanged(Color32 oldValue, Color32 newValue) =>
        ColorChanged?.Invoke(newValue);
}