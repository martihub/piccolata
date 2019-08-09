using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Is the specified ability active?")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class IsAbilityActive : Conditional
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The name of the ability.")]
        [AbilityDrawer] public SharedString m_AbilityType;
        [Tooltip("The priority index can be used to specify which ability should be stopped if multiple abilities types are found.")]
        public SharedInt m_PriorityIndex = -1;

        private GameObject m_PrevTarget;
        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private Ability m_Ability;

        /// <summary>
        /// Retrieves the specified ability.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_CharacterLocomotion = target.GetCachedComponent<UltimateCharacterLocomotion>();
                // Find the specified ability.
                var abilities = m_CharacterLocomotion.GetAbilities(TaskUtility.GetTypeWithinAssembly(m_AbilityType.Value));
                if (abilities.Length > 1) {
                    // If there are multiple abilities found then the priority index should be used, otherwise set the ability to the first value.
                    if (m_PriorityIndex.Value != -1) {
                        for (int i = 0; i < abilities.Length; ++i) {
                            if (abilities[i].Index == m_PriorityIndex.Value) {
                                m_Ability = abilities[i];
                                break;
                            }
                        }
                    } else {
                        m_Ability = abilities[0];
                    }
                } else if (abilities.Length == 1) {
                    m_Ability = abilities[0];
                }
                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Returns succes if the ability is active.
        /// </summary>
        /// <returns>Success if the ability is active.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Ability == null) {
                return TaskStatus.Failure;
            }

            // The ability is not null - is the ability active?
            return m_Ability.IsActive ? TaskStatus.Success : TaskStatus.Failure;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_AbilityType = string.Empty;
            m_PriorityIndex = -1;
        }
    }
}