using Mirror;
using UnityEngine;

namespace Infrastructure.Server
{
    public sealed class ServerParticleSystem : NetworkBehaviour
    {
        [SerializeField] private ClientParticleSystem _clientParticleSystem;

        [ClientRpc]
        public void RpcPlaySpawnEffect(Vector3 position)
        {
            _clientParticleSystem.PlaySpawnEffect(position);
        }

        [ClientRpc]
        public void RpcPlayHitEffect(Vector3 position)
        {
            _clientParticleSystem.PlayHitEffect(position);
        }

    }
}