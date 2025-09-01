using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Field : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	List<EServentCondition> conditions;
	EServentAttribute serventAttribute;
	public EMouseOnArea mouseOnArea;

	public EAbilityType abilityType;

	public bool filled;
	public bool locked;
	public bool isDragable;
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


	bool damageBlock;
	int damageDecrease;
	int damageIncrease;

	int additionalForce;
	private Servent serventObject;

	public void GainForce(int value)
	{
		if(voidWalker)
		return;

		GameObject damageText = Instantiate(floatingTextPrefab);
		damageText.GetComponent<FloatingDamageText>().SetFont(100);
		damageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(value);

		currentForce += value;
	}
	public EServentAttribute GetServentAttribute(){return serventAttribute;}
	public void LoseForce(int value)
	{
		if(voidWalker)
		return;

		if(!filled)
		return;

		if(damageBlock)
		return;

		currentForce -= value;
	}

	public Servent GetServent()
	{ return serventObject;}

	public void TakeDamage(int value)
	{
		if(!filled)
			return;

		if(damageBlock)
			return;

		// 피해 숫자 표시
		GameObject damageText = Instantiate(floatingTextPrefab);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(value);
		damageText.GetComponent<FloatingDamageText>().SetFont(150);

		currentForce -= value;
	}

	public void TakeAttack(int value)
	{
		if (!filled)
			return;

		if (damageBlock)
			return;
		currentForce -= value;
	}

	public void Kill()
	{
		if(voidWalker)
		{return;}

		forceTMP.gameObject.SetActive(false);
		filled = false;
		attacked = false;

		if(serventObject.GetComponent<Servent>().GetServentType() == EServentType.Player)
		BattleManager.Inst.AddTrash(cardData);

		
		serventObject.GetComponent<Servent>().Dead();
		currentForce = 0;
	}

	public void UpdateHealth()
	{
		if(!filled)
		{return;}

		forceTMP.text = currentForce.ToString();

		if(currentForce <= 0)
		{
			forceTMP.gameObject.SetActive(false);
			filled = false;
			attacked = false;

			if(serventObject.GetComponent<Servent>().GetServentType() == EServentType.Player)
			BattleManager.Inst.AddTrash(cardData);

			
			serventObject.GetComponent<Servent>().Dead();
			currentForce = 0;
		}
	}

	public void Summon(Servent serventObject, ServentCardData cardData)
	{

		filled = true;
		this.serventObject = serventObject;
		serventObject.GetComponent<Servent>().SetServentType(cardData.GetServentType());
		serventObject.GetComponent<Servent>().SetForce(cardData.GetForce());
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

	public bool GetFilled()
	{return filled;}


	public EMouseOnArea GetMouseOnArea() {return mouseOnArea;}


	public Transform GetLinePoint()
	{
		if(!filled)
		return this.transform;

		return serventObject.GetDragPoint();
	}
	public void OnPointerEnter(PointerEventData eventData)
	{
		BattleManager.Inst.SetMouseOnField(mouseOnArea);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		BattleManager.Inst.ResetMouseOnField();
	}
}