using System.Collections;
using UnityEngine;

namespace Gyvr.Mythril2D
{
    public class DashAbility : ActiveAbility<DashAbilitySheet>
    {
        [Header("Reference")]
        [SerializeField] private ParticleSystem m_particleSystem = null;

        private Vector2 m_dirction = Vector2.zero;

        public override void Init(CharacterBase character, AbilitySheet settings)
        {
            base.Init(character, settings);
        }

        public override bool CanFire()
        {
            return base.CanFire() && !m_character.IsBeingPushed();
        }

        protected override void Fire()
        {
            m_dirction =
                m_character.IsMoving() ?
                m_character.movementDirection :
                (m_character.GetLookAtDirection() == EDirection.Right ? Vector2.right : Vector2.left);


            m_character.Push(m_dirction, m_sheet.dashStrength, m_sheet.dashResistance, faceOppositeDirection: true);

            m_particleSystem.Play();

            m_character.TryPlayDashAnimation();

            TerminateCasting();
        }


    }
}
