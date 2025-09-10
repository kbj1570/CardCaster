using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class FloatingDamageText : MonoBehaviour
{
    public TMP_Text textMesh;
    float floatDistance = 0.4f;  // 떠오르는 거리
    float scaleUpSize = 1.2f;    // 커지는 크기 비율
    float scaleDownSize = 0.1f;

    float duration  = 1.5f;

    public void SetDamageText(int damage)
    {
        textMesh.text = damage.ToString();
        this.transform.localScale = Vector3.zero;
        AnimateFloatingText();
    }

    public void SetDamageText(string text)
    {
        textMesh.text = text;
        this.transform.localScale = Vector3.zero;
        AnimateFloatingText();
    }

    public void SetFont(int size)
    {
        textMesh.fontSize = size;
    }

    public void SetColor(Color color)
    {textMesh.color = color;}

    private void AnimateFloatingText()
    {
        Vector3 startPos = transform.localPosition;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * scaleUpSize, 0.3f).SetEase(Ease.OutBack));
        seq.Join(transform.DOLocalMove(startPos + new Vector3(Random.Range(-0.7f, 0.7f), floatDistance, 0), 1f).SetEase(Ease.OutCubic));
        seq.Append(transform.DOScale(Vector3.one * scaleDownSize, 0.3f).SetEase(Ease.InQuad));
        seq.Join(textMesh.DOFade(0, 0.3f).SetEase(Ease.InQuad));
        seq.OnComplete(() => Destroy(gameObject));
        seq.Play();
    }
}