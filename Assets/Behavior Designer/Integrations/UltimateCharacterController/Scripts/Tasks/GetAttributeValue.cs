using UnityEngine;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Stores the attribute value.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class GetAttributeValue : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The name of the attribute.")]
        public SharedString m_AttributeName = "Health";
        [Tooltip("The location to store the value of.")]
        [SharedRequired] public SharedFloat m_StoreResult;

        private GameObject m_PrevTarget;
        private string m_PrevAttributeName;
        private AttributeManager m_AttributeManager;
        private Attribute m_Attribute;

        /// <summary>
        /// Retrieves the health component.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_AttributeManager = target.GetCachedComponent<AttributeManager>();
                m_PrevTarget = target;
            }

            if (m_AttributeManager != null && m_AttributeName.Value != m_PrevAttributeName) {
                m_Attribute = m_AttributeManager.GetAttribute(m_AttributeName.Value);
                m_PrevAttributeName = m_AttributeName.Value;
            }
        }

        /// <summary>
        /// Sets the value of the attribute with the specified name.
        /// </summary>
        /// <returns>Success if the value was successfully set.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_AttributeManager == null || m_Attribute == null) {
                return TaskStatus.Failure;
            }

            m_StoreResult.Value = m_Attribute.Value;
            return TaskStatus.Success;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_AttributeName = "Health";
            m_StoreResult = null;
        }
    }
}