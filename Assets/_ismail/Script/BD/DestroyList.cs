using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class DestroyList : Action
{
    public GameObject[] list;
    public string tag;

    public override void OnStart()
    {
        list = GameObject.FindGameObjectsWithTag(tag);
    }

    public override TaskStatus OnUpdate()
    {
        foreach (var item in list)
        {
            if (item) GameObject.Destroy(item);
        }
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        list = null;
    }
}