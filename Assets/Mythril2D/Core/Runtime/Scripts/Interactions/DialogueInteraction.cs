using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class DialogueInteraction : IInteraction
    {
        [SerializeField] private DialogueSequence m_sequence = null;

        public bool TryExecute(CharacterBase source, IInteractionTarget target)
        {
            if (m_sequence != null)
            {
                target.Say(m_sequence);
                return true;
            }

            return false;
        }
    }
}
