using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class ScaleSlot : Action
    {
        public SharedGameObjectList list;

        public override TaskStatus OnUpdate()
        {
            foreach (var item in list.Value)
            {
                item.GetComponent<MatchPart>().ScaleSprite();
            }
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            base.OnReset();
        }
    }
}
