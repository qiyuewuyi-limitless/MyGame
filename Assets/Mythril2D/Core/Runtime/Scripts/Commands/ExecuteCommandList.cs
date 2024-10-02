using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class ExecuteCommandList : ICommand
    {
        [SerializeReference, SubclassSelector] private ICommand[] m_commands = null;

        public void Execute()
        {
            foreach (ICommand command in m_commands)
            {
                command.Execute();
            }
        }
    }
}
