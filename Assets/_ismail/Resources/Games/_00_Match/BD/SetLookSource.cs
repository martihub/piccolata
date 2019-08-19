using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Opsive.UltimateCharacterController.Character;
using DG.Tweening;
using BehaviorDesigner.Runtime.Tasks.DOTween;

public class SetLookSource : Action
{

    public SharedGameObject lookAtGO;
    public bool start;


    float lookWeight, to;

    LocalLookSource lookSource;
    CharacterIK characterIK;
    SharedTweener storeTweener;

    private bool complete;

    public override void OnStart()
    {
        lookWeight = start ? 0 : 1;
        to = start ? 1 : 0;

        lookSource = GetComponent<LocalLookSource>();
        characterIK = GetComponent<CharacterIK>();
        lookSource.Target = lookAtGO.Value.transform;
        storeTweener.Value = DOTween.To(() => lookWeight, x => lookWeight = x, to, 1);
        storeTweener.Value.OnComplete(() => complete = true);
    }

    public override TaskStatus OnUpdate()
    {
        characterIK.LookAtHeadWeight = lookWeight / 2;
        characterIK.LookAtBodyWeight = lookWeight / 4;
        return complete ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        complete = false;
    }
}