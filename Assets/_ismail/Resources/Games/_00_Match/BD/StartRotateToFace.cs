using UnityEngine;
using Opsive.UltimateCharacterController.Character;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.Utility;
using System.Collections;

namespace BehaviorDesigner.Runtime.Tasks.UltimateCharacterController
{
    [TaskCategory("Ultimate Character Controller")]
    [TaskIcon("Assets/Behavior Designer/Integrations/UltimateCharacterController/Editor/Icon.png")]
    public class StartRotateToFace : Action
    {
        [HideInInspector] public SharedGameObject m_TargetGameObject;
        SharedString m_AbilityType = "RotateToFace";
        [HideInInspector] public SharedInt m_PriorityIndex = -1;

        [SharedRequired]
        public SharedGameObject targetGO;
        public SharedBool isAbilityStoped;
        public SharedFloat rotSpeed = 2;


        private GameObject m_PrevTarget;
        private UltimateCharacterLocomotion m_CharacterLocomotion;
        private Ability m_Ability;

        float startTime;
        bool isCompled;

        public override void OnStart()
        {
            startTime = Time.time;
            var target = GetDefaultGameObject(m_TargetGameObject.Value);
            if (target != m_PrevTarget)
            {
                m_CharacterLocomotion = target.GetCachedComponent<UltimateCharacterLocomotion>();
                // Find the specified ability.
                var abilities = m_CharacterLocomotion.GetAbilities(TaskUtility.GetTypeWithinAssembly(m_AbilityType.Value));
                if (abilities.Length > 1)
                {
                    // If there are multiple abilities found then the priority index should be used, otherwise set the ability to the first value.
                    if (m_PriorityIndex.Value != -1)
                    {
                        for (int i = 0; i < abilities.Length; ++i)
                        {
                            if (abilities[i].Index == m_PriorityIndex.Value)
                            {
                                m_Ability = abilities[i];
                                break;
                            }
                        }
                    }
                    else
                    {
                        m_Ability = abilities[0];
                    }
                }
                else if (abilities.Length == 1)
                {
                    m_Ability = abilities[0];
                }
                m_PrevTarget = target;

                if (m_Ability is IPcAbility)
                {
                    if (targetGO != null) (m_Ability as IPcAbility).SetArgument1(targetGO.GetValue());
                    if (rotSpeed != null) (m_Ability as IPcAbility).SetArgument2(rotSpeed.GetValue());
                }
            }
            m_CharacterLocomotion.TryStartAbility(m_Ability);
        }

        public override TaskStatus OnUpdate()
        {
            if (isAbilityStoped.Value)
            {
                isAbilityStoped.Value = false;
                m_CharacterLocomotion.TryStopAbility(m_Ability);
                return TaskStatus.Success;
            }
            return TaskStatus.Running;
        }

        public override void OnReset()
        {
            isAbilityStoped = false;
            m_TargetGameObject = null;
            m_AbilityType = string.Empty;
            m_PriorityIndex = -1;
            targetGO = null;
        }
    }
}