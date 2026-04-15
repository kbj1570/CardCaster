using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Field : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	List<EServentCondition> conditions;
	EServentAttribute serventAttribute;
	public EMouseOnArea mouseOnArea;

	public bool locked;
	public TMP_Text forceTMP;

	public GameObject conditionPanel;
	public GameObject conditionPanelButton;

	public GameObject floatingTextPrefab;

	public GameObject monsterPrefab;
	public GameObject summonEffectPrefab;
	public GameObject summonEffectObject;
	public GameObject monsterEntity;

	public Color forceColorFire;
	public Color forceColorWater;
	public Color forceColorEarth;
	public Color forceColorWind;
	public Color forceColorDarkness;
	public Color forceColorLightness;

	bool filled;


	bool damageBlock;
	int damageDecrease;
	int damageIncrease;

	int additionalForce;
	private Servant serventObject;

	public EServentAttribute GetServentAttribute(){return serventAttribute;}

	public Servant GetServent()
	{ return serventObject;}

	//public void UpdateHealth()
	//{
	//	if(!filled)
	//	{return;}

	//	forceTMP.text = currentForce.ToString();

	//	if(currentForce <= 0)
	//	{
	//		forceTMP.gameObject.SetActive(false);
	//		filled = false;
	//		attacked = false;

	//		if(serventObject.GetComponent<Servent>().GetServentType() == EServentType.Player)
	//		BattleManager.Inst.AddTrash(cardData);

			
	//		serventObject.GetComponent<Servent>().Dead();
	//		currentForce = 0;
	//	}
	//}

	public void Summon(Servant serventObject, ServantCardData cardData)
	{
		this.serventObject = serventObject;
		filled = true;
		serventObject.SetCardData(cardData);
		serventObject.SetField(this);
		serventObject.SetAttribute(cardData.GetAttribute());
		serventObject.SetServentType(cardData.GetServentType());
		serventObject.SetForce(cardData.GetForce());
		locked = false;
	}

	public void HideForce(bool value)
	{
		forceTMP.gameObject.SetActive(!value);
	}

	public void UpdateCondition()
	{
		for(int i = 0; i < conditionPanel.transform.childCount; ++i)
		{Destroy(conditionPanel.transform.GetChild(i).gameObject);}

		foreach(EServentCondition condition in conditions)
		{
			GameObject gameObject = Instantiate(BattleManager.Inst.ReturnConditionMark(condition),
			conditionPanel.transform.position, Utils.QI);
			gameObject.transform.SetParent(conditionPanel.transform);
		}

		if(conditions.Count > 3)
		{conditionPanelButton.SetActive(true);}
		else
		{conditionPanelButton.SetActive(false);}
	}


	public EMouseOnArea GetMouseOnArea() {return mouseOnArea;}


	public Transform GetLinePoint()
	{
		return this.transform;

		//return serventObject.GetDragPoint();
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		BattleManager.Inst.SetMouseOnField(mouseOnArea);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		BattleManager.Inst.ResetMouseOnField();
	}
	public void SetFilled(bool value) { filled = value; }

	public bool IsFilled() { return filled; }
}