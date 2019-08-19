
using UnityEngine;
using Opsive.UltimateCharacterController.Character.Abilities;
using Opsive.UltimateCharacterController.Utility;
using BehaviorDesigner.Runtime;
using Opsive.UltimateCharacterController.Character;

public class RotateToFace : PcBase, IPcAbility
{

    public GameObject targetGO;
    public float rotSpeed;

    protected override void AbilityStarted()
    {

        base.AbilityStarted();
    }

    public override void ApplyRotation()
    {
        var agentPos = m_GameObject.transform.position; // newcode line 1
        var lookDirection = targetGO.transform.position - agentPos; // newcode line 2
        var localLookDirection = m_Transform.InverseTransformDirection(lookDirection);
        localLookDirection.y = 0;
        var yRot = MathUtility.ClampInnerAngle(Quaternion.LookRotation(localLookDirection.normalized, m_CharacterLocomotion.Up).eulerAngles.y);
        int posNeg = yRot > 0 ? 1 : -1;
        if (Mathf.Abs(yRot) > 3)
        {
            m_CharacterLocomotion.Torque = Quaternion.Euler(0, posNeg * rotSpeed, 0);
        }
        else
        {
            GetComponent<BehaviorTree>().SetVariableValue("isAbilityStoped", true);
            //if (GetComponent<LocalLookSource>())
            //{
            //    GetComponent<LocalLookSource>().Target = null;
            //}
            StopAbility();
        }
    }

    public void SetArgument1(object argument1)
    {
        targetGO = (GameObject)argument1;
    }

    public void SetArgument2(object argument2)
    {
        rotSpeed = (float)argument2;
    }


}
