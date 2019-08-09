/// ---------------------------------------------
/// Ultimate Character Controller
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

using UnityEngine;

namespace Opsive.UltimateCharacterController.Character.Abilities
{
    /// <summary>
    /// The AlignToGravity ability provides a base class for any abilities that want to change the character's up rotation.
    /// </summary>
    public abstract class AlignToGravity : Ability
    {
        [Tooltip("Specifies the speed that the character can rotate to align to the ground.")]
        [SerializeField] protected float m_RotationSpeed = 10;
        [Tooltip("The direction of gravit that should be set when the ability stops. Set to Vector3.zero to disable.")]
        [SerializeField] protected Vector3 m_StopGravityDirection = Vector3.zero;

        public float RotationSpeed { get { return m_RotationSpeed; } set { m_RotationSpeed = value; } }
        public Vector3 StopGravityDirection { get { return m_StopGravityDirection; } set { m_StopGravityDirection = value; } }

        public override bool Enabled { get { return base.Enabled; } set { m_Enabled = value; if (!m_Enabled && IsActive) { StopAbility(); } } }
        public override bool IsConcurrent { get { return true; } }
        public override bool CanStayActivatedOnDeath { get { return true; } }

        protected bool m_Stopping;

        /// <summary>
        /// The ability has started.
        /// </summary>
        protected override void AbilityStarted()
        {
            base.AbilityStarted();

            m_CharacterLocomotion.AlignToGravity = true;
            m_Stopping = false;
        }

        /// <summary>
        /// Stops the ability if it needs to be stopped.
        /// </summary>
        public override void Update()
        {
            base.Update();

            if (m_Stopping) {
                StopAbility();
            }
        }

        /// <summary>
        /// Rotates the character to be oriented with the specified normal.
        /// </summary>
        /// <param name="targetNormal">The direction that the character should be oriented towards on the vertical axis.</param>
        protected void Rotate(Vector3 targetNormal)
        {
            var deltaRotation = Quaternion.Euler(m_CharacterLocomotion.DeltaRotation);
            var rotation = m_Transform.rotation * deltaRotation;
            var proj = (rotation * Vector3.forward) - (Vector3.Dot((rotation * Vector3.forward), targetNormal)) * targetNormal;
            if (proj.sqrMagnitude > 0.0001f) {
                var alignToGroundSpeed = m_CharacterLocomotion.Platform == null && !m_Stopping ? m_RotationSpeed * m_CharacterLocomotion.TimeScale * Time.timeScale * m_CharacterLocomotion.DeltaTime : 1;
                var targetRotation = Quaternion.Slerp(rotation, Quaternion.LookRotation(proj, targetNormal), alignToGroundSpeed);
                var rotationDelta = deltaRotation * (Quaternion.Inverse(rotation) * targetRotation);
                m_CharacterLocomotion.DeltaRotation = rotationDelta.eulerAngles;
            }
        }

        /// <summary>
        /// The ability is trying to stop. Ensure the character ends at the correct orientation.
        /// </summary>
        public override void WillTryStopAbility()
        {
            base.WillTryStopAbility();

            if (m_StopGravityDirection.sqrMagnitude > 0) {
                m_CharacterLocomotion.GravityDirection = m_StopGravityDirection.normalized;
            }
            m_Stopping = true;
        }

        /// <summary>
        /// Can the ability be stopped?
        /// </summary>
        /// <returns>True if the ability can be stopped.</returns>
        public override bool CanStopAbility()
        {
            // Don't stop until the character is oriented in the correct direction.
            if (m_StopGravityDirection.sqrMagnitude == 0 || Vector3.Dot(m_CharacterLocomotion.Up, -m_StopGravityDirection) > 0.9999f) {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Resets the gravity direction and align to gravity to their stopping values.
        /// </summary>
        protected void ResetAlignToGravity()
        {
            if (m_StopGravityDirection != Vector3.zero) {
                m_CharacterLocomotion.GravityDirection = m_StopGravityDirection.normalized;
            }
            m_CharacterLocomotion.AlignToGravity = false;
        }
    }
}