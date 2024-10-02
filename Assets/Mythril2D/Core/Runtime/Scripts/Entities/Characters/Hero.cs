using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public class Hero : Character<HeroSheet>
    {
        [Header("Audio")]
        [SerializeField] private AudioClipResolver m_levelUpSound;

        [Header("Hero")]
        [SerializeField] private bool m_restoreHealthOnLevelUp = true;
        [SerializeField] private bool m_restoreManaOnLevelUp = true;

        public int experience => m_experience;
        public int nextLevelExperience => GetTotalExpRequirement(m_level + 1);
        public int availablePoints => m_sheet.pointsPerLevel * (m_level - Stats.MinLevel) - m_usedPoints;
        public SerializableDictionary<EEquipmentType, Equipment> equipments => m_equipments;
        public AbilitySheet[] equippedAbilities => m_equippedAbilities;
        public HashSet<AbilitySheet> bonusAbilities => m_bonusAbilities;
        public UnityEvent<AbilitySheet[]> equippedAbilitiesChanged => m_equippedAbilitiesChanged;
        public Stats customStats => m_customStats;
        public Stats missingCurrentStats => m_missingCurrentStats;
        public int usedPoints => m_usedPoints;

        public const int MaxEquipedAbilityCount = 4;

        private Stats m_customStats = new Stats();
        private Stats m_missingCurrentStats = new Stats();
        private int m_usedPoints = 0;
        private int m_experience = 0;
        private SerializableDictionary<EEquipmentType, Equipment> m_equipments = new SerializableDictionary<EEquipmentType, Equipment>();
        private HashSet<AbilitySheet> m_bonusAbilities = new HashSet<AbilitySheet>();
        private AbilitySheet[] m_equippedAbilities = new AbilitySheet[MaxEquipedAbilityCount];
        private UnityEvent<AbilitySheet[]> m_equippedAbilitiesChanged = new UnityEvent<AbilitySheet[]>();

        public int GetTotalExpRequirement(int level)
        {
            int total = 0;

            for (int i = 1; i < level; i++)
            {
                total += m_sheet.experience[i];
            }

            return total;
        }

        public void AddExperience(int experience, bool silentMode = false)
        {
            Debug.Assert(experience > 0, "Cannot add a negative amount of experience.");

            GameManager.NotificationSystem.experienceGained.Invoke(experience);

            m_experience += experience;

            while (m_experience >= GetTotalExpRequirement(m_level + 1))
            {
                OnLevelUp(silentMode);
            }
        }

        public void AddCustomStats(Stats customStats)
        {
            m_customStats += customStats;
            UpdateStats();
        }

        public void LogUsedPoints(int points)
        {
            m_usedPoints += points;
        }

        private void Start()
        {
            UpdateStats();
        }

        private void ApplyMissingCurrentStats()
        {
            m_currentStats.Set(m_currentStats.stats - m_missingCurrentStats);
            m_missingCurrentStats.Reset();
        }

        public Equipment Equip(Equipment equipment)
        {
            Equipment previousEquipment = Unequip(equipment.type);
            m_equipments[equipment.type] = equipment;
            GameManager.NotificationSystem.itemEquipped.Invoke(equipment);
            UpdateStats();
            return previousEquipment;
        }

        public Equipment Unequip(EEquipmentType type)
        {
            m_equipments.TryGetValue(type, out Equipment toUnequip);

            if (toUnequip)
            {
                m_equipments.Remove(type);
                GameManager.NotificationSystem.itemUnequipped.Invoke(toUnequip);
                UpdateStats();
            }

            return toUnequip;
        }

        public void AddBonusAbility(AbilitySheet ability)
        {
            if (!m_bonusAbilities.Contains(ability))
            {
                m_bonusAbilities.Add(ability);
                AddAbility(ability);
            }
        }

        public void RemoveBonusAbility(AbilitySheet ability)
        {
            if (!m_bonusAbilities.Contains(ability))
            {
                Debug.LogAssertion("Cannot remove an ability that hasn't been added in the first place.");
            }
            else
            {
                m_bonusAbilities.Remove(ability);
                RemoveAbility(ability);
            }
        }

        protected override bool AddAbility(AbilitySheet ability)
        {
            if (base.AddAbility(ability))
            {
                if (!IsAbilityEquiped(ability))
                {
                    for (int i = 0; i < m_equippedAbilities.Length; ++i)
                    {
                        if (m_equippedAbilities[i] == null)
                        {
                            Equip(ability, i);
                        }
                    }
                }

                GameManager.NotificationSystem.abilityAdded.Invoke(ability);
                return true;
            }

            return false;
        }

        protected override bool RemoveAbility(AbilitySheet ability)
        {
            if (base.RemoveAbility(ability))
            {
                for (int i = 0; i < m_equippedAbilities.Length; ++i)
                {
                    if (m_equippedAbilities[i] == ability)
                    {
                        Unequip(i);
                    }
                }

                GameManager.NotificationSystem.abilityRemoved.Invoke(ability);
                return true;
            }

            return false;
        }

        public void Equip(AbilitySheet ability, int index)
        {
            m_equippedAbilities[index] = ability;
            m_equippedAbilitiesChanged.Invoke(m_equippedAbilities);
        }

        public void Unequip(int index)
        {
            m_equippedAbilities[index] = null;
            m_equippedAbilitiesChanged.Invoke(m_equippedAbilities);
        }

        public bool IsAbilityEquiped(AbilitySheet abilitySheet)
        {
            foreach (AbilitySheet ability in m_equippedAbilities)
            {
                if (ability == abilitySheet) return true;
            }

            return false;
        }

        private Stats CalculateEquipmentStats()
        {
            Stats equipmentStats = new Stats();

            foreach (Equipment piece in m_equipments.Values)
            {
                if (piece)
                {
                    equipmentStats += piece.bonusStats;
                }
            }

            return equipmentStats;
        }

        private void UpdateStats()
        {
            Stats equipmentStats = CalculateEquipmentStats();
            Stats totalStats = m_sheet.baseStats + m_customStats + equipmentStats;

            m_stats.Set(totalStats);

            ApplyMissingCurrentStats();
        }

        private void OnLevelUp(bool silentMode = false)
        {
            ++m_level;

            if (!silentMode)
            {
                if (m_restoreHealthOnLevelUp)
                {
                    Heal(m_stats[EStat.Health] - m_currentStats[EStat.Health]);
                }

                if (m_restoreManaOnLevelUp)
                {
                    RecoverMana(m_stats[EStat.Mana] - m_currentStats[EStat.Mana]);
                }

                GameManager.NotificationSystem.levelUp.Invoke(m_level);
                GameManager.NotificationSystem.audioPlaybackRequested.Invoke(m_levelUpSound);
            }

            foreach (AbilitySheet ability in m_sheet.GetAbilitiesUnlockedAtLevel(m_level))
            {
                AddAbility(ability);
            }
        }

        public void Initialize(PlayerDataBlock block)
        {
            m_usedPoints = block.usedPoints;

            if (block.experience > 0)
            {
                AddExperience(block.experience, true);
            }

            foreach (var ability in block.bonusAbilities)
            {
                AddBonusAbility(GameManager.Database.LoadFromReference(ability));
            }

            m_equipments = new SerializableDictionary<EEquipmentType, Equipment>();

            foreach (var piece in block.equipments)
            {
                Equipment instance = GameManager.Database.LoadFromReference(piece);
                m_equipments[instance.type] = instance;
            }

            m_customStats = block.customStats;

            // Copy missing current stats so block data doesn't get altered
            m_missingCurrentStats = new Stats(block.missingCurrentStats);

            // Clear equipped abilities
            for (int i = 0; i < m_equippedAbilities.Length; ++i)
            {
                m_equippedAbilities[i] = i < block.equippedAbilities.Length ? GameManager.Database.LoadFromReference(block.equippedAbilities[i]) : null;
            }

            m_equippedAbilitiesChanged.Invoke(m_equippedAbilities);

            transform.position = block.position;
        }

        protected override void OnDeath()
        {
            m_destroyOnDeath = false; // Prevents the Hero GameObject from being destroyed, so it can be used in the death screen.
            base.OnDeath();
            GameManager.NotificationSystem.deathScreenRequested.Invoke();
        }
    }
}
