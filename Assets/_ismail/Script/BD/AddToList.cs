using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class AddToList : Action
    {
        public SharedGameObject item;
        public SharedGameObjectList list;

        public override void OnStart()
        {
            list.Value.Add(item.Value);
        }

        public override TaskStatus OnUpdate()
        {
            return TaskStatus.Success;
        }

        public override void OnReset()
        {
            item = null;
            list = null;
        }

    }
}


