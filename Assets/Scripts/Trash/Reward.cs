using Mirror;

public class Reward : NetworkBehaviour
{
    // public bool available = true;
    // // public ColoringRenderer _coloringRenderer;
    //
    // void OnValidate()
    // {
    //     // if (_coloringRenderer == null)
    //     //     _coloringRenderer = GetComponent<ColoringRenderer>();
    // }
    //
    // [ServerCallback]
    // void OnTriggerEnter(Collider other)
    // {
    //     Debug.Log("REWARD TRIGGER");
    //     if (other.gameObject.CompareTag("Player"))
    //     {
    //         ClaimPrize(other.gameObject);
    //     }
    // }
    //
    // // This is called from PlayerController.CmdClaimPrize which is invoked by PlayerController.OnControllerColliderHit
    // // This only runs on the server
    // public void ClaimPrize(GameObject player)
    // {
    //     if (available)
    //     {
    //         // This is a fast switch to prevent two players claiming the prize in a bang-bang close contest for it.
    //         // First hit turns it off, pending the object being destroyed a few frames later.
    //         available = false;
    //
    //         Color32 color = _coloringRenderer.color;
    //
    //         // calculate the points from the color ... lighter scores higher as the average approaches 255
    //         // UnityEngine.Color RGB values are float fractions of 255
    //         uint points = (uint)(((color.r) + (color.g) + (color.b)) / 3);
    //         //Debug.Log($"Scored {points} points R:{color.r} G:{color.g} B:{color.b}");
    //
    //         // award the points via SyncVar on the PlayerController
    //         player.GetComponent<Player>().Score += points;
    //
    //         // spawn a replacement
    //         Spawner.SpawnReward();
    //
    //         // destroy this one
    //         //NetworkServer.Destroy(gameObject);
    //     }
    // }
}
