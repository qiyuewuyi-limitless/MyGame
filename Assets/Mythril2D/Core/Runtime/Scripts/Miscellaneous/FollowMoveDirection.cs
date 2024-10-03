using Unity.Mathematics;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    class FollowMoveDirection : MonoBehaviour
    {
        public enum EFollowStrategy
        {
            FlipSprites,
            NegativeScale
        }

        // Inspector Settings
        [SerializeField] private CharacterBase m_target = null;
        [SerializeField] private EFollowStrategy m_strategy = EFollowStrategy.FlipSprites;
        [SerializeField] private SpriteRenderer[] m_toFlip = null;

        // Private Members
        private Vector3 m_initialPosition;

        public void Awake()
        {
            if (m_target != null)
            {
                m_target.directionChangedEventOfMe.AddListener(OnTargetDirectionChanged);
                m_target.directionChangedEvent.AddListener(OnTargetDirectionChanged);
            }

            m_initialPosition = transform.localPosition;
        }
        public void OnTargetDirectionChanged(Vector2 direction)
        {
            EDirection myEDirection = direction.x >= 0.0f ? EDirection.Right : EDirection.Left;
            float modifier = myEDirection == EDirection.Right ? 1.0f : -1.0f;
            if (m_strategy == EFollowStrategy.FlipSprites)
            {
                transform.localPosition = new Vector3(m_initialPosition.x * modifier, m_initialPosition.y, m_initialPosition.z);

                if (m_toFlip != null)
                {
                    foreach (SpriteRenderer spriteRenderer in m_toFlip)
                    {
                        spriteRenderer.flipX = myEDirection == EDirection.Left;
                    }
                }
            }
            else
            {
                float angle = math.atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Debug.Log("angle is " + angle);
                transform.localRotation = Quaternion.Euler(0.0f,0.0f,myEDirection == EDirection.Left ? angle - 180.0f : angle);
                transform.localScale = new Vector3(math.abs(transform.localScale.x) * modifier, transform.localScale.y, transform.localScale.z);
            }
        }
        public void OnTargetDirectionChanged(EDirection direction)
        {
            float modifier = direction == EDirection.Right ? 1.0f : -1.0f;

            if (m_strategy == EFollowStrategy.FlipSprites)
            {
                transform.localPosition = new Vector3(m_initialPosition.x * modifier, m_initialPosition.y, m_initialPosition.z);

                if (m_toFlip != null)
                {
                    foreach (SpriteRenderer spriteRenderer in m_toFlip)
                    {
                        spriteRenderer.flipX = direction == EDirection.Left;
                    }
                }
            }
            else
            {
                transform.localScale = new Vector3(math.abs(transform.localScale.x) * modifier, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
