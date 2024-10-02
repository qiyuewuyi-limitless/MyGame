using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public class QuestInteraction : IInteraction
    {
        private bool TryCompletingQuest(NPC npc)
        {
            Quest quest = GameManager.JournalSystem.GetQuestToComplete(npc);

            if (quest)
            {
                if (quest.questCompletedDialogue != null)
                {
                    npc.Say(quest.questCompletedDialogue, (actionFeed) =>
                    {
                        GameManager.JournalSystem.CompleteQuest(quest);
                    });

                    return true;
                }
                else
                {
                    Debug.LogErrorFormat("No quest completed dialogue provided for [{0}]", quest.title);
                }
            }

            return false;
        }

        private bool TryGivingHint(NPC npc)
        {
            // Try to find a hint for a fullfilled quest (quest with no task, such as "Talk to X")
            Quest quest = GameManager.JournalSystem.GetFullfilledQuest(npc);

            if (!quest)
            {
                // Try to find a hint for a started quest
                quest = GameManager.JournalSystem.GetStartedQuest(npc);
            }

            if (quest != null && quest.questHintDialogue != null)
            {
                npc.Say(quest.questHintDialogue);
                return true;
            }

            return false;
        }

        private bool TryOfferingQuest(NPC npc)
        {
            Quest quest = GameManager.JournalSystem.GetQuestToStart(npc);

            if (quest)
            {
                if (quest.questOfferDialogue != null)
                {
                    npc.Say(quest.questOfferDialogue, (messages) =>
                    {
                        if (messages.Contains(EDialogueMessageType.Accept))
                        {
                            GameManager.JournalSystem.StartQuest(quest);
                        }
                    });

                    return true;
                }
                else
                {
                    Debug.LogErrorFormat("No quest offer dialogue provided for [{0}]", quest.title);
                }
            }

            return false;
        }

        public bool TryExecute(CharacterBase source, IInteractionTarget target)
        {
            if (target is NPC npc)
            {
                if (!TryCompletingQuest(npc))
                {
                    if (!TryOfferingQuest(npc))
                    {
                        if (!TryGivingHint(npc))
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                Debug.LogError("QuestInteraction can only be used with NPC targets.");
                return false;
            }

            return true;
        }
    }
}
