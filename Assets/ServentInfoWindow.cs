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
    public Sprite lightAttribute;

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
            case EMonsterAttribute.Fire:
            serventAttribute.sprite = fireAttribute;
            break;

            case EMonsterAttribute.Wind:
            serventAttribute.sprite = windAttribute;
            break;

            case EMonsterAttribute.Ice:
            serventAttribute.sprite = iceAttribute;
            break;

            case EMonsterAttribute.Earth:
            serventAttribute.sprite = earthAttribute;
            break;

            case EMonsterAttribute.Darkness:
            serventAttribute.sprite = darknessAttribute;
            break;

            case EMonsterAttribute.Light:
            serventAttribute.sprite = lightAttribute;
            break;
        }
    }

    public void OnOff(bool isOpened)
    {
        if(!isOpened)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
        }
        else
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.InOutQuad);
        }
    }

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;
}
