using UnityEngine;

namespace Infrastructure
{
    public sealed class ClientParticleSystem : MonoBehaviour
    {
        [SerializeField] private ParticleSystem _spawnEffect;
        [SerializeField] private ParticleSystem _hitEffect;

        [SerializeField] private float _spawnEffectFastForwardTime = 0.3f;

        public void PlaySpawnEffect(Vector3 position)
        {
            ParticleSystem effect = Instantiate(_spawnEffect, position, Quaternion.identity);
            effect.Simulate(_spawnEffectFastForwardTime);
            effect.Play();
        }

        public void PlayHitEffect(Vector3 position)
        {
            Instantiate(_hitEffect, position, Quaternion.identity);
        }
    }
}