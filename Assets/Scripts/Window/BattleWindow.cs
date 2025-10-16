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
		actor.transform.SetParent(transform);
		actor.transform.localScale = new Vector3(0.55f, 0.55f, 1f);
		actor.transform.localPosition = actorPosition.localPosition;
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
