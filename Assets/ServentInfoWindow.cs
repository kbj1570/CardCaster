using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ServentInfoWindow : MonoBehaviour
{
    public TMP_Text originForce;
    public TMP_Text serventName;
    public TMP_Text serventAbility;
    public Image serventAttribute;

    public Sprite fireAttribute;
    public Sprite windAttribute;
    public Sprite iceAttribute;
    public Sprite earthAttribute;
    public Sprite darknessAttribute;
    public Sprite lightnessAttribute;

    void Start()
    {
        ScaleZero();
    }

    public void UpdateCardData(CardData cardData)
    {
        originForce.text = cardData.GetForce().ToString();
        serventName.text = cardData.GetCardName();
        serventAbility.text = cardData.GetCardAbility();
        switch(cardData.GetAttribute())
        {
            case EServentAttribute.Fire:
            serventAttribute.sprite = fireAttribute;
            break;

            case EServentAttribute.Wind:
            serventAttribute.sprite = windAttribute;
            break;

            case EServentAttribute.Water:
            serventAttribute.sprite = iceAttribute;
            break;

            case EServentAttribute.Earth:
            serventAttribute.sprite = earthAttribute;
            break;

            case EServentAttribute.Dark:
            serventAttribute.sprite = darknessAttribute;
            break;

            case EServentAttribute.Light:
            serventAttribute.sprite = lightnessAttribute;
            break;
        }
    }

    public void OnOff(bool isOpened)
    {
        if(!isOpened)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.2f).SetEase(Ease.OutCirc));
        }
        else
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.2f)).SetEase(Ease.OutCirc);
        }
    }

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;
}
