
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    public class SetSpeechText : Action
    {
        public string[] speech;
        public SharedString selectedText;
        public SharedInt a;

        //public override void OnStart()
        //{
        //    speech = MatchExtras.instance.GetSpeech(talkType);
        //    int count = speech.Split(' ').Length;
        //    speechDuration.Value = count / 2;
        //    GetComponent<PlayerExtras>().PlayTTS(speech);
        //}

        public override void OnStart()
        {
            //  selectedText.Value = speech[a];

            GetComponent<BehaviorTree>().SetVariableValue("selectedText", speech[a.Value]);
            a.Value++;
            if (a.Value == speech.Length)
            {
                a.Value = 0;
            }

        }

    }
}

