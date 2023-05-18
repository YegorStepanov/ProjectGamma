using UnityEngine;

public sealed class PlayerColor : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private Material _material;
    private Player _player;

    private void Start()
    {
        _material = CloneMaterial(_renderer);

        _player = GetComponent<Player>().NotNull();
        _player.Data.ColorChanged += OnColorChanged;
        OnColorChanged(_player.Data.Color);
    }

    private void OnDestroy()
    {
        _player.Data.ColorChanged -= OnColorChanged;

        Destroy(_material);
    }

    private void OnColorChanged(Color32 color) =>
        _material.color = color;

    private static Material CloneMaterial(Renderer renderer) =>
        renderer.material;
}