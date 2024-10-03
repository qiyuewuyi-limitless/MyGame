using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class ExecuteCommandHandler : ICommand
    {
        [SerializeField] private CommandHandler m_commandHandler = null;

        public void Execute() => m_commandHandler?.Execute();
    }
}
