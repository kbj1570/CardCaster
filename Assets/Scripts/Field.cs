using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Field : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
	ServentCardData cardData;
	List<EServentCondition> conditions;
	EServentAttribute serventAttribute;
	public EMouseOnArea mouseOnArea;

	public EAbilityType abilityType;

	public bool filled;
	public bool locked;
	public bool isDragable;
	private bool attacked;
	public TMP_Text forceTMP;

	public GameObject conditionPanel;
	public GameObject conditionPanelButton;

	public GameObject floatingTextPrefab;

	public GameObject monsterPrefab;
	public GameObject summonEffectPrefab;
	public GameObject summonEffectObject;
	public GameObject monsterEntity;

	public Transform lowLinePoint;
	public Transform middleLinePoint;
	public Transform highLinePoint;

	public Color forceColorFire;
	public Color forceColorWater;
	public Color forceColorEarth;
	public Color forceColorWind;
	public Color forceColorDarkness;
	public Color forceColorLightness;
	
	public int currentForce;
	public int fieldNum;

	bool penetrate;
	bool suicide;

	bool voidWalker;
	bool damageBlock;

	int damageDecrease;
	int damageIncrease;

	int additionalForce;

	private Servent serventObject;
	

	public void SetForce(int value)
	{
		if(voidWalker)
		return;

		currentForce = value;
	}
	public int GetForce(){return currentForce;}

	public int GetFieldNum(){return fieldNum;}

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

	public void SetHealth(int value)
	{
		currentForce = value;
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

	public void Summon(ServentCardData cardData, Servent serventObject)
	{

		filled = true;
		this.cardData = cardData;
		currentForce = cardData.GetForce();
		forceTMP.gameObject.SetActive(true);
		forceTMP.text = currentForce.ToString();
		
		attacked = false;
		penetrate = cardData.GetPenetrate();
		voidWalker = cardData.GetVoidWalker();
		serventAttribute = cardData.GetAttribute();
		this.serventObject = serventObject;
		gameObject.GetComponent<Servent>().SetServentType(cardData.GetServentType());
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

	public void ActivateTurnEnd()
	{

		if(voidWalker)
		return;

		if(suicide)
		{currentForce = 0;}


	}

	public void ResetCondition()
	{

		if(voidWalker)
		return;

		conditions.Clear();
	}
	
	public void AddCondition(EServentCondition value)
	{
		if(voidWalker)
		return;

		conditions.Add(value);
	}

	public void RemoveCondition(EServentCondition value)
	{

		if(voidWalker)
		return;

		conditions.Remove(value);
	}

	public bool GetFilled()
	{return filled;}

	public bool GetAttacked()
	{return attacked;}

	public void SetAttacked(bool value)
	{this.attacked = value;}
	public ServentCardData GetCardData()
	{return cardData;}

	public void SetSuicide(bool value)
	{
		if(voidWalker)
		return;

		suicide = value;
	}

	public bool GetPenetrate()
	{return penetrate;}

	public EMouseOnArea GetMouseOnArea() {return mouseOnArea;}


	public Transform GetLinePoint()
	{
		if(!filled)
		return lowLinePoint;

		return serventObject.GetDragPoint();
	}
	public void OnBeginDrag(PointerEventData eventData)
	{ }

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!GetFilled())
			return;
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!GetFilled())
			return;

		if (mouseOnArea != EMouseOnArea.Hole && mouseOnArea != EMouseOnArea.Enemy && mouseOnArea != EMouseOnArea.Player && BattleManager.Inst.CheckAttackable(mouseOnArea))
			StartCoroutine(BattleManager.Inst.EndAttackLine(mouseOnArea, BattleManager.Inst.CheckAttackable(mouseOnArea)));
	}
	public void OnDrag(PointerEventData eventData)
	{

		if (!GetFilled())
			return;

		if (mouseOnArea != EMouseOnArea.Hole && mouseOnArea != EMouseOnArea.Enemy && mouseOnArea != EMouseOnArea.Player)
			BattleManager.Inst.DrawAttackLine(this.transform.position, BattleManager.Inst.CheckAttackable(mouseOnArea));
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