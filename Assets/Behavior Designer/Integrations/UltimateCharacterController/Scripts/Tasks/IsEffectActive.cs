using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Effects;
using Opsive.UltimateCharacterController.Utility;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Is the specified effect active?")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class IsEffectActive : Conditional
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The name of the effect.")]
        [EffectDrawer] public SharedString m_EffectType;

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
        /// Returns succes if the effect is active.
        /// </summary>
        /// <returns>Success if the effect is active.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_Effect == null) {
                return TaskStatus.Failure;
            }

            // The effect is not null - is the effect active?
            return m_Effect.IsActive ? TaskStatus.Success : TaskStatus.Failure;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_EffectType = string.Empty;
        }
    }
}