using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class AddExperience : ICommand
    {
        [SerializeField] private int m_experience;

        public void Execute()
        {
            GameManager.Player.AddExperience(m_experience);
        }
    }
}
