using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{
    public class CompareMatchParts : Conditional
    {
        public SharedGameObjectList list;
        public override TaskStatus OnUpdate()
        {
            if (list.Value.Count == 2)
            {
                int a = list.Value[0].GetComponent<MatchPart>().ID;
                int b = list.Value[1].GetComponent<MatchPart>().ID;
                return a == b ? TaskStatus.Success : TaskStatus.Failure;
            }
            return TaskStatus.Failure;
        }
    }
}
