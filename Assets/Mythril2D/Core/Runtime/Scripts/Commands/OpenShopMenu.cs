using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class OpenShopMenu : ICommand
    {
        [SerializeField] private Shop m_shop = null;

        public void Execute()
        {
            Debug.Assert(m_shop != null, "Missing Shop reference!");
            GameManager.NotificationSystem.shopRequested.Invoke(m_shop);
        }
    }
}
