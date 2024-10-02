using UnityEngine;
using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public class Entity : MonoBehaviour, IInteractionTarget
    {
        [Header("Entity Settings")]
        [SerializeReference, SubclassSelector] private IInteraction m_interaction = null;

        public virtual string GetSpeakerName() => string.Empty;

        public virtual void Say(DialogueSequence sequence, UnityAction<DialogueMessageFeed> onDialogueEnded = null, params string[] args)
        {
            string speaker = GetSpeakerName();

            DialogueTree dialogueTree = sequence.ToDialogueTree(speaker, args);

            if (onDialogueEnded != null)
            {
                dialogueTree.dialogueEnded.AddListener(onDialogueEnded);
            }

            GameManager.DialogueSystem.Main.PlayNow(dialogueTree);
        }

        public virtual void OnInteract(CharacterBase sender)
        {
            m_interaction?.TryExecute(sender, this);
        }
    }
}
