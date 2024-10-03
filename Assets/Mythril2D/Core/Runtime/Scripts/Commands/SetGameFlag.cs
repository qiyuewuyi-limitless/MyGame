using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class SetGameFlag : ICommand
    {
        [SerializeField] private string m_flagID = string.Empty;
        [SerializeField] private bool m_state = true;

        public void Execute()
        {
            GameManager.GameFlagSystem.Set(m_flagID, m_state);
        }
    }
}
