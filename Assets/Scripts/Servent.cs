using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Servent : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler , IPointerClickHandler
{
	private ServentCardData cardData;

	private EServentType serventType;
	private EServentState serventState;
	private EServentAttribute serventAttribute;

	private Field field;

	private Sprite idle;
	private Sprite attack;
	private Sprite ready;
	private Sprite guard;
	private Sprite death;

	public Transform dragPoint;
	public int currentForce;


	public GameObject border;
	public GameObject infoWindow;
	public GameObject floatingTextPrefab;
	public TMP_Text serventForceText;
	public SpriteRenderer spriteRenderer;

	int maxAttackCount = 1;
	int attackCount;

	int maxActivationCount = 1;
	int activationCount;

	public Color fadeColor;
	public int serventNum;
	private bool mouseOn;

	public Texture2D texture2D;
	private Material monsterMaterial;

	public EBattleObjectType battleObjectType = EBattleObjectType.Servent;

	bool isDissolving = false;
	bool isDying = false;
	bool locked;
	float fade = 1f;

	void Awake()
	{
		monsterMaterial = spriteRenderer.GetComponent<SpriteRenderer>().material;
		monsterMaterial.SetTexture("_MainTex", texture2D);
		monsterMaterial.SetFloat("_Fade", fade);
		monsterMaterial.SetColor("_Color", fadeColor);
	}

	public void OnBattleWindow()
	{spriteRenderer.sortingOrder = 104;}

	void Update()
	{
		serventForceText.text = currentForce.ToString();

		if (isDissolving)
		{
			fade += Time.deltaTime * 1.1f;

			if (fade >= 1f)
			{
				fade = 1f;
				isDissolving = false;
			}
			monsterMaterial.SetFloat("_Fade", fade);
		}
	}
	public void InitWithEffect()
	{
		isDissolving = true;
		fade = 0f;
		monsterMaterial.SetFloat("_Fade", fade);
	}

	public IEnumerator DieCoroutine()
	{
		float fade = 1f; // 처음 페이드 값
		BattleManager.Inst.PlayServentDeathSound();

		while (fade > 0.1f)
		{
			fade -= Time.deltaTime * 1.1f;
			monsterMaterial.SetFloat("_Fade", fade);
			yield return null; // 다음 프레임까지 대기
		}

		fade = 0f;
		monsterMaterial.SetFloat("_Fade", fade);
		isDying = false;

		BattleManager.Inst.AddTrash(cardData);
		Vector3 startPos = transform.position;
		BattleManager.Inst.ShotMissile(startPos);
		Destroy(gameObject);
		yield break;
	}
	public void ChangeState(EServentState state)
	{
		switch (state)
		{
			case EServentState.Idle:
				spriteRenderer.sprite = idle;
				break;
			case EServentState.Attack:
				spriteRenderer.sprite = attack;
				break;
			case EServentState.Guard:
				spriteRenderer.sprite = guard;
				break;
			case EServentState.Death:
				spriteRenderer.sprite = death;
				break;
			case EServentState.Ready:
				spriteRenderer.sprite = ready;
				break;
		}
	}

	public void OnMouseEnter()
	{ mouseOn = true; }
	public void OnMouseExit()
	{ mouseOn = false; }

	void OnDestroy()
	{
		field.SetFilled(false);
	}

	public void SetLock(bool locked)
	{ this.locked = locked; }
	public int GetServentNum()
	{ return serventNum; }
	public EServentType GetServentType()
	{ return serventType; }
	public void SetServentType(EServentType serventType)
	{ this.serventType = serventType; }
	public Transform GetDragPoint()
	{ return dragPoint; }
	public void GainForce(int value)
	{
		GameObject damageText = Instantiate(floatingTextPrefab, this.transform);
		damageText.transform.position = serventForceText.transform.position;
		damageText.GetComponent<FloatingDamageText>().SetDamageText(value);
		damageText.GetComponent<FloatingDamageText>().SetFont(20);
		damageText.GetComponent<FloatingDamageText>().SetColor(Color.green);
		currentForce += value;
	}
	public void LoseForce(int value)
	{currentForce -= value;}

	public void SetForce(int value)
	{currentForce = value;}

	public int GetForce()
	{ return currentForce; }

	public void SetAttribute(EServentAttribute value)
	{ serventAttribute = value; }

	public ServentCardData GetCardData()
	{ return cardData; }

	public EServentAttribute GetAttribute()
	{ return serventAttribute; }

	public void OnBeginDrag(PointerEventData eventData)
	{
		if(locked) return;

		BattleManager.Inst.ReadyServentAttack(this);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (locked) return;
		StartCoroutine(BattleManager.Inst.ShowServentInfo(this));
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (locked) return;
		if (BattleManager.Inst.CheckAttackable(this))
			StartCoroutine(BattleManager.Inst.BattlePhase());

		BattleManager.Inst.ClearLine();
	}
	public void OnDrag(PointerEventData eventData)
	{
		if (locked) return;
		BattleManager.Inst.DrawAttackLine(this.transform.position, BattleManager.Inst.CheckAttackable(this));
	}

	public void TakeDamage(int damage)
	{
		GameObject damageText = Instantiate(floatingTextPrefab, this.transform);
		damageText.transform.position = serventForceText.transform.position;
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(20);
		damageText.GetComponent<FloatingDamageText>().SetColor(Color.red);
		currentForce -= damage;
	}

	public void AddAttackCount()
	{ attackCount++; }

	public void ResetAttackCount()
	{attackCount = 0;}

	public void AddActivationCount()
	{ activationCount++; }

	public bool IsAttackable()
	{ return attackCount < maxAttackCount; }

	public bool IsActivationable()
	{ return activationCount < maxActivationCount; }

	public void HideForce()
	{
		if (serventForceText.gameObject == null) return;

		serventForceText.gameObject.SetActive(false);
	}
	public void ShowForce()
	{

		if (serventForceText.gameObject == null) return;

		serventForceText.gameObject.SetActive(true);
	}

	public void SetCardData(ServentCardData cardData)
	{this.cardData = cardData;}


	public void SetField(Field field)
	{ this.field = field; }

	public Field GetField()
	{ return field; }

}