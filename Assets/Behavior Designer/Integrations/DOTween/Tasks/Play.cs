using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Plays a tween or all tweens if no tween is specified.")]
    [TaskIcon("Assets/Behavior Designer/Integrations/DOTween/Editor/Icon.png")]
    public class Play : Action
    {
        [RequiredField]
        [Tooltip("The optional tweener to play. If null all tweens will be played")]
        public SharedTweener targetTweener;

        public override TaskStatus OnUpdate()
        {
            if (targetTweener != null && targetTweener.Value != null) {
                targetTweener.Value.Play();
            } else {
                DG.Tweening.DOTween.Play(targetTweener.Value);
            }
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            targetTweener = null;
        }
    }
}