using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Sets the delay of the tween.")]
    [TaskIcon("Assets/Behavior Designer/Integrations/DOTween/Editor/Icon.png")]
    public class SetDelay : Action
    {
        [RequiredField]
        [Tooltip("The tweener to set the ease of")]
        public SharedTweener targetTweener;
        [Tooltip("The delay amount")]
        public SharedFloat delay;

        public override TaskStatus OnUpdate()
        {
            targetTweener.Value.SetDelay(delay.Value);
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            targetTweener = null;
            delay = 0;
        }
    }
}