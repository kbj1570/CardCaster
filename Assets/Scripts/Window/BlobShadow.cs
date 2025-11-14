using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BlobShadow : MonoBehaviour
{
    public Transform target;           // 캐릭터 Transform
    public float baseScale = 1.0f;     // 기본 크기
    public float maxStretch = 0.25f;   // 위아래로 늘어나는 정도
    public float minAlpha = 0.25f;
    public float maxAlpha = 0.6f;
    public float heightSensitivity = 0.5f; // y 높이에 따른 변화 민감도

    SpriteRenderer sr;
    float baseY;

    void Awake() {
        sr = GetComponent<SpriteRenderer>();
        if (target == null) target = transform.parent;
        baseY = target.position.y;
    }

    void LateUpdate() {
        float h = (target.position.y - baseY) * heightSensitivity;
        float scale = baseScale * (1f - Mathf.Clamp(h, -maxStretch, maxStretch));
        transform.position = new Vector3(target.position.x, baseY - 0.02f, transform.position.z);
        transform.localScale = new Vector3(scale, scale * 0.6f, 1f); // 살짝 납작

        float a = Mathf.Lerp(maxAlpha, minAlpha, Mathf.InverseLerp(-maxStretch, maxStretch, h));
        var c = sr.color; c.a = a; sr.color = c;
    }
}