using UnityEngine;
using Opsive.UltimateCharacterController.Events;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskDescription("Returns success when the agent takes damage.")]
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    [HelpURL("https://www.opsive.com/support/documentation/behavior-designer/integrations/opsive-character-controllers/")]
    public class HasTakenDamage : Conditional
    {
        [Tooltip("A reference to the agent. If null it will be retrieved from the current GameObject.")]
        public SharedGameObject m_TargetGameObject;
        [Tooltip("The GameObject that caused the damage.")]
        public SharedGameObject m_Attacker;

        private GameObject m_PrevTarget;
        private int m_DamageFrame = -1;
        private GameObject m_Originator;

        /// <summary>
        /// Retrieves the health component.
        /// </summary>
        public override void OnStart()
        {
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            // If the targets aren't equal then the character hasn't been set or the target has switched.
            if (target != m_PrevTarget) {
                if (m_PrevTarget != null) {
                    EventHandler.UnregisterEvent<float, Vector3, Vector3, GameObject, Collider>(m_PrevTarget, "OnHealthDamage", OnDamage);
                    EventHandler.UnregisterEvent<Vector3, Vector3, GameObject>(m_PrevTarget, "OnDeath", OnDeath);
                }

                if (target != null) {
                    EventHandler.RegisterEvent<float, Vector3, Vector3, GameObject, Collider>(target, "OnHealthDamage", OnDamage);
                    EventHandler.RegisterEvent<Vector3, Vector3, GameObject>(target, "OnDeath", OnDeath);
                }

                m_PrevTarget = target;
            }
        }

        /// <summary>
        /// Returns succes if the agent has taken damage.
        /// </summary>
        /// <returns>Success if the agent has taken damage.</returns>
        public override TaskStatus OnUpdate()
        {
            if (m_DamageFrame >= Time.frameCount - 1) {
                if (m_Attacker.IsShared) {
                    m_Attacker.Value = m_Originator;
                }
                return TaskStatus.Success;
            }
            return TaskStatus.Failure;
        }

        /// <summary>
        /// The task has ended - reset the damage variable.
        /// </summary>
        public override void OnEnd()
        {
            m_Originator = null;
            m_DamageFrame = -1;
        }

        /// <summary>
        /// The object has taken damage.
        /// </summary>
        /// <param name="amount">The amount of damage taken.</param>
        /// <param name="position">The position of the damage.</param>
        /// <param name="force">The amount of force applied to the object while taking the damage.</param>
        /// <param name="attacker">The GameObject that did the damage.</param>
        /// <param name="hitCollider">The Collider that was hit.</param>
        private void OnDamage(float amount, Vector3 position, Vector3 force, GameObject attacker, Collider hitCollider)
        {
            m_DamageFrame = Time.frameCount;
            m_Originator = attacker;
        }

        /// <summary>
        /// The object has died.
        /// </summary>
        /// <param name="position">The position of the force.</param>
        /// <param name="force">The amount of force which killed the character.</param>
        /// <param name="attacker">The GameObject that killed the character.</param>
        private void OnDeath(Vector3 position, Vector3 force, GameObject attacker)
        {
            m_DamageFrame = -1;
        }

        /// <summary>
        /// Resets the objects back to their default values.
        /// </summary>
        public override void OnReset()
        {
            m_TargetGameObject = null;
            m_Attacker = null;
        }
    }
}