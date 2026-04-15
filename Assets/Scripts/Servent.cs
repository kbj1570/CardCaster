using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Servant : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler , IPointerClickHandler
{
	private ServantCardData cardData;

	private EServentType serventType;
	private EServentState serventState;
	private EServentAttribute serventAttribute;

	private Field field;

	public Sprite idle;
	public Sprite attack;
	public Sprite ready;
	public Sprite guard;
	public Sprite death;

	public Transform dragPoint;
	public int currentForce;

	private Tweener fadeTween;
	private bool isTransitioning = false;

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

	public Texture2D texture2D;
	private Material monsterMaterial;

	public EBattleObjectType battleObjectType = EBattleObjectType.Servent;

	bool isDissolving = false;
	bool isDying = false;
	bool locked;
	float fade = 1f;

	void Awake()
	{
		spriteRenderer.material = new Material(spriteRenderer.material);
		monsterMaterial = spriteRenderer.material;
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
		float fade = 1f;
		BattleManager.Inst.PlayServentDeathSound();

		while (fade > 0.1f)
		{
			fade -= Time.deltaTime * 1.1f;
			monsterMaterial.SetFloat("_Fade", fade);
			yield return null;
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
	// private void ChangeState(EServentState state)
	// {
	// 	switch (state)
	// 	{
	// 		case EServentState.Idle:
	// 			spriteRenderer.sprite = idle;
	// 			break;
	// 		case EServentState.Attack:
	// 			spriteRenderer.sprite = attack;
	// 			break;
	// 		case EServentState.Guard:
	// 			spriteRenderer.sprite = guard;
	// 			break;
	// 		case EServentState.Death:
	// 			spriteRenderer.sprite = death;
	// 			break;
	// 		case EServentState.Ready:
	// 			spriteRenderer.sprite = ready;
	// 			break;
	// 	}
	// }


	void OnDestroy()
	{
		if (field != null)
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

	public ServantCardData GetCardData()
	{ return cardData; }

	public EServentAttribute GetAttribute()
	{ return serventAttribute; }

	private Sprite SpriteForState(EServentState state)
	{
		switch (state)
		{
			case EServentState.Idle:   return idle;
			case EServentState.Attack: return attack;
			case EServentState.Guard:  return guard;
			case EServentState.Death:  return death;
			case EServentState.Ready:  return ready;
			default:                   return idle;
		}
	}

	public void ChangeState(EServentState state, bool instant = false, float duration = 0.1f)
	{
		if (instant)
		{
			// 즉시 교체(연출 없이 바뀌어야 하는 특수 상황용)
			spriteRenderer.sprite = SpriteForState(state);
			return;
		}
		StartCoroutine(StateTransitionCoroutine(state, duration));
	}

	private IEnumerator StateTransitionCoroutine(EServentState nextState, float halfDuration)
	{
		if (isDying) yield break;
		if (isTransitioning) yield break;
		isTransitioning = true;

		if (fadeTween != null && fadeTween.IsActive()) fadeTween.Kill();

		try
		{
			fadeTween = spriteRenderer.DOFade(0f, halfDuration).SetEase(Ease.OutQuad);
			yield return fadeTween.WaitForCompletion();
			spriteRenderer.sprite = SpriteForState(nextState);
			fadeTween = spriteRenderer.DOFade(1f, halfDuration).SetEase(Ease.InQuad);
			yield return fadeTween.WaitForCompletion();
		}
		finally
		{
			isTransitioning = false;
		}
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if(locked) return;
		
		ChangeState(EServentState.Ready, false, 0.1f);
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
		else
			ChangeState(EServentState.Idle, false, 0.1f);

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

	public void SetCardData(ServantCardData cardData)
	{this.cardData = cardData;}


	public void SetField(Field field)
	{ this.field = field; }

	public Field GetField()
	{ return field; }

}