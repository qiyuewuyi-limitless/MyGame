using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    [Serializable]
    public struct JournalDataBlock
    {
        public List<DatabaseEntryReference<Quest>> unlockedQuests;
        public List<QuestProgressDataBlock> activeQuests;
        public List<DatabaseEntryReference<Quest>> fullfilledQuests;
        public List<DatabaseEntryReference<Quest>> completedQuests;
    }

    public class JournalSystem : AGameSystem, IDataBlockHandler<JournalDataBlock>
    {
        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_questStartedSound;
        [SerializeField] private AudioClipResolver m_questCompletedSound;

        public List<Quest> unlockedQuests => m_unlockedQuests;
        public List<Quest> availableQuests => m_availableQuests;
        public List<QuestProgress> activeQuests => m_activeQuests;
        public List<Quest> fullfilledQuests => m_fullfilledQuests;
        public List<Quest> completedQuests => m_completedQuests;

        private List<Quest> m_unlockedQuests = new List<Quest>();
        private List<Quest> m_availableQuests = new List<Quest>();
        private List<QuestProgress> m_activeQuests = new List<QuestProgress>();
        private List<Quest> m_fullfilledQuests = new List<Quest>();
        private List<Quest> m_completedQuests = new List<Quest>();

        public bool IsQuestUnlocked(Quest quest) => m_unlockedQuests.Contains(quest);
        public bool IsQuestAvailable(Quest quest) => m_availableQuests.Contains(quest);
        public bool IsQuestActive(Quest quest) => m_activeQuests.Find((QuestProgress progress) => progress.quest == quest) != null;
        public bool IsQuestFullfilled(Quest quest) => m_fullfilledQuests.Contains(quest);
        public bool IsQuestCompleted(Quest quest) => m_completedQuests.Contains(quest);

        public bool IsTaskActive(QuestTask task)
        {
            foreach (QuestProgress progress in activeQuests)
            {
                if (progress.currentTasks.Find((taskProgress) => taskProgress.task == task) != null)
                {
                    return true;
                }
            }

            return false;
        }

        public void StartQuest(Quest quest)
        {
            QuestProgress instance = new QuestProgress(quest, OnQuestFullfilled);
            m_unlockedQuests.Remove(quest);
            m_availableQuests.Remove(quest);
            activeQuests.Add(instance);
            GameManager.NotificationSystem.questStarted.Invoke(quest);
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_questStartedSound);

            // Some quests have no tasks, and thus should be considered as fullfilled right away
            instance.CheckFullfillment();
        }

        public void OnQuestFullfilled(QuestProgress instance)
        {
            fullfilledQuests.Add(instance.quest);
            activeQuests.Remove(instance);
            GameManager.NotificationSystem.questFullfilled.Invoke(instance.quest);
        }

        public void CompleteQuest(Quest quest)
        {
            fullfilledQuests.Remove(quest);
            completedQuests.Add(quest);

            GameManager.NotificationSystem.questCompleted.Invoke(quest);
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_questCompletedSound);

            if (quest.repeatable)
            {
                UnlockQuest(quest);
            }

            quest.toExecuteOnQuestCompletion?.Execute();
        }

        public void UnlockQuest(Quest quest)
        {
            m_unlockedQuests.Add(quest);

            if (!m_availableQuests.Contains(quest) && CheckIfQuestRequirementsAreMet(quest))
            {
                m_availableQuests.Add(quest);
                GameManager.NotificationSystem.questAvailabilityChanged.Invoke(quest, true);
            }

            GameManager.NotificationSystem.questUnlocked.Invoke(quest);
        }

        public QuestProgress GetNonFullfilledQuestToReportTo(NPC npc)
        {
            return activeQuests.Find((quest) => quest.quest.reportTo == npc.characterSheet);
        }

        public Quest GetQuestToComplete(NPC npc)
        {
            return fullfilledQuests.Find((quest) => quest.reportTo == npc.characterSheet);
        }

        public Quest GetQuestToStart(NPC npc)
        {
            return availableQuests.Find((quest) => quest.offeredBy == npc.characterSheet);
        }

        public Quest GetStartedQuest(NPC npc)
        {
            List<QuestProgress> results = activeQuests.FindAll((quest) => quest.quest.offeredBy == npc.characterSheet);
            return results != null && results.Count > 0 ? results[0].quest : null;
        }

        public Quest GetFullfilledQuest(NPC npc)
        {
            List<Quest> results = fullfilledQuests.FindAll((quest) => quest.offeredBy == npc.characterSheet);
            return results != null && results.Count > 0 ? results[0] : null;
        }

        
        private void UpdateQuestsAvailability()
        {
            foreach (Quest quest in m_unlockedQuests)
            {
                if (!m_availableQuests.Contains(quest) && CheckIfQuestRequirementsAreMet(quest))
                {
                    m_availableQuests.Add(quest);
                    GameManager.NotificationSystem.questAvailabilityChanged.Invoke(quest, true);
                }
                else if (m_availableQuests.Contains(quest) && !CheckIfQuestRequirementsAreMet(quest))
                {
                    m_availableQuests.Remove(quest);
                    GameManager.NotificationSystem.questAvailabilityChanged.Invoke(quest, false);
                }
            }
        }

        private bool CheckIfQuestRequirementsAreMet(Quest quest)
        {
            return quest.requiredLevel <= GameManager.Player.level;
        }

        private void Start()
        {
            GameManager.NotificationSystem.levelUp.AddListener((int level) => UpdateQuestsAvailability());
        }

        public void LoadDataBlock(JournalDataBlock block)
        {
            m_unlockedQuests = block.unlockedQuests.Select(quest => GameManager.Database.LoadFromReference(quest)).ToList();
            m_activeQuests = new List<QuestProgress>(block.activeQuests.Count);
            m_fullfilledQuests = block.fullfilledQuests.Select(quest => GameManager.Database.LoadFromReference(quest)).ToList();
            m_completedQuests = block.completedQuests.Select(quest => GameManager.Database.LoadFromReference(quest)).ToList();

            foreach (QuestProgressDataBlock progressDataBlock in block.activeQuests)
            {
                QuestProgress progress = new QuestProgress(progressDataBlock, OnQuestFullfilled);
                m_activeQuests.Add(progress);
                progress.CheckFullfillment();
            }

            UpdateQuestsAvailability();
        }

        public JournalDataBlock CreateDataBlock()
        {
            return new JournalDataBlock
            {
                unlockedQuests = m_unlockedQuests.Select(quest => GameManager.Database.CreateReference(quest)).ToList(),
                activeQuests = new List<QuestProgressDataBlock>(m_activeQuests.Select(qi => qi.CreateDataBlock())),
                fullfilledQuests = m_fullfilledQuests.Select(quest => GameManager.Database.CreateReference(quest)).ToList(),
                completedQuests = m_completedQuests.Select(quest => GameManager.Database.CreateReference(quest)).ToList()
            };
        }
    }
}