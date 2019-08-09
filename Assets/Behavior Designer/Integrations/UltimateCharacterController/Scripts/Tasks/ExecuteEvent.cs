using UnityEngine;
using Opsive.UltimateCharacterController.Events;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Executes the event using the Ultimate Character Controller event system.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class ExecuteEvent : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The event name to send.")]
        public SharedString m_EventName;

        private GameObject m_PrevTarget;

        /// <summary>
        /// Executes the specified event.
        /// </summary>
        /// <returns>Success if the event was successfully executed.</returns>
        public override TaskStatus OnUpdate()
        {
            if (string.IsNullOrEmpty(m_EventName.Value)) {
                return TaskStatus.Failure;
            }

            EventHandler.ExecuteEvent(GetDefaultGameObject(m_TargetGameObject.Value), m_EventName.Value);
            return TaskStatus.Success;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_EventName = string.Empty;
        }
    }
}