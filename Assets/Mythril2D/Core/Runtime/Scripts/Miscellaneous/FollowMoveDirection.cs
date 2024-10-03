using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using static Codice.Client.BaseCommands.Import.Commit;

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
                m_target.directionChangedEvent.AddListener(OnTargetDirectionChanged);
                m_target.directionChangedEventOfMe.AddListener(OnTargetDirectionChanged);
            }

            m_initialPosition = transform.localPosition;
        }
        public void OnTargetDirectionChanged(Vector2 direction)
        {
            Debug.Log("运动方向为:" + direction);
            EDirection myDirection = direction.x >= 0.0f ? EDirection.Right:EDirection.Left;
            float modifier = myDirection == EDirection.Right ? 1.0f : -1.0f;
            if (m_strategy == EFollowStrategy.FlipSprites)
            {
                transform.localPosition = new Vector3(m_initialPosition.x * modifier, m_initialPosition.y, m_initialPosition.z);
                if (m_toFlip != null)
                {
                    foreach (SpriteRenderer spriteRenderer in m_toFlip)
                    {
                        spriteRenderer.flipX = myDirection == EDirection.Left;
                    }
                }
            }
            else
            {
                Double angle = Math.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Debug.Log("角度是：" + angle);
                transform.localScale = new Vector3(math.abs(transform.localScale.x) * modifier, transform.localScale.y, transform.localScale.z);
                transform.localRotation = Quaternion.Euler(new Vector3(0, 0, myDirection== EDirection.Right?(float)angle:(float)angle -180));
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
