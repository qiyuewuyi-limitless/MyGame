using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class OpenCraftMenu : ICommand
    {
        [SerializeField] private CraftingStation m_craftingStation = null;

        public void Execute()
        {
            Debug.Assert(m_craftingStation != null, "Missing CraftingStation reference!");
            GameManager.NotificationSystem.craftRequested.Invoke(m_craftingStation);
        }
    }
}
