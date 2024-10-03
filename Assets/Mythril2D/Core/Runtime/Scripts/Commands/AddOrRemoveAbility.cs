using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class AddOrRemoveAbility : ICommand
    {
        [SerializeField] private EAction m_action = EAction.Add;
        [SerializeField] private AbilitySheet m_abilitySheet = null;
       
        public void Execute()
        {
            switch (m_action)
            {
                case EAction.Add:
                    GameManager.Player.AddBonusAbility(m_abilitySheet);
                    break;

                case EAction.Remove:
                    GameManager.Player.RemoveBonusAbility(m_abilitySheet);
                    break;
            }
        }
    }
}
