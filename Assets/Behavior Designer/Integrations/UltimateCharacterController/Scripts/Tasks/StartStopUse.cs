using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Tries to start or stop the use ability.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class StartStopUse : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The SlotID of the use ability.")]
        public SharedInt m_SlotID = -1;
        [Tooltip("The ActionID of the use ability.")]
        public SharedInt m_ActionID;
        [Tooltip("Should the ability be started?")]
        public SharedBool m_Start = true;
        [Tooltip("Should the ability wait to stop until the use has completed? This only applies if the item is starting to be used.")]
        public SharedBool m_WaitForUseComplete = true;
        [Tooltip("Should the task always return success even if the item isn't used successfully?")]
        public SharedBool m_AlwaysReturnSuccess;
        [Tooltip("The GameObject that the character should aim at.")]
        public SharedGameObject m_AimTarget;

        private Opsive.UltimateCharacterController.Character.Abilities.Items.Use m_UseAbility;

        private GameObject m_PrevTarget;
        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private LocalLookSource m_LocalLookSource;
        private bool m_WaitForUse;
        private bool m_WaitForStopUse;

        /// <summary>
        /// Retrieves the use ability.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_CharacterLocomotion = target.GetCachedComponent<UltimateCharacterLocomotion>();
                m_LocalLookSource = target.GetCachedComponent<LocalLookSource>();
                // Find the specified ability.
                var abilities = m_CharacterLocomotion.GetAbilities<Opsive.UltimateCharacterController.Character.Abilities.Items.Use>();
                // The slot ID and action ID must match.
                for (int i = 0; i < abilities.Length; ++i) {
                    if (abilities[i].SlotID == m_SlotID.Value && abilities[i].ActionID == m_ActionID.Value) {
                        m_UseAbility = abilities[i];
                        break;
                    }
                }
                if (m_UseAbility == null) {
                    Debug.LogWarning("Error: Unable to find a Use ability with slot " + m_SlotID.Value + " and action " + m_ActionID.Value);
                    return;
                }
                m_PrevTarget = target;
            }
            m_WaitForUse = m_WaitForStopUse = false;
        }

        /// <summary>
        /// Tries to start or stop the use of the current item.
        /// </summary>
        /// <returns>Success if the item was used.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_UseAbility == null) {
                return TaskStatus.Failure;
            }

            // Return a status of running for as long as the character is using the item.
            if (m_WaitForUse && m_UseAbility.IsActive) {
                for (int i = 0; i < m_UseAbility.UsableItems.Length; ++i) {
                    if (m_UseAbility.UsableItems[i] != null) {
                        return TaskStatus.Running;
                    }
                }
                m_WaitForUse = false;
                m_WaitForStopUse = true;
            }

            // The item has been used. Return success as soon as the ability is stopped.
            if (m_WaitForStopUse) {
                m_CharacterLocomotion.TryStopAbility(m_UseAbility);
                if (!m_UseAbility.IsActive) {
                    return TaskStatus.Success;
                }
                return TaskStatus.Running;
            }

            // The item should be used in the direction of the target.
            if (m_LocalLookSource != null) {
                if (m_AimTarget.Value != null) {
                    m_LocalLookSource.Target = m_AimTarget.Value.transform;
                } else {
                    m_LocalLookSource.Target = null;
                }
            }

            // The Use ability has been found - try to start or stop the ability.
            if (m_Start.Value) {
                var abilityStarted = m_CharacterLocomotion.TryStartAbility(m_UseAbility);
                // The ability should wait to end until the Use ability is complete.
                if (m_WaitForUseComplete.Value) {
                    m_WaitForUse = true;
                    return TaskStatus.Running;
                }
                return (abilityStarted || m_AlwaysReturnSuccess.Value) ? TaskStatus.Success : TaskStatus.Failure;
            } else {
                var abilityStopped = m_CharacterLocomotion.TryStopAbility(m_UseAbility, true);
                return (abilityStopped || m_AlwaysReturnSuccess.Value) ? TaskStatus.Success : TaskStatus.Failure;
            }
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_SlotID = -1;
            m_ActionID = 0;
            m_Start = true;
            m_AlwaysReturnSuccess = false;
            m_AimTarget = null;
        }
    }
}