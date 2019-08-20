using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class denenme : MonoBehaviour
{
    BehaviorTree behaviorTree;
    void Start()
    {
        behaviorTree.SetVariableValue("", 34);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
