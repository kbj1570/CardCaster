using UnityEngine;
using TMPro;
using DG.Tweening;

public class FloatingDamageText : MonoBehaviour
{
    public TMP_Text textMesh;
    float floatDistance = 0.2f;  // 떠오르는 거리
    float scaleUpSize = 1f;    // 커지는 크기 비율
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
        // (풀링 대비) 초기 상태 리셋
        transform.localScale = Vector3.zero;
        var c = textMesh.color; c.a = 1f; textMesh.color = c;

        Vector3 startPos = transform.position;                       // 월드 기준
        Vector3 offset   = new Vector3(Random.Range(-0.7f, 0.7f),    // 좌우 흔들림
                                    floatDistance, 0f);

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * scaleUpSize, 0.3f).SetEase(Ease.OutBack));
        seq.Join(transform.DOMove(startPos + offset, 1f).SetEase(Ease.OutCubic)); // ← DOMove 사용
        seq.Append(transform.DOScale(Vector3.one * scaleDownSize, 0.3f).SetEase(Ease.InQuad));
        seq.Join(textMesh.DOFade(0f, 0.3f).SetEase(Ease.InQuad));
        seq.OnComplete(() => Destroy(gameObject));
        seq.Play();
    }
}