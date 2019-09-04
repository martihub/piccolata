using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class ManageGoogleWords : MonoBehaviour
{


    public GameType gameType;
    public static ManageGoogleWords instance;
    public int[] ints = new int[2] { -1, -1 };
    string str;
    //AudioSource audioSource;

    void Awake()
    {
        if (!instance) instance = this;
        // audioSource = GetComponent<AudioSource>();
    }

    public void SetWords(string _str)
    {
        str = _str;
        Invoke(gameType.ToString(), 0);
    }

    int count = 0;
    private bool isLongWait;
    public void _00_Match()
    {
        foreach (var item in str)
        {
            if (char.IsNumber(item) && count < 2)
            {
                int a = item.ToInt();
                ints[count] = a;
                count++;
                if (count >= 2)
                {
                    count = 0;
                }
            }
        }
        StartCoroutine(_00_MatchIE());
    }
    int a;
    IEnumerator _00_MatchIE()
    {

        if (ints[0] != -1 && ints[1] != -1)//İkisi aynı anda geldi demek ki...
        {
            a = ints[0] - 1;
            GameObject clickedSlot = MatchCreator.instance.allSlots[a];
            GetComponent<BehaviorTree>().SendEvent<object>("SlotClick", clickedSlot);
            yield return new WaitForSeconds(1);
            a = ints[1] - 1;
            clickedSlot = MatchCreator.instance.allSlots[a];
            GetComponent<BehaviorTree>().SendEvent<object>("SlotClick", clickedSlot);
            for (int i = 0; i < ints.Length; i++) ints[i] = -1;
        }

        else if (ints[0] != -1)
        {
            a = ints[0] - 1;
            GameObject clickedSlot = MatchCreator.instance.allSlots[a];
            GetComponent<BehaviorTree>().SendEvent<object>("SlotClick", clickedSlot);
            ints[0] = -1;
        }

        else if (ints[1] != -1)
        {
            a = ints[1] - 1;
            GameObject clickedSlot = MatchCreator.instance.allSlots[a];
            GetComponent<BehaviorTree>().SendEvent<object>("SlotClick", clickedSlot);
            ints[1] = -1;
        }

    }
}
