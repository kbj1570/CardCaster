using UnityEngine;

public class CameraEdgeMove : MonoBehaviour
{
	float moveSpeed = 10f;          // 카메라 이동 속도
	float edgePercent = 0.07f;      // 화면 끝에서 몇 % 지점부터 이동할지
	Vector2 minMaxX = new Vector2(-2.7f, 2.7f); // 카메라 X 이동 한계

	private float smoothTime = 0.2f;

	private float velocity = 0f;
	private float targetX;
	void Start()
	{
		targetX = transform.position.x;
	}

	void Update()
	{

		if (CampsiteManager.Inst.screenLocked)
			return;


		float mouseX = Input.mousePosition.x;
		float screenWidth = Screen.width;

		float leftEdge = screenWidth * edgePercent;
		float rightEdge = screenWidth * (1f - edgePercent);

		if (mouseX < leftEdge)
		{
			targetX -= moveSpeed * Time.deltaTime;
		}
		else if (mouseX > rightEdge)
		{
			targetX += moveSpeed * Time.deltaTime;
		}

		targetX = Mathf.Clamp(targetX, minMaxX.x, minMaxX.y);

		float newX = Mathf.SmoothDamp(transform.position.x, targetX, ref velocity, smoothTime);
		transform.position = new Vector3(newX, transform.position.y, transform.position.z);
	}
}