using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class PlayDialogueLine : ICommand
    {
        [SerializeField] private string m_speaker = string.Empty;
        [Multiline][SerializeField] private string m_line = string.Empty;

        public void Execute()
        {
            GameManager.DialogueSystem.Main.AddToQueue(new DialogueTree(new DialogueNode(StringFormatter.Format(m_line), m_speaker)));
            GameManager.DialogueSystem.Main.PlayQueue();
        }
    }
}
