using System.Collections;
using UnityEngine;

public class Direction : MonoBehaviour
{
    float floatSize = 0.15f;
    float floatSpeed = 3f;
    private float startY;
    private float startX;
    float radius = 2f;
    float rotateDuration = 0.1f;
    public EDirection direction;
    private float currentAngle;
    private float targetAngle;
    private bool isRotating = false;

    void Start()
    {
        SetStartXY(transform.localPosition.x, transform.localPosition.y);
    }

    void Update()
    {
        // 플로팅 애니메이션만 유지
        if (direction == EDirection.North || direction == EDirection.South)
        {
            float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatSize;
            transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
        }
        else
        {
            float newX = startX + Mathf.Sin(Time.time * floatSpeed) * floatSize;
            transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
        }
    }

    void SetStartXY(float x, float y)
    {
        startX = x;
        startY = y;
    }

    public EDirection GetDirection()
    {
        return direction;
    }

    public void SetDirection(EDirection newDirection)
    {
        if (direction == newDirection) return; // 같은 방향이면 무시
        direction = newDirection;

        // 방향에 따라 목표 각도 지정
        float degrees = 0f;
        switch (newDirection)
        {
            case EDirection.North:
                degrees = 90f; break;
            case EDirection.East:
                degrees = 0f; break;
            case EDirection.South:
                degrees = 270f; break;
            case EDirection.West:
                degrees = 180f; break;
        }

        // 회전 시작
        targetAngle = degrees * Mathf.Deg2Rad;
        StartCoroutine(RotateAlongCircle());
    }

    IEnumerator RotateAlongCircle()
    {
        isRotating = true;

        float startAngle = currentAngle;
        float angleDiffDeg = Mathf.DeltaAngle(startAngle * Mathf.Rad2Deg, targetAngle * Mathf.Rad2Deg);
        float finalAngle = startAngle + angleDiffDeg * Mathf.Deg2Rad;

        float time = 0f;
        while (time < rotateDuration)
        {
            time += Time.deltaTime;
            float t = time / rotateDuration;

            float angleNow = Mathf.LerpAngle(startAngle * Mathf.Rad2Deg, finalAngle * Mathf.Rad2Deg, t) * Mathf.Deg2Rad;
            UpdateArrowPosition(angleNow);

            yield return null;
        }

        currentAngle = finalAngle;
        UpdateArrowPosition(currentAngle);
        SetStartXY(transform.localPosition.x, transform.localPosition.y);
        isRotating = false;
    }

    void UpdateArrowPosition(float angle)
    {
        Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
        transform.localPosition = offset;

        float zRotation = angle * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, zRotation);
    }
}