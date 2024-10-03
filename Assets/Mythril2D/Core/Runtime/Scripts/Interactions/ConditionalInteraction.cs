using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class ConditionalInteraction : IInteraction
    {
        [SerializeReference, SubclassSelector] private ICondition m_condition = null;
        [SerializeReference, SubclassSelector] private IInteraction m_interaction = null;

        public bool TryExecute(CharacterBase source, IInteractionTarget target)
        {
            if (m_condition?.Evaluate() ?? true)
            {
                return m_interaction.TryExecute(source, target);
            }

            return false;
        }
    }
}
