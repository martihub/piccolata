﻿using UnityEngine;
using BehaviorDesigner.Runtime;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MatchPart : MonoBehaviour
{
    public int ID;
    public bool isOK, isClicked;
    public Sprite frontSprite, backSprite;
    public Image image;
    public TextMeshProUGUI number;
    public AnimationCurve headShakeCurve;
    public AnimationCurve jumpCurve;
    private BehaviorTree behaviorTree;
    private Sequence sequence;

    private void Start()
    {
        behaviorTree = transform.root.GetComponent<BehaviorTree>();

    }

    public void SendEvent()
    {
        if (IsClickable())
        {
            behaviorTree.SendEvent<object>("SlotClick", gameObject);
            //isClicked = true;
        }
    }

    public bool IsClickable()
    {

        var isClickable = ((SharedBool)behaviorTree.GetVariable("IsClickable")).Value;
        Debug.Log(isOK + " " + isClicked + " " + isClickable);
        return !isOK && !isClicked && isClickable ? true : false;
    }



    public void ChangeSprite(bool isFront, float duration, bool headShake)
    {
        sequence = null;
        sequence = DOTween.Sequence();
        if (!isFront)
        {
            if (!headShake)
            {
                sequence.Append(image.transform.DOLocalRotate(new Vector3(0, 180, 0), duration)).SetEase(Ease.InOutCubic);
                sequence.InsertCallback(duration * .3f, () => image.sprite = backSprite);
            }
            else
            {
                sequence.Append(image.transform.DOLocalRotate(new Vector3(0, 180, 0), duration)).SetEase(headShakeCurve);
                sequence.InsertCallback(duration * .3f, () => image.sprite = backSprite);
            }

        }
        else
        {
            sequence.Append(image.transform.DOLocalRotate(new Vector3(0, 0, 0), duration)).SetEase(Ease.InOutCubic);
            sequence.InsertCallback(duration * .5f, () => image.sprite = frontSprite);
        }
    }

    public void ScaleSprite()
    {
        image.transform.DOScale(new Vector3(.75f, .75f, .75f), .5f).SetEase(jumpCurve);
    }

    public void SetFrontSprite(Sprite sprite)
    {
        frontSprite = sprite;
        image.sprite = sprite;
    }
}
