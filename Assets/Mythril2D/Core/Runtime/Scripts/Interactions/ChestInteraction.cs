using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class ChestInteraction : IInteraction
    {
        [SerializeField] private Chest m_chest = null;

        public bool TryExecute(CharacterBase source, IInteractionTarget target)
        {
            return m_chest.TryOpen();
        }
    }
}
