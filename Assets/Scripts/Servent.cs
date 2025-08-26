using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using System.Collections;
using UnityEngine.EventSystems;

public class Servent : MonoBehaviour
{
	private EServentType serventType;
	private EServentState serventState;

	private string serventName;
	private string serventOriginForce;
	private string serventAbility;

	private Sprite idle;
	private Sprite attack;
	private Sprite ready;
	private Sprite guard;
	private Sprite death;

	public Transform dragPoint;
	public int serventForce;

	public GameObject border;
	public GameObject infoWindow;
	public TMP_Text serventForceText;
	public Button activationButton;
	public SpriteRenderer spriteRenderer;

	public Color fadeColor;
	public int serventNum;
	private bool mouseOn;

	public Texture2D texture2D;
	private Material monsterMaterial;

	bool isDissolving = true;
	bool isDying = false;
	float fade = 0f;

	void Start()
	{
		monsterMaterial = spriteRenderer.GetComponent<SpriteRenderer>().material;
		monsterMaterial.SetTexture("_MainTex", texture2D);
		monsterMaterial.SetFloat("_Fade", fade);
		monsterMaterial.SetColor("_Color", fadeColor);
	}

	void Update()
	{
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

		if (isDying)
		{
			if (fade == 1f)
			{ BattleManager.Inst.PlayServentDeathSound(); }

			fade -= Time.deltaTime * 1.1f;

			if (fade <= 0.1f)
			{
				fade = 0f;
				isDying = false;
				BattleManager.Inst.ShotMissile(transform);
				Destroy(this.gameObject);
			}
			monsterMaterial.SetFloat("_Fade", fade);
		}
	}

	public void Dead()
	{isDying = true;}

	public void ChangeState(EServentState state)
	{
		switch(state)
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

	public void OnMouseUp()
	{
		if(mouseOn)
		{StartCoroutine(BattleManager.Inst.ShowServentInfo(this));}
	}

	public void OnMouseEnter()
	{mouseOn = true;}

	public void OnMouseExit()
	{mouseOn = false;}

	public void ShowInfo()
	{
		infoWindow.GetComponent<ServentInfoWindow>().OnOff(true);
		border.SetActive(true);
		activationButton.gameObject.SetActive(true);
	}
	public void CloseInfo()
	{
		infoWindow.GetComponent<ServentInfoWindow>().OnOff(false);
		border.SetActive(false);
		activationButton.gameObject.SetActive(false);
	}
	public int GetServentNum()
	{return serventNum;}
	public EServentType GetServentType()
	{return serventType;}
	public void SetServentType(EServentType serventType)
	{this.serventType = serventType;}
	public Transform GetDragPoint()
	{return dragPoint; }
}
