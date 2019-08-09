using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    public class TTSTalk : Action
    {
        public SharedString speech;
        AudioSource _audio;

        public override void OnStart()
        {
            GetComponent<PlayerExtras>().PlayTTS(speech.Value);
        }


    }
}

