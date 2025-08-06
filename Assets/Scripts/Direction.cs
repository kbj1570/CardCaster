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
		if (isRotating) {return;}
		else if (direction == EDirection.North || direction == EDirection.South)
		{
			float newY = startY + Mathf.Sin(Time.time * floatSpeed) * floatSize;
			transform.localPosition = new Vector3(transform.localPosition.x, newY, transform.localPosition.z);
		}
		else
		{
			float newX = startX + Mathf.Sin(Time.time * floatSpeed) * floatSize;
			transform.localPosition = new Vector3(newX, transform.localPosition.y, transform.localPosition.z);
		}

		if (isRotating) return;

		if (Input.GetKeyDown(KeyCode.UpArrow))
			SetTargetAngle(90f);     // 위
		else if (Input.GetKeyDown(KeyCode.RightArrow))
			SetTargetAngle(0f);      // 오른쪽
		else if (Input.GetKeyDown(KeyCode.DownArrow))
			SetTargetAngle(270f);    // 아래
		else if (Input.GetKeyDown(KeyCode.LeftArrow))
			SetTargetAngle(180f);    // 왼쪽



		if (Input.GetKeyDown(KeyCode.W))
		{
			SetTargetAngle(90f);
		}
		else if (Input.GetKeyDown(KeyCode.D))
		{
			SetTargetAngle(0f);
		}
		else if (Input.GetKeyDown(KeyCode.S))
		{
			SetTargetAngle(270f);
		}
		else if (Input.GetKeyDown(KeyCode.A))
		{
			SetTargetAngle(180f);
		}

	}

	void SetTargetAngle(float degrees)
	{
		targetAngle = degrees * Mathf.Deg2Rad; // 라디안 변환
		StartCoroutine(RotateAlongCircle());
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
	{direction = newDirection;}

	IEnumerator RotateAlongCircle()
	{
		isRotating = true;

		float startAngle = currentAngle;

		// 최단 거리 각도 차이 계산 (DeltaAngle은 결과가 -180~180도 범위)
		float angleDiffDeg = Mathf.DeltaAngle(startAngle * Mathf.Rad2Deg, targetAngle * Mathf.Rad2Deg);
		float finalAngle = startAngle + angleDiffDeg * Mathf.Deg2Rad; // 최종 목표 라디안

		float time = 0f;
		while (time < rotateDuration)
		{
			time += Time.deltaTime;
			float t = time / rotateDuration;

			// 각도 보간 (부드러운 회전)
			float angleNow = Mathf.LerpAngle(startAngle * Mathf.Rad2Deg, finalAngle * Mathf.Rad2Deg, t) * Mathf.Deg2Rad;
			UpdateArrowPosition(angleNow);

			yield return null;
		}

		// 회전 종료 후 최종 각도 확정
		currentAngle = finalAngle;
		UpdateArrowPosition(currentAngle);
		SetStartXY(transform.localPosition.x, transform.localPosition.y);
		isRotating = false;
	}

	void UpdateArrowPosition(float angle)
	{
		// 원 둘레 위치 = 중심 + (cos,sin)*반지름
		Vector3 offset = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius;
		transform.localPosition = Vector3.zero + offset;

		// 화살표 회전 (중심 바라보는 방향 기준으로 -90도 보정)
		float zRotation = angle * Mathf.Rad2Deg - 90f;
		transform.rotation = Quaternion.Euler(0, 0, zRotation);
	}
}
