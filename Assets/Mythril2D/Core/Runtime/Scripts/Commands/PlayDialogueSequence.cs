using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class PlayDialogueSequence : ICommand
    {
        [SerializeField] private DialogueSequence m_dialogueSequence = null;
        [SerializeField] private string m_speaker = string.Empty;

        public void Execute()
        {
            GameManager.DialogueSystem.Main.PlayNow(m_dialogueSequence.ToDialogueTree(m_speaker));
        }
    }
}
