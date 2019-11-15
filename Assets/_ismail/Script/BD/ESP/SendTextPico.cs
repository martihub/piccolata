using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    public class SendTextPico : Action
    {

        public SharedString speech;
        public SharedFloat speechDuration;

        public override void OnStart()
        {
            int count = speech.Value.Split(' ').Length;
            speechDuration.Value = count / 2;
            GetComponent<PlayerExtras>().PlayTTS(speech.Value);
        }

    }
}

