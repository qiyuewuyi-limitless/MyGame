using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class ExecuteCommandIf : ICommand
    {
        [SerializeReference, SubclassSelector] private ICondition m_condition = null;
        [SerializeReference, SubclassSelector] private ICommand m_ifTrue = null;
        [SerializeReference, SubclassSelector] private ICommand m_ifFalse = null;

        public void Execute()
        {
            if (m_condition?.Evaluate() ?? true)
            {
                m_ifTrue.Execute();
            }
            else
            {
                m_ifFalse.Execute();
            }
        }
    }
}
