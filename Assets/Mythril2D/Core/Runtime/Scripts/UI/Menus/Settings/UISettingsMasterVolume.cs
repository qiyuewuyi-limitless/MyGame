using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Gyvr.Mythril2D
{
    public class UISettingsMasterVolume : UISettingsVolume
    {
        public void RegisterCallbacks(UnityAction decrease, UnityAction increase)
        {
            m_decreaseButton.onClick.AddListener(decrease);
            m_increaseButton.onClick.AddListener(increase);
        }
    }
}
