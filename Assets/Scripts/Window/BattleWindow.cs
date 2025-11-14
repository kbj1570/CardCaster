using UnityEngine;

using System.Collections.Generic;
public class BattleWindow : MonoBehaviour
{
	private int dungeonNum;
	public Transform actorPosition;
	public SpriteRenderer backGroundSpriteRenderer;
	public SpriteRenderer floorSpriteRenderer;
	public List<Sprite> leftBackGroundSprite;
	public List<Sprite> rightBackGroundSprite;
	public List<Sprite> floorSprite;

	private GameObject currentActor;


	public void SetBackGround(int dungeonNum)
	{
		this.dungeonNum = dungeonNum;
		//if (reversed)
		//{
		//	backGroundSpriteRenderer.sprite = rightBackGroundSprite[dungeonNum];
		//	actorPosition.localPosition = new Vector3(3.5f, actorPosition.localPosition.y, actorPosition.localPosition.z);
		//}
		//else
		//{
		//	backGroundSpriteRenderer.sprite = leftBackGroundSprite[dungeonNum];
		//	actorPosition.localPosition = new Vector3(-3.5f, actorPosition.localPosition.y, actorPosition.localPosition.z);
		//}

		floorSpriteRenderer.sprite = floorSprite[dungeonNum];
	}
	public void SetActor(GameObject actor)
	{
		    // 부모 스케일/좌표계 **상속** (월드 보존 X)
		actor.transform.SetParent(actorPosition, false);

		// 부모의 기준점에 정확히 붙이기
		actor.transform.localPosition = Vector3.zero;
		actor.transform.localRotation = Quaternion.identity;

		// 원하는 기본 스케일(부모 스케일을 상속하길 원하면 Vector3.one 권장)
		actor.transform.localScale = new Vector3(0.4f, 0.4f, 1);

		currentActor = actor;
	}

	public void ClearActor()
	{
		if(currentActor != null)
		{
			Destroy(currentActor.gameObject);
			currentActor = null;
		}
	}
}
