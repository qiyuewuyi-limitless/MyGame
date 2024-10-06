using System;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class Monster : Character<MonsterSheet>
    {
        [Header("Monster Settings")]
        [SerializeField] private bool m_permanentDeath = false;
        [SerializeField] private string m_gameFlagID = "monster_00";

        protected override void Awake()
        {
            base.Awake();
            UpdateStats();
        }

        private void Start()
        {
            if (m_permanentDeath && GameManager.GameFlagSystem.Get(m_gameFlagID))
            {
                Destroy(gameObject);
            }
        }

        public void SetLevel(int level)
        {
            m_level = level;
            UpdateStats();
        }

        public void UpdateStats()
        {
            m_stats.Set(m_sheet.stats[m_level]);
        }

        protected override void Die()
        {
            // close the collider and destory the enemy object
            MonsterDie();
            GameManager.NotificationSystem.monsterKilled.Invoke(m_sheet);
            GameManager.Player.AddExperience(m_sheet.experience[m_level]);
            
            m_sheet.executeOnDeath?.Execute();

            if (m_permanentDeath)
            {
                GameManager.GameFlagSystem.Set(m_gameFlagID, true);
            }
        }

        void MonsterDie()
        {
            GameManager.NotificationSystem.audioPlaybackRequested.Invoke(characterSheet.deathAudio);

            if (!TryPlayDeathAnimation())
            {
                
                if (TryPlayDeadAnimation() == true)
                {
                }
            }
            else
            {
                Collider2D[] colliders = GetComponentsInChildren<Collider2D>();
                Array.ForEach(colliders, (collider) => collider.enabled = false);
            }
        }

        void Loot()
        {
            foreach (Loot loot in m_sheet.potentialLoot)
            {
                if (GameManager.Player.level >= loot.minimumPlayerLevel && m_level >= loot.minimumMonsterLevel && loot.IsAvailable() && loot.ResolveDrop())
                {
                    GameManager.InventorySystem.AddToBag(loot.item, loot.quantity);
                }
            }
            GameManager.InventorySystem.AddMoney(m_sheet.money[m_level]);

            OnDeath();

            //animator.SetBool(m_isLootedAnimationParameter, true); private key
            animator.SetBool("isLooted", true);
        }
    }
}
