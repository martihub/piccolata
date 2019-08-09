using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Kills a tween or all tweens if no tween is specified.")]
    [TaskIcon("Assets/Behavior Designer/Integrations/DOTween/Editor/Icon.png")]
    public class Kill : Action
    {
        [RequiredField]
        [Tooltip("The optional tweener to kill. If null all tweens will be killed")]
        public SharedTweener targetTweener;

        public override TaskStatus OnUpdate()
        {
            if (targetTweener != null && targetTweener.Value != null) {
                targetTweener.Value.Kill();
            } else {
                DG.Tweening.DOTween.Kill(targetTweener.Value);
            }
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            targetTweener = null;
        }
    }
}