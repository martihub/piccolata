using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class ClearList : Action
    {
        public SharedGameObjectList list;

        public override TaskStatus OnUpdate()
        {
            list.Value.Clear();
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            list = null;
        }
    }

}
