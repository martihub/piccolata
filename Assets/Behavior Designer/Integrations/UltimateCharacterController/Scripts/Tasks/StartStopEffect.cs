using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Effects;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Tries to start or stop the effect.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class StartStopEffect : Action
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The name of the effect to start or stop.")]
        [EffectDrawer] public SharedString m_EffectType;
        [Tooltip("Should the effect be started?")]
        public SharedBool m_Start = true;
        [Tooltip("Should the task always return success even if the effect is started/stopped successfully?")]
        public SharedBool m_AlwaysReturnSuccess;

        private GameObject m_PrevTarget;
        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private Effect m_Effect;

        /// <summary>
        /// Retrieves the specified effect.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget) {
                m_CharacterLocomotion = target.GetCachedComponent<UltimateCharacterLocomotion>();
                m_Effect = m_CharacterLocomotion.GetEffect(TaskUtility.GetTypeWithinAssembly(m_EffectType.Value));
                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Tries to start or stop the specified effect.
        /// </summary>
        /// <returns>Success if the effect was started or stopped.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Effect == null) {
                return TaskStatus.Failure;
            }

            // The effect is not null - try to start or stop the effect.
            if (m_Start.Value) {
                var effectStarted = m_CharacterLocomotion.TryStartEffect(m_Effect);
                return (effectStarted || m_AlwaysReturnSuccess.Value) ? TaskStatus.Success : TaskStatus.Failure;
            } else {
                var effectStopped = m_CharacterLocomotion.TryStopEffect(m_Effect);
                return (effectStopped || m_AlwaysReturnSuccess.Value) ? TaskStatus.Success : TaskStatus.Failure;
            }
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_EffectType = string.Empty;
            m_Start = true;
            m_AlwaysReturnSuccess = true;
        }
    }
}