using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class CameraShake : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float m_amplitude = 0.05f;
        [SerializeField] private float2 m_frequency = new float2(60.0f, 50.0f);
        [SerializeField] private float m_duration = 0.2f;
        [SerializeField] private float m_criticalHitAmplitudeModifier = 2.0f;

        private void OnEnable()
        {
            GameManager.NotificationSystem.damageApplied.AddListener(OnDamageApplied);
        }

        private void OnDisable()
        {
            GameManager.NotificationSystem.damageApplied.RemoveListener(OnDamageApplied);
        }

        private bool IsCameraAllowedToShake()
        {
            return GameManager.Config.cameraShakeSources != ECameraShakeSources.None;
        }

        private bool IsValidShakeSource(CharacterBase target, DamageInputDescriptor damageInputDescriptor)
        {
            return
                (
                    GameManager.Config.cameraShakeSources.HasFlag(ECameraShakeSources.PlayerReceiveDamage) &&
                    target == GameManager.Player
                )
                ||
                (
                    GameManager.Config.cameraShakeSources.HasFlag(ECameraShakeSources.AnyCharacterReceiveDamageFromPlayer) &&
                    damageInputDescriptor.attacker == (object)GameManager.Player
                );
        }

        private void OnDamageApplied(CharacterBase target, DamageInputDescriptor damageInputDescriptor)
        {
            if (IsCameraAllowedToShake() && IsValidShakeSource(target, damageInputDescriptor))
            {
                if (!damageInputDescriptor.flags.HasFlag(EDamageFlag.Miss))
                {
                    bool isCriticalHit = damageInputDescriptor.flags.HasFlag(EDamageFlag.Critical);
                    float amplitude = isCriticalHit ? m_amplitude * m_criticalHitAmplitudeModifier : m_amplitude;
                    TransformShaker.Shake(transform, amplitude, m_frequency, m_duration);
                }
            }
        }
    }
}
