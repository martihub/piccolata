using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorDesigner.Runtime.Tasks
{
    public class TTSTalk : Action
    {
        public TalkType talkType;
        public string speech;
        public SharedFloat speechDuration;

        public override void OnStart()
        {
            speech = MatchExtras.instance.GetSpeech(talkType);
            int count = speech.Split(' ').Length;
            speechDuration.Value = count / 2;   //Konuşma süresinin toplam kelimelerin yarısı oluğunu sizden öğrenmiştim.
            GetComponent<PlayerExtras>().PlayTTS(speech);
        }
    }
}

