using UnityEngine;
using DG.Tweening;

namespace BehaviorDesigner.Runtime.Tasks.DOTween
{
    [TaskCategory("DOTween")]
    [TaskDescription("Tween any particular Vector2 value.")]
    [TaskIcon("Assets/Behavior Designer/Integrations/DOTween/Editor/Icon.png")]
    public class Vector2To : Action
    {
        [Tooltip("The original tween value")]
        public SharedVector2 from;
        [Tooltip("The target tween value")]
        public SharedVector2 to;
        [Tooltip("The time the tween takes to complete")]
        public SharedFloat time;
        [SharedRequired]
        [Tooltip("The stored tweener")]
        public SharedTweener storeTweener;

        private bool complete;

        public override void OnStart()
        {
            storeTweener.Value = DG.Tweening.DOTween.To(() => from.Value, x => from.Value = x, to.Value, time.Value);
            storeTweener.Value.OnComplete(() => complete = true);
        }

        public override TaskStatus OnUpdate()
        {
            return complete ? TaskStatus.Success : TaskStatus.Running;
        }

        public override void OnEnd()
        {
            complete = false;
        }

        public override void OnReset()
        {
            from = Vector2.zero;
            to = Vector2.zero;
            time = 0;
            storeTweener = null;
        }
    }
}