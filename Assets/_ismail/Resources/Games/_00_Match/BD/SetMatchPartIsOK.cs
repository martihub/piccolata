using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class SetMatchPartIsOK : Action
    {
        public SharedGameObjectList list;

        public override TaskStatus OnUpdate()
        {
            foreach (var item in list.Value)
            {
                item.GetComponent<MatchPart>().isOK = true;
            }
            return TaskStatus.Success;
        }


    }
}

