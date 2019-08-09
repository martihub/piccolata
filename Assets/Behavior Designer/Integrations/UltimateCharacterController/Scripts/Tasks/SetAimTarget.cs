using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Sets the target of the LookSource.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class SetAimTarget : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The GameObject that the character should aim at.")]
        public SharedGameObject m_AimTarget;

        private GameObject m_PrevTarget;
        private LocalLookSource m_LocalLookSource;

        /// <summary>
        /// Retrieves the local look source.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_LocalLookSource = target.GetCachedComponent<LocalLookSource>();
                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Tries to stop the ability specified within AbilityActionBase.
        /// </summary>
        /// <returns>Success if the ability was stopped.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_LocalLookSource == null) {
                return TaskStatus.Failure;
            }

            // The look source exists - set the target.
            if (m_AimTarget.Value != null) {
                m_LocalLookSource.Target = m_AimTarget.Value.transform;
            } else {
                m_LocalLookSource.Target = null;
            }
            return TaskStatus.Success;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_AimTarget = null;
        }
    }
}