using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public static class TransformShaker
    {
        public static void Shake(Transform target, float amplitude, float2 frequency, float duration)
        {
            GameManager.Instance.StartCoroutine(
                ShakeCoroutine(target, amplitude, frequency, duration)
            );
        }

        private static IEnumerator ShakeCoroutine(Transform target, float amplitude, float2 frequency, float duration)
        {
            float elapsedTime = 0f;
            Vector3 initialPosition = target.localPosition;

            while (elapsedTime < duration)
            {
                float2 offset = math.sin(frequency * elapsedTime) * amplitude;
                target.localPosition = new Vector3(initialPosition.x + offset.x, initialPosition.y + offset.y, initialPosition.z);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            target.localPosition = initialPosition;
        }
    }
}
