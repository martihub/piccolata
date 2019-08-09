using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Tries to start or stop the ability.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class StartStopAbility2 : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The name of the ability to start or stop.")]
        public SharedString m_AbilityType;
        [Tooltip("The priority index can be used to specify which ability should be started or stopped if multiple abilities of the same type are found.")]
        public SharedInt m_PriorityIndex = -1;
        [Tooltip("Should the ability be started?")]
        public SharedBool m_Start = true;
        [Tooltip("Should the task always return success even if the ability doesn't start/stop successfully?")]
        public SharedBool m_AlwaysReturnSuccess;

        private GameObject m_PrevTarget;
        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private Ability m_Ability;

        /// <summary>
        /// Retrieves the specified ability.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget)
            {
                m_CharacterLocomotion = target.GetCachedComponent<UltimateCharacterLocomotion>();
                // Find the specified ability.
                string m_AbilityTypeTemp = m_AbilityType.Value;
                m_AbilityType.Value = "Opsive.UltimateCharacterController.Character.Abilities." + m_AbilityTypeTemp;
                var abilities = m_CharacterLocomotion.GetAbilities(TaskUtility.GetTypeWithinAssembly(m_AbilityType.Value));
                if (abilities.Length > 1)
                {
                    // If there are multiple abilities found then the priority index should be used, otherwise set the ability to the first value.
                    if (m_PriorityIndex.Value != -1)
                    {
                        for (int i = 0; i < abilities.Length; ++i)
                        {
                            if (abilities[i].Index == m_PriorityIndex.Value)
                            {
                                m_Ability = abilities[i];

                                break;
                            }
                        }
                    }
                    else
                    {
                        m_Ability = abilities[0];

                    }
                }
                else if (abilities.Length == 1)
                {
                    m_Ability = abilities[0];
                }
                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Tries to start or stop the specified ability.
        /// </summary>
        /// <returns>Success if the ability was started or stopped.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Ability == null)
            {
                return TaskStatus.Failure;
            }

            // The ability is not null - try to start or stop the ability.
            if (m_Start.Value)
            {
                var abilityStarted = m_CharacterLocomotion.TryStartAbility(m_Ability);
                return (abilityStarted || m_AlwaysReturnSuccess.Value) ? TaskStatus.Success : TaskStatus.Failure;
            }
            else
            {
                var abilityStopped = m_CharacterLocomotion.TryStopAbility(m_Ability);
                return (abilityStopped || m_AlwaysReturnSuccess.Value) ? TaskStatus.Success : TaskStatus.Failure;
            }
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_AbilityType = string.Empty;
            m_PriorityIndex = -1;
            m_Start = true;
            m_AlwaysReturnSuccess = false;
        }
    }
}