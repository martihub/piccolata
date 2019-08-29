using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;
public class MatchManager : MonoBehaviour
{
    public GameType gameType;
    public GameObject panel;
    public Image image;
    string gamesFolder = "Games";
    List<GameObject> slots = new List<GameObject>();
    List<GameObject> _slots;

    DirectoryInfo directoryInfo;
    FileInfo[] allFiles;

    public void GenerateSlots(int _count)
    {
        Debug.Log(_count);
        for (int i = 0; i < _count * 2; i++)
        {
            GameObject slot = Instantiate(Resources.Load<GameObject>(gamesFolder + "/" + gameType.ToString() + "/" + "Slot"));
            slot.transform.parent = panel.transform;
            slots.Add(slot);
        }
        _slots = new List<GameObject>(slots);
        for (int i = 0; i < _count; i++)
        {
            for (int j = 0; j < 2; j++)
            {
                int a = Random.Range(0, _slots.Count);
                _slots[a].GetComponent<MatchPart>().ID = i;
                _slots.Remove(_slots[a]);
            }

        }

        GetComponent<BehaviorTree>().SetVariableValue("Slots", slots);
        List<GameObject> allSlots = new List<GameObject>(slots);
        GetComponent<BehaviorTree>().SetVariableValue("AllSlots", allSlots);

        directoryInfo = new DirectoryInfo(Application.streamingAssetsPath + "/" + gamesFolder + "/" + gameType.ToString() + "/00");
        allFiles = directoryInfo.GetFiles("*.png");
        ImageCoroutine(_count);
    }

    void ImageCoroutine(int _count)
    {
        for (int i = 0; i < _count * 2; i++)
        {
            int a = slots[i].GetComponent<MatchPart>().ID;
            StartCoroutine(SpriteLoader.LoadSpriteIE(slots[i].GetComponent<Image>(), allFiles[a].ToString()));
        }
    }
}

public enum GameType
{
    _00_Match,
}