using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class EnableDisableIsClicked : Action
    {
        public SharedGameObjectList list;
        public SharedBool isTrue;
        public override TaskStatus OnUpdate()
        {
            foreach (var item in list.Value)
            {
                item.GetComponent<MatchPart>().isClicked = isTrue.Value;
            }

            return TaskStatus.Success;
        }

    }
}

