using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class UnlockQuest : ICommand
    {
        [SerializeField] private Quest m_quest = null;

        public void Execute()
        {
            Debug.Assert(m_quest != null, "Missing Quest reference!");
            GameManager.JournalSystem.UnlockQuest(m_quest);
        }
    }
}
