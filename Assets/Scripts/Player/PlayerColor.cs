using UnityEngine;

public sealed class PlayerColor : MonoBehaviour
{
    [SerializeField] private Renderer _renderer;

    private Material _material;
    private IPlayer _player;

    private void Start()
    {
        _player = GetComponent<IPlayer>().NotNull();
        _player.Data.Changed += OnColorChanged;

        _material = CloneMaterial(_renderer);
    }

    private void OnDestroy()
    {
        _player.Data.Changed -= OnColorChanged;

        Destroy(_material);
    }

    private static Material CloneMaterial(Renderer renderer) =>
        renderer.material;

    private void OnColorChanged(IPlayer player) =>
        _material.color = player.Data.Color;
}