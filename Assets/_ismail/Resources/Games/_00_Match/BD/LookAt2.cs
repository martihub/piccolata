using UnityEngine;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.Utility;
using Opsive.UltimateCharacterController.Character;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform
{
    [TaskCategory("Ismail")]
    [TaskIcon("Assets/_ismail/_Etc/Icon/LookAtIcon.png")]
    public class LookAt2 : Action
    {
        //public SharedGameObject LookAtTarget;
        //float m_MaxRotationAngle = 90;
        UltimateCharacterLocomotion m_CharacterLocomotion;

        public SharedGameObject targetGameObject;
        public SharedVector3 to;
        public SharedFloat time = 1;
        public AxisConstraint axisConstraint = AxisConstraint.Y;
        public SharedVector3 up = Vector3.up;
        public SharedGameObject target;


        //public override void OnStart()
        //{
        //    m_CharacterLocomotion = GetComponent<UltimateCharacterLocomotion>();
        //}

        public override void OnStart()
        {
            base.OnStart();
            // var target = GetDefaultGameObject(targetGameObject.Value).transform;
            m_CharacterLocomotion = GetComponent<UltimateCharacterLocomotion>();
        }

        public override TaskStatus OnUpdate()
        {
            //Vector3 relativePos = LookAtTarget.Value.transform.position - transform.position;
            //Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);

            m_CharacterLocomotion.transform.DOLookAt(target.Value.transform.position, time.Value, axisConstraint, up.Value);



            // Vector3 v2 = new Vector3(rotation.eulerAngles.x, rotation.eulerAngles.y, rotation.eulerAngles.z);
            //Quaternion Q2 = Quaternion.Euler(0, v2.y, v2.z);
            //  m_CharacterLocomotion.SetRotation(Q2);
            return TaskStatus.Success;
            //return TaskStatus.Running;
        }

        public override void OnReset()
        {
            targetGameObject = null;
            to = null;
            time = null;
            target = null;

        }
    }
}