using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class CraftInteraction : IInteraction
    {
        [Header("Dialogues")]
        [SerializeField] private DialogueSequence m_dialogue = null;

        [Header("References")]
        [SerializeField] private CraftingStation m_craftingStation = null;

        public bool TryExecute(CharacterBase source, IInteractionTarget target)
        {
            if (m_craftingStation != null)
            {
                target.Say(m_dialogue, (messages) =>
                {
                    if (messages.Contains(EDialogueMessageType.Accept))
                    {
                        GameManager.NotificationSystem.craftRequested.Invoke(m_craftingStation);
                    }
                });

                return true;
            }

            return false;
        }
    }
}
