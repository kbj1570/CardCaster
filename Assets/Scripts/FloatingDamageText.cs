using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;

public class FloatingDamageText : MonoBehaviour
{
    public TMP_Text textMesh;
    public float floatDistance = 100f;  // 떠오르는 거리
    public float duration = 1f;       // 애니메이션 지속 시간

    public void SetDamageText(int damage)
    {
        textMesh.text = damage.ToString();
        AnimateFloatingText();
    }

    private void AnimateFloatingText()
    {
        // 시작 위치 저장
        Vector3 startPos = transform.position;

        // Y축으로 부드럽게 떠오르면서 점점 사라지기
        transform.DOMoveY(startPos.y + floatDistance, duration).SetEase(Ease.OutCubic);
        transform.DOMove(startPos + new Vector3(Random.Range(-50f, 50f), floatDistance, 0), duration);
        textMesh.DOFade(0, duration).SetEase(Ease.InQuad).OnComplete(() => Destroy(gameObject));
    }
}