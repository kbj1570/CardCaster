using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CameraEdgeMove : MonoBehaviour
{
	Vector2 minMaxX = new Vector2(-2.7f, 2.7f);
	private float smoothTime = 0.2f;

	[SerializeField] private Button leftButton;
	[SerializeField] private Button rightButton;

	private Coroutine moveRoutine;

	public void MoveLeft()
	{
		StartMove(minMaxX.x);
	}

	public void MoveRight()
	{
		StartMove(minMaxX.y);
	}

	public void MoveCenter()
	{
		StartMove((minMaxX.x + minMaxX.y) * 0.5f);
	}

	private void StartMove(float targetX)
	{
		// 이동 중에는 버튼들 잠시 비활성화
		if (leftButton) leftButton.gameObject.SetActive(false);
		if (rightButton) rightButton.gameObject.SetActive(false);

		// 이미 이동중이면 중단
		if (moveRoutine != null)
			StopCoroutine(moveRoutine);

		moveRoutine = StartCoroutine(MoveCamera(targetX));
	}

	private IEnumerator MoveCamera(float targetX)
	{
		float velocity = 0f;

		while (Mathf.Abs(transform.position.x - targetX) > 0.01f)
		{
			float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocity, smoothTime);
			transform.position = new Vector3(newX, transform.position.y, transform.position.z);
			yield return null;
		}

		// 최종 위치 보정
		transform.position = new Vector3(targetX, transform.position.y, transform.position.z);

		// 도착한 위치에 따라 버튼 표시 제어
		if (Mathf.Approximately(targetX, minMaxX.x))
		{
			// 왼쪽 끝 → 오른쪽 버튼만 보이기
			if (rightButton) rightButton.gameObject.SetActive(true);
		}
		else if (Mathf.Approximately(targetX, minMaxX.y))
		{
			// 오른쪽 끝 → 왼쪽 버튼만 보이기
			if (leftButton) leftButton.gameObject.SetActive(true);
		}
		else
		{
			// 중앙일 때 → 둘 다 보이기
			if (leftButton) leftButton.gameObject.SetActive(true);
			if (rightButton) rightButton.gameObject.SetActive(true);
		}

		moveRoutine = null;
	}
}