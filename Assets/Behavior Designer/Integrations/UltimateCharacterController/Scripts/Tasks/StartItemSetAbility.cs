using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities.Items;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Tries to start the ItemSet ability.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class StartItemSetAbility : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The name of the ItemSet ability.")]
        [ItemSetAbilityDrawer] public SharedString m_AbilityType;
        [Tooltip("The category that the ability should respond to.")]
        [ItemSetCategoryDrawer] public SharedInt m_CategoryID;

        private GameObject m_PrevTarget;
        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private ItemSetAbilityBase m_ItemSetAbility;

        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_CharacterLocomotion = target.GetCachedComponent<UltimateCharacterLocomotion>();
                // Find the specified ability.
                var abilities = m_CharacterLocomotion.GetAbilities<ItemSetAbilityBase>();
                // The category ID must match.
                for (int i = 0; i < abilities.Length; ++i) {
                    if (abilities[i].ItemSetCategoryID == m_CategoryID.Value) {
                        m_ItemSetAbility = abilities[i];
                        break;
                    }
                }
                if (m_ItemSetAbility == null) {
                    Debug.LogWarning("Error: Unable to find an ItemSet ability with category ID " + m_CategoryID.Value + ".");
                    return;
                }
                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Tries to start the ItemSet ability.
        /// </summary>
        /// <returns>Success if the ability was started.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_ItemSetAbility == null) {
                return TaskStatus.Failure;
            }

            // The ability is not null - try to start the ability.
            return m_CharacterLocomotion.TryStartAbility(m_ItemSetAbility) ? TaskStatus.Success : TaskStatus.Failure;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_AbilityType = string.Empty;
            m_CategoryID = 0;
        }
    }
}