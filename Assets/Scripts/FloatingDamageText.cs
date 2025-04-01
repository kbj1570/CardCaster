using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class FloatingDamageText : MonoBehaviour
{
    public TMP_Text textMesh;
    public float floatDistance = 100f;  // 떠오르는 거리

    public float scaleUpSize = 1.5f;    // 커지는 크기 비율
    public float scaleDownSize = 0.1f;

    public float duration  = 1f;

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
        // 시작 위치 저장
        // Vector3 startPos = transform.position;

        // // Y축으로 부드럽게 떠오르면서 점점 사라지기
        // transform.DOMoveY(startPos.y + floatDistance, duration).SetEase(Ease.OutCubic);
        // transform.DOMove(startPos + new Vector3(Random.Range(-50f, 50f), floatDistance, 0), duration);
        // textMesh.DOFade(0, duration).SetEase(Ease.InQuad).OnComplete(() => Destroy(gameObject));


        Vector3 startPos = transform.position;

        Sequence seq = DOTween.Sequence();


        seq.Append(transform.DOScale(Vector3.one * scaleUpSize, 0.2f).SetEase(Ease.OutBack));

        seq.Join(transform.DOMove(startPos + new Vector3(Random.Range(-50f, 50f), floatDistance, 0), 0.3f).SetEase(Ease.OutCubic));

        seq.Append(transform.DOScale(Vector3.one * scaleDownSize, 0.2f).SetEase(Ease.InQuad));
        seq.Join(textMesh.DOFade(0, 0.2f).SetEase(Ease.InQuad));

        seq.OnComplete(() => Destroy(gameObject));

        seq.Play();
    }
}