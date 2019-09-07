using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class SlotIsOkControl : Conditional
{
    public SharedGameObject clickedSlot;

    public override TaskStatus OnUpdate()
    {
        var slot = clickedSlot.Value.GetComponent<MatchPart>();
        bool bl = slot.IsClickable();
        if (bl)
        {
            slot.isClicked = true;
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }
}