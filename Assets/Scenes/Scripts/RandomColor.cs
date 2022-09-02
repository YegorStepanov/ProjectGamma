using Mirror;
using UnityEngine;

public class RandomColor : NetworkBehaviour
{
    [SyncVar(hook = nameof(SetColor))]
    public Color32 color = Color.black;

    // Unity clones the material when GetComponent<Renderer>().material is called
    // Cache it here and destroy it in OnDestroy to prevent a memory leak
    private Material _cachedMaterial;

    public override void OnStartServer()
    {
        color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
    }

    private void SetColor(Color32 _, Color32 newColor)
    {
        if (_cachedMaterial == null)
            _cachedMaterial = GetComponentInChildren<Renderer>().material;

        _cachedMaterial.color = newColor;
    }

    private void OnDestroy()
    {
        Destroy(_cachedMaterial);
    }
}
