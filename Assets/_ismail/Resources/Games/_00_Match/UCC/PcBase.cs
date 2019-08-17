
/// ---------------------------------------------
/// Ultimate Character Controller
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

using UnityEngine;
using Opsive.UltimateCharacterController.Events;
using Opsive.UltimateCharacterController.Game;
using Opsive.UltimateCharacterController.Utility;

namespace Opsive.UltimateCharacterController.Character.Abilities
{
    /// <summary>
    /// Memur durulmasını talep eder.
    /// </summary>

    public class PcBase : Ability
    {
        [Tooltip("Specifies if the ability should stop when the OnAnimatorGenericAbilityComplete event is received or wait the specified amount of time before ending the ability.")]
        [SerializeField] protected AnimationEventTrigger m_StopEvent = new AnimationEventTrigger(false, 0.5f);

        public AnimationEventTrigger StopEvent { get { return m_StopEvent; } set { m_StopEvent = value; } }

        /// <summary>
        /// Initialize the default values.
        /// </summary>
        public override void Awake()
        {
            base.Awake();

            EventHandler.RegisterEvent(m_GameObject, "OnAnimatorGenericAbilityComplete", OnComplete);
        }

        /// <summary>
        /// The ability has started.
        /// </summary>
        protected override void AbilityStarted()
        {
            base.AbilityStarted();

            if (!m_StopEvent.WaitForAnimationEvent)
            {
                Scheduler.ScheduleFixed(m_StopEvent.Duration, OnComplete);
            }
        }

        /// <summary>
        /// The animation is done playing - stop the ability.
        /// </summary>
        private void OnComplete()
        {
            StopAbility();
        }

        /// <summary>
        /// The object has been destroyed.
        /// </summary>
        public override void OnDestroy()
        {
            base.OnDestroy();

            EventHandler.UnregisterEvent(m_GameObject, "OnAnimatorGenericAbilityComplete", OnComplete);
        }
    }
}