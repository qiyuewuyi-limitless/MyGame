using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class ShopInteraction : IInteraction
    {
        [Header("Dialogues")]
        [SerializeField] private DialogueSequence m_dialogue = null;

        [Header("References")]
        [SerializeField] private Shop m_shop = null;

        public bool TryExecute(CharacterBase source, IInteractionTarget target)
        {
            if (m_shop != null)
            {
                target.Say(m_dialogue, (messages) =>
                {
                    if (messages.Contains(EDialogueMessageType.Accept))
                    {
                        GameManager.NotificationSystem.shopRequested.Invoke(m_shop);
                    }
                });

                return true;
            }

            return false;
        }
    }
}
