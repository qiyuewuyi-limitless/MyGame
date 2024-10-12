using UnityEngine.Events;

namespace Gyvr.Mythril2D
{
    public interface ITriggerableAbility
    {
        public void Fire(UnityAction onAbilityEnded);
        public bool CanFire();
        public AbilityBase GetAbilityBase();
    }

    public abstract class ActiveAbility<SheetType> : Ability<SheetType>, ITriggerableAbility where SheetType : AbilitySheet
    {
        private UnityAction m_onAbilityEndedCallback = null;

        public void Fire(UnityAction onAbilityEnded)
        {
            m_onAbilityEndedCallback = onAbilityEnded;
            m_character.DisableActions(m_sheet.disabledActionsWhileCasting);
            Fire();
            ConsumeMana();
            ConsumeStamina();
        }

        public virtual bool CanFire()
        {
            if(m_character.tag == "Player")
                return m_character.Can(EActionFlags.UseAbility) && m_character.currentStats[EStat.Mana] >= m_sheet.manaCost && m_character.currentStats[EStat.Stamina] >= m_sheet.staminaCost;
            else
                return m_character.Can(EActionFlags.UseAbility) && m_character.currentStats[EStat.Mana] >= m_sheet.manaCost;
        }

        protected virtual void ConsumeMana()
        {
            m_character.ConsumeMana(m_sheet.manaCost);
        }

        protected virtual void ConsumeStamina()
        {
            m_character.ConsumeStamina(m_sheet.staminaCost);
        }

        protected void TerminateCasting()
        {
            m_character.EnableActions(m_sheet.disabledActionsWhileCasting);
            m_onAbilityEndedCallback?.Invoke();
            m_onAbilityEndedCallback = null;
        }

        protected abstract void Fire();

        public AbilityBase GetAbilityBase() => this;
    }
}
