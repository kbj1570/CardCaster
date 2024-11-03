using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Window : MonoBehaviour
{

    bool isOpened;
    void Start()
    {
        ScaleZero();
        
    }
    void Update()
    {
        
    }

    public void OnOff()
    {
        if(isOpened)
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
            isOpened = false;
        }
        else
        {
            DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.InOutQuad);
            isOpened = true;
        }
    }

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;
}
