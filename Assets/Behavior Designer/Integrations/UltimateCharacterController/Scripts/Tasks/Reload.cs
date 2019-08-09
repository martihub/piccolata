#if ULTIMATE_CHARACTER_CONTROLLER_SHOOTER
using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Tries to reload the current item.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class Reload : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The SlotID of the item that should be reloaded.")]
        public SharedInt m_SlotID = -1;
        [Tooltip("The ActionID of the item that should be reloaded.")]
        public SharedInt m_ActionID;

        private Opsive.UltimateCharacterController.Character.Abilities.Items.Reload m_ReloadAbility;

        private GameObject m_PrevTarget;
        private UltimateCharacterLocomotion m_CharacterLocomotion;

        /// <summary>
        /// Retrieves the use ability.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_CharacterLocomotion = target.GetCachedComponent<UltimateCharacterLocomotion>();
                // Find the specified ability.
                var abilities = m_CharacterLocomotion.GetAbilities<Opsive.UltimateCharacterController.Character.Abilities.Items.Reload>();
                // The slot ID and action ID must match.
                for (int i = 0; i < abilities.Length; ++i) {
                    if (abilities[i].SlotID == m_SlotID.Value && abilities[i].ActionID == m_ActionID.Value) {
                        m_ReloadAbility = abilities[i];
                        break;
                    }
                }
                if (m_ReloadAbility == null) {
                    Debug.LogWarning("Error: Unable to find a Reload ability with slot " + m_SlotID.Value + " and action " + m_ActionID.Value);
                    return;
                }
                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Tries to reload the current item.
        /// </summary>
        /// <returns>Success if the item was used.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_ReloadAbility == null) {
                return TaskStatus.Failure;
            }

            // The Reload ability has been found - try to use the ability.
            return m_CharacterLocomotion.TryStartAbility(m_ReloadAbility) ? TaskStatus.Success : TaskStatus.Failure;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_SlotID = -1;
            m_ActionID = 0;
        }
    }
}
#endif