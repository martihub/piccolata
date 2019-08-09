using UnityEngine;
using Opsive.UltimateCharacterController.Inventory;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Gets the amount of the specified ItemType that the agent has.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class GetItemTypeCount : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The ItemType to get the count of.")]
        public SharedItemType m_ItemType;
        [Tooltip("The amount of the ItemType that the agent has.")]
        [SharedRequired] public SharedFloat m_StoreResult;

        private GameObject m_PrevTarget;
        private InventoryBase m_Inventory;

        /// <summary>
        /// Retrieves the inventory component.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_Inventory = target.GetCachedComponent<InventoryBase>();
                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Sets the value of the ItemType count.
        /// </summary>
        /// <returns>Success if the value was successfully set.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Inventory == null || m_ItemType.Value == null) {
                return TaskStatus.Failure;
            }

            m_StoreResult.Value = m_Inventory.GetItemTypeCount(m_ItemType.Value);
            return TaskStatus.Success;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_ItemType = null;
            m_StoreResult = null;
        }
    }
}