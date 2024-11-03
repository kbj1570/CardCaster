using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;

public class SpeechBubble : MonoBehaviour
{
    private bool isOpened;
    public TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        ScaleZero();
    }

    public void On()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.InOutQuad);
            isOpened = true;

        Invoke("Off", 1.5f);
    }

    public void Off()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence()
            .Append(transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
            isOpened = false;
    }

    public void SetText(string value)
    {
        this.text.text = value;
    }

    [ContextMenu("ScaleOne")]
    void ScaleOne() => transform.localScale = Vector3.one;

    [ContextMenu("ScaleZero")]
    void ScaleZero() => transform.localScale = Vector3.zero;
}
