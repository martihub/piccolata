using UnityEngine;
using Opsive.UltimateCharacterController.Traits;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Is the agent alive?")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class IsAlive : Conditional
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;

        private GameObject m_PrevTarget;
        private Health m_Health;

        /// <summary>
        /// Retrieves the health component.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_Health = target.GetCachedParentComponent<Health>();
                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Returns succes if the agent is alive.
        /// </summary>
        /// <returns>Success if the agent is alive.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Health == null) {
                return TaskStatus.Failure;
            }

            return m_Health.IsAlive() ? TaskStatus.Success : TaskStatus.Failure;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
        }
    }
}