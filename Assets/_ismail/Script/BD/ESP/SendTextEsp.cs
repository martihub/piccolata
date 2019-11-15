using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks
{

    public class SendTextEsp : Action
    {
        public SharedString selectedText;
        public override void OnStart()
        {
            GetComponent<EspMono>().Gonder(selectedText.Value);
        }

    }
}

