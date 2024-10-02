using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [CreateAssetMenu(menuName = AssetMenuIndexer.Mythril2D_Utils + nameof(CommandHandler))]
    public class CommandHandler : DatabaseEntry
    {
        [SerializeReference, SubclassSelector]
        private ICommand m_command = null;

        public void Execute() => m_command.Execute();
    }
}
