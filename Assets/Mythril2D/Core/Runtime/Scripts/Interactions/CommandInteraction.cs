using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class CommandInteraction : IInteraction
    {
        [SerializeReference, SubclassSelector] private ICommand m_command = null;

        public bool TryExecute(CharacterBase source, IInteractionTarget target)
        {
            if (m_command != null)
            {
                m_command.Execute();
                return true;
            }

            return false;
        }
    }
}
