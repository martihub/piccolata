using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Sets the ease of the tween.")]
    [TaskIcon("Assets/Behavior Designer/Integrations/DOTween/Editor/Icon.png")]
    public class SetEase : Action
    {
        [RequiredField]
        [Tooltip("The tweener to set the ease of")]
        public SharedTweener targetTweener;
        [Tooltip("The ease type")]
        public Ease ease = Ease.InOutQuint;

        public override TaskStatus OnUpdate()
        {
            targetTweener.Value.SetEase(ease);
            return TaskStatus.Success;
        }
        public override void OnReset()
        {
            targetTweener = null;
            ease = Ease.InOutQuint;
        }
    }
}