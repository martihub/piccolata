using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Pauses a tween or all tweens if no tween is specified.")]
    [TaskIcon("Assets/Behavior Designer/Integrations/DOTween/Editor/Icon.png")]
    public class Pause : Action
    {
        [RequiredField]
        [Tooltip("The optional tweener to pause. If null all tweens will be paused")]
        public SharedTweener targetTweener;

        public override TaskStatus OnUpdate()
        {
            if (targetTweener != null && targetTweener.Value != null) {
                targetTweener.Value.Pause();
            } else {
                DG.Tweening.DOTween.Pause(targetTweener.Value);
            }
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            targetTweener = null;
        }
    }
}