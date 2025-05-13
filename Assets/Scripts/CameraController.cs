
using UnityEngine;
using DG.Tweening;
using System.Collections;

//  public class CameraController : MonoBehaviour
//     {
//         public GameObject camera;
//         public float moveRate;

//         private Vector3 tmpClickPos;
//         private Vector3 tempCameraPos;

//         public Transform player; // 플레이어
//         private float followSpeed = 0.7f; // 카메라 이동 속도

//         void LateUpdate()
//         {
//             Vector3 targetPosition = new Vector3(player.position.x, player.position.y, camera.transform.position.z);
//             camera.transform.DOMove(targetPosition, followSpeed).SetEase(Ease.OutSine);
//         }
//         void Start()
//         {

//         }
//         void Update()
//         {
//             MouseMovement();
//         }

//         private void MouseMovement()
//         {
//             if(Input.GetMouseButtonDown(0))
//             {
//                 tmpClickPos = Input.mousePosition;
//                 tempCameraPos = camera.transform.position;
//             }
//             else if(Input.GetMouseButton(0))
//             {
//                 Vector3 movePos = Camera.main.ScreenToViewportPoint(tmpClickPos - Input.mousePosition);
//                 camera.transform.position = tempCameraPos + (movePos * moveRate);

//             }
//         }
//     }


public class CameraController : MonoBehaviour
{
	public GameObject camera;
	public Transform player;
	private float followSpeed = 0.7f;
	private float returnToFollowDelay = 3f;
	private bool isFollowing = true;
	private float lastDragTime;
	private Coroutine zoomCoroutine;
	private float originSize;
	public static CameraController Inst{get; private set;}
	void Awake() => Inst = this;

	private bool dragLocked = false;

	void Start()
	{originSize = camera.GetComponent<Camera>().orthographicSize;}

	void Update()
	{
		// 마우스 드래그로 카메라 이동 감지
		if (Input.GetMouseButton(0) && !dragLocked)
		{
		
			isFollowing = false;
			lastDragTime = Time.time;
			DragCamera();
		}
		else if (!isFollowing && Time.time - lastDragTime > returnToFollowDelay)
		{isFollowing = true;}
	}

	void LateUpdate()
	{
		if (isFollowing)
		{
			Vector3 targetPosition = new Vector3(player.position.x, player.position.y, camera.transform.position.z);
			camera.transform.DOMove(targetPosition, followSpeed).SetEase(Ease.OutSine);
		}
	}

	public void ZoomIn(float time)
	{
		if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
		zoomCoroutine = StartCoroutine(CameraZoomInEffect(time));
	}

	public void ZoomOut(float time)
	{
		if (zoomCoroutine != null) StopCoroutine(zoomCoroutine);
		zoomCoroutine = StartCoroutine(CameraZoomOutEffect(time));
	}

	public IEnumerator  CameraZoomInEffect(float time)
	{
		float startSize = originSize;
		float targetSize = startSize * 0.7f;

		for (float t = 0; t < 1; t += Time.deltaTime * time)
		{
			camera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(startSize, targetSize, t);
			yield return null;
		}

	}


	public IEnumerator CameraZoomOutEffect(float time)
	{
		float startSize = camera.GetComponent<Camera>().orthographicSize;
		float targetSize = originSize;

		for (float t = 0; t < 1; t += Time.deltaTime * time)
		{
			camera.GetComponent<Camera>().orthographicSize = Mathf.Lerp(startSize, targetSize, t);
			yield return null;
		}

	}




	void DragCamera()
	{
		float dragSpeed = 0.5f;
		Vector3 move = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0) * dragSpeed;
		camera.transform.position += move;
	}

	public void SetFollowing()
	{isFollowing = true;}

	public void SetDragLock(bool value)
	{dragLocked = value;}
}

