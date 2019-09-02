using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using BehaviorDesigner.Runtime;
public class MatchManager : MonoBehaviour
{
    public GameType gameType;
    public GameObject panel;
    //  public Image image;
    public Transform enviroMain;
    string gamesFolder = "Games";
    List<GameObject> slots = new List<GameObject>();
    List<GameObject> _slots;
    public Sprite[] sprites;

    private void Start()
    {
        GameObject enviro = Instantiate(BundleWorks.GetObject<GameObject>(GameType._00_Match, BundleType._00_BkgGameObjects, "00", "BgkGameobject"));
        //  enviro.GetComponent<Renderer>().sharedMaterial.shader = Shader.Find("Standard");
        enviro.transform.parent = enviroMain;
        enviro.transform.localEulerAngles = Vector3.zero;
        enviro.transform.localPosition = Vector3.zero;

        //string str = Application.streamingAssetsPath + "/Bundles/_00_Match/_00_BkgGameObjects/00.assetbundle";
        //var myLoadedAssetBundle = AssetBundle.LoadFromFile(str);
        //GameObject gObj = myLoadedAssetBundle.LoadAsset<GameObject>("BgkGameobject");
        //Instantiate(gObj);
        //myLoadedAssetBundle.Unload(false);
        sprites = BundleWorks.GetRandomAssets<Sprite>(GameType._00_Match, BundleType._01_Images);
    }

    public void GenerateSlots(int _count)
    {
        for (int i = 0; i < _count * 2; i++)
        {
            GameObject slot = Instantiate(Resources.Load<GameObject>(gamesFolder + "/" + gameType + "/" + "_etc" + "/" + "Slot"));
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

        for (int i = 0; i < _count * 2; i++)
        {
            int a = slots[i].GetComponent<MatchPart>().ID;
            slots[i].GetComponent<MatchPart>().SetFrontSprite(sprites[a]);
        }
    }
}

