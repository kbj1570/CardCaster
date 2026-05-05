using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;



public class BattleManager : MonoBehaviour , ILockable
{
	Tween parryTween;
	public AudioSource backGroundMusic;
	public AudioSource soundEffect;
	public AudioClip serventDeath;
	public AudioClip serventSummon;
	public GameObject serventSelectAlert;

	Dictionary<string, int> cardHashMap;

	List<ItemData> reward;
	int rewardGold;

	EParryState parryState;
	bool playerDamageBlock;
	bool enemyDamageBlock;
	int playerDamageDecrease;
	int enemyDamageDecrease;
	int playerDamageIncrease;
	int enemyDamageIncrease;

	public Servant currentAttacker;
	public Servant selectedServant;
	public EServentAttribute targetServentAttribute;

	public Servant activatingServant;
	public Servant currentDefender;
	public int originalDefenderForce;


	List<Enemy> enemies = new();
	Enemy currentEnemy;
	int enemyIndex = 0;

	public GameObject runawayButton;
	public GameObject floatingTextPrefab;
	public Transform alertPoint; 
	public static BattleManager Inst{get; private set;}
	public Canvas canvas;
	public Camera camera;
	public Field field_1;
	public Field field_2;
	public Field field_3;
	public Field field_4;
	public Field field_5;
	public Field field_6;

	public List<GameObject> anyWhereAreas;
	public List<GameObject> playerServentPrefabList;
	public List<GameObject> playerServentInfoList;
	public List<GameObject> enemyServentPrefabList;
	public List<GameObject> enemyServentInfoList;
	public List<Sprite> cardImageList;
	public List<CardData> deckList;
	public List<CardData> trashList;
	public List<CardData> handList;

	public Transform cardAreaBorderLeft;
	public Transform cardAreaBorderRight;

	public EMouseOnArea mouseOnArea;
	private List<GameObject> cardObjectList;
	public LineRenderer cardDragLine;
	public LineRenderer attackDragLine;
	public int lineCount;
	public List<GameObject> conditionMarkList;

	public GameObject playerObject;
	public GameObject enemyObject;
	public GameObject hole;
	public GameObject battleWindowLeftSide;
	public GameObject battleWindowRightSide;
	public GameObject itemOrganizeWindow;

	public Transform battleWindowLeftSideFloatTextLocation;
	public Transform battleWindowRightSideFloatTextLocation;
	public Transform battleWindowLeftSideFirstPosition;
	public Transform battleWindowLeftSideSecondPosition;
	public Transform battleWindowRightSideFirstPosition;
	public Transform battleWindowRightSideSecondPosition;

	public GameObject missile;
	public GameObject missileTarget;
	public GameObject clickedServentInfo;

	public Servant clickedServent;

	public TMP_Text costCountText;
	public TMP_Text cardCountText;
	public TMP_Text playerHealthText;
	public TMP_Text enemyHealthText;

	private int costCount;
	private int deckCount;
	private int trashCount;

	private BattleState battleState = BattleState.Idle;

	public List<Servant> summonedServants;
	public List<CardData> selectedCards;
	public List<Servant> activatingServ;

	public GridLayoutGroup selectedCardLayoutGroup;
	public GridLayoutGroup trashLayoutGroup;
	public GameObject trashWindow;
	public Image fadeImage;
	public Image flashImage;
	public SpriteRenderer smallCircle;
	public SpriteRenderer bigCircle;

	public GameObject serventCardPrefab;
	public GameObject spellCardPrefab;
	public GameObject enemyCardPrefab;
	public GameObject fieldSpellCardPrefab;
	public GameObject alertMessage;
	public GameObject gameOverWindow;

	private int selectedLimit;
	public bool phaseFlag;
	public bool actionFlag;
	bool isParryWindowActive = false;

	float parryWindowTime = 0.3f;
	float circleSpeed = 1f;

	public int playerHealth;
	public int enemyHealth;

	private IEnumerator ShowBattleWindow(int attackerDamage, int defenderDamage)
	{
		foreach (Servant servent in summonedServants)
		{servent.HideForce();}

		foreach (GameObject card in cardObjectList)
		{card.GetComponent<Card>().SetLock(true);}

		battleWindowLeftSide.transform.DOMove(battleWindowLeftSideSecondPosition.position,
		0.2f).SetEase(Ease.Linear);
		battleWindowRightSide.transform.DOMove(battleWindowRightSideSecondPosition.position,
		0.2f).SetEase(Ease.Linear);

		if(defenderDamage != 0)
		{
			GameObject defenderDamageText = Instantiate(floatingTextPrefab);
			defenderDamageText.transform.SetParent(battleWindowRightSide.transform, false);
			defenderDamageText.GetComponent<FloatingDamageText>().SetDamageText(defenderDamage);
			defenderDamageText.GetComponent<FloatingDamageText>().SetFont(30);
		}

		if (attackerDamage != 0)
		{
			GameObject attackerDamageText = Instantiate(floatingTextPrefab);
			attackerDamageText.transform.SetParent(battleWindowLeftSide.transform, false);
			attackerDamageText.GetComponent<FloatingDamageText>().SetDamageText(attackerDamage);
			attackerDamageText.GetComponent<FloatingDamageText>().SetFont(30);
		}

		yield return new WaitForSeconds(0.2f);

		battleWindowLeftSide.transform.DOMove(battleWindowLeftSideSecondPosition.position + new Vector3(1.5f, 0, 0),
		1.5f).SetEase(Ease.OutExpo);
		battleWindowRightSide.transform.DOMove(battleWindowRightSideSecondPosition.position + new Vector3(-1.5f, 0, 0),
		1.5f).SetEase(Ease.OutExpo);
		yield return new WaitForSeconds(1.5f);

		battleWindowLeftSide.transform.DOMove(battleWindowLeftSideFirstPosition.position,
		0.2f).SetEase(Ease.InQuad);
		battleWindowRightSide.transform.DOMove(battleWindowRightSideFirstPosition.position,
		0.2f).SetEase(Ease.InQuad);

		yield return new WaitForSeconds(0.2f);

		foreach (Servant servent in summonedServants)
		{ servent.ShowForce(); }

		battleWindowLeftSide.GetComponent<BattleWindow>().ClearActor();
		battleWindowRightSide.GetComponent<BattleWindow>().ClearActor();

		foreach (GameObject card in cardObjectList)
		{ card.GetComponent<Card>().SetLock(false); }
	}

	public List<Field> GetPlayerFields()
	{ return new List<Field> { field_1, field_2, field_3 }; }
	public List<Field> GetEnemyFields()
	{ return new List<Field> { field_4, field_5, field_6 }; }
	public List<Field> GetAllFields()
	{ return new List<Field> { field_1, field_2, field_3, field_4, field_5, field_6 }; }

	public void FlashMultipleTimes()
	{
		int flashCount = 2;
		float flashDuration = 0.05f;

		flashImage.gameObject.SetActive(true);
		flashImage.color = new Color(1, 1, 1, 0);

		Sequence sequence = DOTween.Sequence();
		for (int i = 0; i < flashCount; i++)
		{
			sequence.Append(flashImage.DOFade(0.75f, flashDuration))
			.Append(flashImage.DOFade(0, flashDuration));
		}

		sequence.OnComplete(() => {flashImage.gameObject.SetActive(false);});
		sequence.Play();
	}

	private IEnumerator FadeIn() 
	{
		float time = 0;
		Color color = fadeImage.color;
		
		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(1, 0, time / 1f);
			fadeImage.color = color;
			yield return null;
		}
		fadeImage.gameObject.SetActive(false);
	}
	void Awake()
	{
		Inst = this;
		enemies = BattleData.enemies;
		GameSetup();

		reward = new();
		handList = new();
		selectedCards = new();
		summonedServants = new();
		mouseOnArea = EMouseOnArea.None;

		StartCoroutine(FadeIn());

		StartCoroutine(GameLoop());
	}

	public IEnumerator GameLoop()
	{
		yield return new WaitForSeconds(1f);
		while (true)
		{
			yield return PlayerTurn();
			yield return EnemyTurn();
		}
	}

	void ResetTurnState()
	{
		foreach (Servant servant in summonedServants)
			servant.ResetAttackCount();

		enemyDamageBlock = false;

		foreach (GameObject card in cardObjectList)
			card.GetComponent<Card>().HideAndReveal(false);
	}

	void GameSetup()
	{
		trashCount = 0;
		deckCount = 0;
		costCount = 0;


		if (PlayerData.saveData == null)
		{ PlayerData.saveData = DataController.Inst.LoadData(); }

		playerHealth = PlayerData.saveData.health;
		
		if(enemies == null)
		{
			enemies = new();
			enemies.Add(new UnknownMonster()); 
		}

		currentEnemy = enemies[enemyIndex];
		enemyHealth = currentEnemy.GetHealth();
		cardHashMap = DataController.Inst.LoadCardHashMap();

		Dictionary<CardData, int> deck = new Dictionary<CardData, int>();
		List<CardData> cardDatabase = DataController.Inst.LoadCardDatabase();
		Dictionary<string, int> myDeck = PlayerData.saveData.deck;

		if (DungeonData.dungeon != null)
        {
            battleWindowLeftSide.GetComponent<BattleWindow>().SetBackGround(DungeonData.dungeon.GetDungeonNum());
			battleWindowRightSide.GetComponent<BattleWindow>().SetBackGround(DungeonData.dungeon.GetDungeonNum());
        }



		foreach (KeyValuePair<string, int> value in myDeck)
		{
			deck.Add(cardDatabase[cardHashMap[value.Key]], value.Value);
		}

		deckList = new();
		cardObjectList = new();
		trashList = new();
		
		foreach(KeyValuePair<CardData, int> value in deck)
		{
			for(int i = 0; i < value.Value; ++i)
			{deckList.Add(value.Key);}
		};

		Shuffle();
	}

	private void Shuffle()
	{
		for(int i = 0; i < 100; ++i)
		{
			int a = Random.Range(0, deckList.Count);
			int b = Random.Range(0, deckList.Count);
			CardData c = deckList[a];
			deckList[a] = deckList[b];
			deckList[b] = c;
		}
	}
	public void UpdateCondition()
	{
		if (deckList == null) return;

		deckCount = deckList.Count;
		trashCount = trashList.Count;

		costCountText.text = "Cost: " + costCount.ToString();
		cardCountText.text = trashCount.ToString() + " / " + deckCount.ToString();
		playerHealthText.text = "Player HP: " + playerHealth.ToString();
		enemyHealthText.text = "Enemy HP: " + enemyHealth.ToString();

		if (playerHealth <= 0)
		{
			playerHealth = 0;
			StopAllCoroutines();
			StartCoroutine(GameOver());
		}
	}
	public bool AddSelectedCards(CardData cardData)
	{
		bool foo = selectedCards.Count < selectedLimit;

		if(foo)
		{selectedCards.Add(cardData);}

		return foo;
	}


	private IEnumerator FadeOut()
	{
		yield return new WaitForSeconds(0.5f);
		fadeImage.gameObject.SetActive(true);
		float time = 0;
		Color color = fadeImage.color;

		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(0, 1, time / 1f);
			fadeImage.color = color;
			yield return null;
		}
	}

	public void RemoveSelectedCards(CardData cardData)
	{

		selectedCards.Remove(cardData);
	}

	public void HealPlayer(int value)
	{
		playerHealth += value;
	}

	IEnumerator CheckEnemyCondition()
	{
		if(enemyHealth <= 0)
		{
			enemyHealth = 0;

			AlertMessage("적을 쓰러트렸습니다.");
			yield return new WaitForSeconds(1f);


			foreach(ItemData itemData in currentEnemy.GetReward())
			{
				reward.Add(itemData);
			}
			rewardGold += currentEnemy.GetGold();

			if (enemyIndex == enemies.Count - 1)
			{
				AlertMessage("전투에서 승리했습니다.");

				PlayerData.saveData.health = playerHealth;

				foreach (GameObject card in cardObjectList)
				{card.GetComponent<Card>().HideAndReveal(true);}

				yield return new WaitForSeconds(0.3f);

				StartCoroutine(EnemyFieldClear());
				yield return new WaitForSeconds(1f);

				itemOrganizeWindow.GetComponent<ItemOrganizeWindow>().SetItemList(reward);
				itemOrganizeWindow.GetComponent<ItemOrganizeWindow>().SetGold(rewardGold);
				itemOrganizeWindow.GetComponent<ItemOrganizeWindow>().OnOff();
				runawayButton.SetActive(true);
			}
			else
			{StartCoroutine(LoadNextEnemy());}
		}

	}

	void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			switch (battleState)
			{
				case BattleState.Idle: HandleIdleClick(); break;
				case BattleState.SelectingServent: HandleSelectServentClick(); break;
				case BattleState.EnemyTurn: break;
			}
		}

		UpdateCondition();

		// ⬇️ 패링 입력 처리
		if (Input.GetKeyDown(KeyCode.Space) && parryState == EParryState.Parry)
		{
			if (isParryWindowActive)
			{
				GameObject damageText = Instantiate(floatingTextPrefab, alertPoint);
				damageText.GetComponent<FloatingDamageText>().SetDamageText("Guard!!");
				damageText.GetComponent<FloatingDamageText>().SetFont(30);
				damageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);
				parryState = EParryState.Succecced;
			}
			else
			{
				parryState = EParryState.Failed;
			}

			// 진행 중인 패링 트윈/창 닫기
			if (parryTween != null && parryTween.IsActive()) parryTween.Kill();
			isParryWindowActive = false;
		}
	}
	IEnumerator StartParrySequence(float duration, float windowCenterNorm, float windowDuration)
	{
		// 초기화
		isParryWindowActive = false;
		parryState = EParryState.Parry;

		Vector3 targetScale = bigCircle.transform.localScale;
		smallCircle.transform.localScale = Vector3.zero;

		// 노멀라이즈드 윈도우 구간 계산
		float winHalf = Mathf.Clamp01((windowDuration / duration) * 0.5f);
		float winStart = Mathf.Clamp01(windowCenterNorm - winHalf);
		float winEnd   = Mathf.Clamp01(windowCenterNorm + winHalf);

		float u = 0f;

		// 진행 트윈: u를 0→1로 선형 진행하면서 스케일 보간, 윈도우 on/off 제어
		parryTween = DOTween.To(() => u, x =>
		{
			u = x;
			smallCircle.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, u);

			if (!isParryWindowActive && u >= winStart && u < winEnd) isParryWindowActive = true;
			if (isParryWindowActive && u >= winEnd)                 isParryWindowActive = false;

		}, 1f, duration)
		.SetEase(Ease.Linear)
		.OnKill(() =>
		{
			// 트윈이 중도 종료되든 자연 종료되든 원을 초기화
			smallCircle.transform.localScale = Vector3.zero;
			isParryWindowActive = false;
		});

		yield return parryTween.WaitForCompletion();

		// 플레이어 입력이 없었다면 실패 판정
		if (parryState == EParryState.Parry)
			parryState = EParryState.Failed;
	}

	public void StartParryWindow()
	{
		StartCoroutine(ParryWindowCoroutine());
	}

	void HandleIdleClick()
	{
		if (clickedServentInfo != null)
		{
			CloseServentInfo();
		}
	}

	void HandleSelectServentClick()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

		if (hit.collider != null)
		{
			GameObject selectedObject = hit.collider.gameObject;

			if (selectedObject.CompareTag("Servent"))
			{

				selectedServant = selectedObject.GetComponent<Servant>();
				if (targetServentAttribute == EServentAttribute.None)
				{
					actionFlag = true;
				}
				else if(selectedServant.GetAttribute() == targetServentAttribute)
				{
					actionFlag = true;
				}
				
			}
			else
			{Debug.Log("선택할 수 없는 대상입니다.");}
		}
	}

	private IEnumerator ParryWindowCoroutine()
	{
		isParryWindowActive = true;
		yield return new WaitForSeconds(parryWindowTime);
		isParryWindowActive = false;
	}

	public IEnumerator GameOver()
	{
		List<string> items = PlayerData.saveData.inventory_items;

		int itemCount = items.Count / 2;
		int randomNum = 0;

		for (int i = 0; i < itemCount; ++i)
		{
			randomNum = Random.Range(0, items.Count);
			items.RemoveAt(randomNum);
		}

		yield return null;
	}


	public GameObject ReturnConditionMark(EServentCondition value)
	{
		switch(value)
		{
			case EServentCondition.Void:
			return conditionMarkList[0];

			case EServentCondition.Oblivion:
			return conditionMarkList[1];
		}
		return null;
	}

	public void ShotMissile(Transform startPoint, Transform targetPoint)
	{
		GameObject bullet = Instantiate(missile, camera.ScreenToWorldPoint(startPoint.position), Utils.QI);
		BezierMissile missileScript = bullet.GetComponent<BezierMissile>();

		missileScript.masterPos = camera.ScreenToWorldPoint(startPoint.position);
		missileScript.enemyPos = targetPoint.position;
	}

	public void ShotDrawMissile(Transform targetPoint)
	{
		GameObject bullet = Instantiate(missile, hole.transform.position, Utils.QI);
		BezierMissile missileScript = bullet.GetComponent<BezierMissile>();

		missileScript.masterPos = hole.transform.position;
		missileScript.enemyPos = camera.ScreenToWorldPoint(targetPoint.position);
	}



	public void ShotMissile(Vector3 startPoint)
	{
		GameObject bullet = Instantiate(missile, startPoint, Utils.QI);
		BezierMissile missileScript = bullet.GetComponent<BezierMissile>();
		missileScript.masterPos = startPoint;
		missileScript.enemyPos = hole.transform.position;
	}

	public IEnumerator ShowServentInfo(Servant servent)
	{
		if(servent.GetServentType() == EServentType.Player)
		{clickedServentInfo = Instantiate(playerServentInfoList[0], Input.mousePosition, Utils.QI);}
		else if(servent.GetServentType() == EServentType.Enemy)
		{clickedServentInfo = Instantiate(playerServentInfoList[0], Input.mousePosition, Utils.QI);}
		
		Vector3 vector = clickedServentInfo.transform.position;
		vector.x += clickedServentInfo.GetComponent<RectTransform>().rect.width * 0.7f;
		clickedServentInfo.transform.position = vector;
		yield return new WaitForSeconds(0.1f);
		clickedServentInfo.GetComponent<ServentInfoWindow>().OnOff(true);
		clickedServentInfo.GetComponent<ServentInfoWindow>().UpdateCardData(servent);
		clickedServentInfo.transform.SetParent(canvas.transform);
		clickedServent = servent; 
	}
	public void CloseServentInfo()
	{
		// 클릭된 정보창 자체가 없으면 종료
		if (clickedServentInfo == null) return;

		var info = clickedServentInfo.GetComponent<ServentInfoWindow>();
		// 마우스가 정보창 위에 있으면 닫지 않음
		if (info != null && info.onMouse) return;

		// 선택된 소환수 참조 해제 및 창 파괴
		if (clickedServent != null)
		{
			Destroy(clickedServentInfo);
			clickedServentInfo = null;
			clickedServent = null;
		}
	}

	


	public void SetEnemyDamageBlock(bool value)
	{ this.enemyDamageBlock = value; }

	public IEnumerator DrawPhase()
	{
		/*
		 * 패가 5장이 될 때까지 드로우하고 드로우페이즈를 마친다.
		 * 만약 덱이 0장이라면 대신 묘지에서 카드를 드로우하고,
		 * 묘지에서 카드를 드로우 할 때마다 1 대미지를 받는다.
		 * 덱과 묘지가 모두 0장이라면 드로우를 진행하지 않고 드로우 페이즈를 마친다.
		*/
		if (handList.Count < 5)
		{
			int p = 5 - handList.Count;
			for (int i = 0; i < p; ++i)
			{
				yield return new WaitForSeconds(0.5f);
				DrawCard();
			}
		}
		else
		{ DrawCard(); }
		yield return new WaitForSeconds(1f);
		
	}
	public IEnumerator StandByPhase()
	{
		/*
		 * 턴 시작시 효과들을 처리하는 단계
		 * 필드 효과, 소환수에게 걸려있는 상태 효과, 소환수 효과, 마법 효과 순서대로 처리된다.
		 * 처리할 효과가 없거나 효과를 모두 처리하면 스탠바이 페이즈를 마친다
		 */

		//foreach (var card in activatingCards)
		//{
		//	yield return StartCoroutine(card.GetCardData().EndPhaseEffectExecute(this));
		//}
		yield return new WaitForSeconds(1f);
	}

	public IEnumerator MainPhase()
	{
		/* 플레이어가 행동하는 단계
		 * 행동을 모두 마치고 메인 페이즈를 마친다.
		 * 행동이란 소환, 마법 사용, 공격, 패 버리기 등을 포함한다.
		 * 행동을 모두 마쳤다면 메인 페이즈를 마친다.
		 */


		foreach (GameObject card in cardObjectList)
		{ card.GetComponent<Card>().SetLock(false); }

		yield return new WaitUntil(() => phaseFlag);
	}
	public IEnumerator EndPhase()
	{
		/*
		 * 턴 종료시 효과들을 처리하는 단계
		 * 필드 효과, 소환수에게 걸려있는 상태 효과, 소환수 효과, 마법 효과 순서대로 처리된다.
		 */
		//foreach (var card in activatingCards)
		//{
		//	yield return StartCoroutine(card.GetCardData().EndPhaseEffectExecute(this));
		//}
		foreach (GameObject card in cardObjectList)
		{ card.GetComponent<Card>().SetLock(true); }

		foreach (GameObject card in cardObjectList)
		{ card.GetComponent<Card>().HideAndReveal(true); }
		yield return new WaitForSeconds(1f);
	}


	IEnumerator PlayerTurn()
	{
		phaseFlag = false;

		ResetTurnState();
		yield return new WaitForSeconds(0.4f);
		yield return DrawPhase();
		yield return StandByPhase();
		yield return MainPhase();
		yield return EndPhase();
	}

	IEnumerator EnemyTurn()
{
    yield return new WaitForSeconds(0.3f);

    int tokens = currentEnemy.GetActionToken();
    for (int t = 0; t < tokens; t++)
    {
        // 매 토큰마다 보드 상태를 다시 평가
        var action = SelectEnemyAction_V2();

        switch (action)
        {
            case EEnemyAction.Summon:
                yield return TryEnemySummon();
                break;

            case EEnemyAction.Attack:
                yield return TryEnemyAttack();
                break;

            case EEnemyAction.ServentAbility:
                yield return TryUseServentAbility();
                break;

            case EEnemyAction.EnemyAbility:
                yield return TryUseEnemyAbility();
                break;

            case EEnemyAction.None:
                AlertMessage("적이 할 수 있는 행동이 없습니다.");
                yield return new WaitForSeconds(0.5f);
                break;
        }

        // 매 액션 사이 약간의 텀
        yield return new WaitForSeconds(0.6f);
    }
}
















	// IEnumerator EnemyTurn()
	// {
	// 	yield return new WaitForSeconds(0.3f);
	// 	int actionToken = currentEnemy.GetActionToken();

	// 	for (int i = 0; i < actionToken; ++i)
	// 	{
	// 		List<Field> filledField = new();
	// 		List<Field> emptyField = new();
	// 		List<Servent> attackableServent = new();

	// 		Field[] enemyFields = { field_4, field_5, field_6 };

	// 		for (int idx = 0; idx < enemyFields.Length; idx++)
	// 		{
	// 			if (enemyFields[idx].IsFilled())
	// 			{
	// 				filledField.Add(enemyFields[idx]);
	// 				if (!enemyFields[idx].GetServent().IsAttackable())
	// 					attackableServent.Add(enemyFields[idx].GetServent());

	// 			}
	// 			else
	// 			{ emptyField.Add(enemyFields[idx]); }
	// 		}

	// 		EEnemyAction action = SelectEnemyAction(emptyField.Count, attackableServent.Count);

	// 		switch (action)
	// 		{
	// 			case EEnemyAction.Summon:
	// 				if (emptyField.Count > 0)
	// 				{

	// 					Field field = emptyField[Random.Range(0, emptyField.Count)];
	// 					field.locked = true;

	// 					List<EnemyServentCardData> serventList = currentEnemy.GetServentDeck();
	// 					EnemyServentCardData randomServent = serventList[Random.Range(0, serventList.Count)];

	// 					yield return ShowEnemyActionCard(randomServent, field.transform);

	// 					GameObject serventObject =
	// 						Instantiate(enemyServentPrefabList[cardHashMap[randomServent.GetCardNum()]], field.transform.position, Utils.QI);
	// 					field.Summon(serventObject.GetComponent<Servent>(), randomServent);
	// 					serventObject.GetComponent<Servent>().InitWithEffect();
	// 					summonedServents.Add(serventObject.GetComponent<Servent>());

	// 					soundEffect.PlayOneShot(serventSummon);
	// 					field.locked = false;
	// 				}
	// 				break;

	// 			case EEnemyAction.Attack:
	// 				if (attackableServent.Count > 0)
	// 				{
	// 					List<EMouseOnArea> playerTargets = new List<EMouseOnArea> { EMouseOnArea.Player };
	// 					if (field_1.IsFilled()) playerTargets.Add(EMouseOnArea.Field_1);
	// 					if (field_2.IsFilled()) playerTargets.Add(EMouseOnArea.Field_2);
	// 					if (field_3.IsFilled()) playerTargets.Add(EMouseOnArea.Field_3);

	// 					Servent attacker = attackableServent[Random.Range(0, attackableServent.Count)];
	// 					EMouseOnArea targetField = playerTargets[Random.Range(0, playerTargets.Count)];

	// 					if (targetField == EMouseOnArea.Player)
	// 					{
	// 						yield return EnemyAttackPlayer(attacker, playerObject);
	// 					}
	// 					else
	// 					{
	// 						Servent defender = null;
	// 						switch (targetField)
	// 						{
	// 							case EMouseOnArea.Field_1:
	// 								{ defender = field_1.GetServent(); }
	// 								break;
	// 							case EMouseOnArea.Field_2:
	// 								{ defender = field_2.GetServent(); ; }
	// 								break;
	// 							case EMouseOnArea.Field_3:
	// 								{ defender = field_3.GetServent(); }
	// 								break;
	// 						}

	// 						yield return EnemyAttackServent(attacker, defender);
	// 					}


	// 				}
	// 				break;

	// 			case EEnemyAction.None:
	// 				AlertMessage("적이 아무것도 할 수 없습니다.");
	// 				break;
	// 		}
	// 		yield return new WaitForSeconds(2.5f);
	// 	}
	// }
	



EEnemyAction SelectEnemyAction_V2()
{
    // 적/아군 필드 스캔
    var enemyFields = new[] { field_4, field_5, field_6 };
    var playerFields = new[] { field_1, field_2, field_3 };



    List<Field> emptyEnemy = new();
    List<Servant> attackable = new();
    List<Servant> activatable = new();

    foreach (var f in enemyFields)
    {
        if (!f.IsFilled()) emptyEnemy.Add(f);
        else
        {
            var s = f.GetServent();
            if (s != null)
            {
                if (s.IsAttackable()) attackable.Add(s); // ✅ 공격 가능만 수집
                if (s.IsActivationable() && s.GetCardData().IsCardUsable(this))
                    activatable.Add(s);
            }
        }
    }

    bool canSummon = emptyEnemy.Count > 0 && currentEnemy.GetServentDeck() != null && currentEnemy.GetServentDeck().Count > 0;
    bool canAttack = attackable.Count > 0; // 플레이어/소환수 중 하나는 항상 타깃 가능(필드/플레이어 HP 따라)
    bool canServentAbility = activatable.Count > 0;
    bool canEnemyAbility = true; // 규칙이 정해지면 여기서 조건 체크

    // 가중치 튜닝 포인트
    int wSummon = canSummon ? emptyEnemy.Count * 3 : 0;
    int wAttack = canAttack ? attackable.Count * 2 : 0;
    int wServentAbility = canServentAbility ? activatable.Count * 2 : 0;
    int wEnemyAbility = canEnemyAbility ? 2 : 0;

    // 플레이어가 위기 상황이면 공격 성향 강화
    if (playerHealth <= 10) wAttack = Mathf.CeilToInt(wAttack * 1.5f);

    // 초반 전개 시 소환 우대
    if (emptyEnemy.Count >= 2) wSummon *= 2;

    int total = wSummon + wAttack + wServentAbility + wEnemyAbility;
    if (total <= 0) return EEnemyAction.None;

    int roll = Random.Range(0, total);
    if ((roll -= wSummon) < 0) return EEnemyAction.Summon;
    if ((roll -= wAttack) < 0) return EEnemyAction.Attack;
    if ((roll -= wServentAbility) < 0) return EEnemyAction.ServentAbility;
    return EEnemyAction.EnemyAbility;
}
IEnumerator TryEnemySummon()
{
    var enemyFields = new[] { field_4, field_5, field_6 };
    List<Field> empty = new();
    foreach (var f in enemyFields) if (!f.IsFilled()) empty.Add(f);

    var deck = currentEnemy.GetServentDeck();
    if (empty.Count == 0 || deck == null || deck.Count == 0) yield break;

    Field field = empty[Random.Range(0, empty.Count)];
    field.locked = true;

    EnemyServentCardData pick = deck[Random.Range(0, deck.Count)];

    // 연출 카드 보여주기
    yield return ShowEnemyActionCard(pick, field.transform);

    // 소환
    GameObject obj = Instantiate(
        enemyServentPrefabList[cardHashMap[pick.GetCardNum()]],
        field.transform.position, Utils.QI);

    field.Summon(obj.GetComponent<Servant>(), pick);
    obj.GetComponent<Servant>().InitWithEffect();
    summonedServants.Add(obj.GetComponent<Servant>());

    soundEffect.PlayOneShot(serventSummon);
    field.locked = false;

    // 혹시 즉발 알림/패시브 반응이 있으면 호출
    yield return StartCoroutine(NotifyServentSummon(obj.GetComponent<Servant>()));
    yield return StartCoroutine(CheckServentsCondition());
}

IEnumerator TryEnemyAttack()
{
    var enemyFields = new[] { field_4, field_5, field_6 };
    List<Servant> canAtk = new();
    foreach (var f in enemyFields)
        if (f.IsFilled() && f.GetServent().IsAttackable())
            canAtk.Add(f.GetServent());

    if (canAtk.Count == 0) yield break;

    Servant attacker = canAtk[Random.Range(0, canAtk.Count)];

    // 타깃 후보: 플레이어 + 존재하는 아군 소환수들
    List<EMouseOnArea> targets = new() { EMouseOnArea.Player };
    if (field_1.IsFilled()) targets.Add(EMouseOnArea.Field_1);
    if (field_2.IsFilled()) targets.Add(EMouseOnArea.Field_2);
    if (field_3.IsFilled()) targets.Add(EMouseOnArea.Field_3);

    var pick = targets[Random.Range(0, targets.Count)];

    if (pick == EMouseOnArea.Player)
    {
        yield return EnemyAttackPlayer(attacker, playerObject);
    }
    else
    {
        Servant defender = null;
        switch (pick)
        {
            case EMouseOnArea.Field_1: defender = field_1.GetServent(); break;
            case EMouseOnArea.Field_2: defender = field_2.GetServent(); break;
            case EMouseOnArea.Field_3: defender = field_3.GetServent(); break;
        }
        if (defender != null)
            yield return EnemyAttackServant(attacker, defender);
    }

    // 공격 후 조건 체크
    yield return StartCoroutine(CheckServentsCondition());
}

IEnumerator TryUseServentAbility()
{
    var enemyFields = new[] { field_4, field_5, field_6 };
    List<Servant> candidates = new();

    foreach (var f in enemyFields)
    {
        if (!f.IsFilled()) continue;
        var s = f.GetServent();
        if (s != null && s.IsActivationable() && s.GetCardData().IsCardUsable(this))
            candidates.Add(s);
    }

    if (candidates.Count == 0) yield break;

    Servant caster = candidates[Random.Range(0, candidates.Count)];

    // 연출 카드(적 행동 카드)로 "능력 사용" 표기하고 싶다면 아래 사용 가능
    // yield return ShowEnemyActionCard("적 소환수 능력", caster.GetCardData().GetCardName());

    activatingServant = caster;
    caster.AddActivationCount();
    yield return StartCoroutine(caster.GetCardData().ActivationEffectExecute(this));
    activatingServant = null;

    yield return StartCoroutine(CheckServentsCondition());
}


IEnumerator TryUseEnemyAbility()
{
    // UI로 보여주고 싶으면 사용
    // yield return ShowEnemyActionCard("적의 능력", currentEnemy.GetName() + "이(가) 기술을 사용했다.");

    yield return StartCoroutine(currentEnemy.EffectExecute(this));
    yield return StartCoroutine(CheckServentsCondition());
}




























	public void SelectServentOnField(EServentAttribute targetServentAttribute)
	{
		this.targetServentAttribute = targetServentAttribute;
		StartCoroutine(SelectServentOnFieldCo());
	}

	public IEnumerator SelectServentOnFieldCo()
	{
		battleState = BattleState.SelectingServent;
		actionFlag = false;
		serventSelectAlert.SetActive(true);
		yield return new WaitUntil(() => actionFlag);
		serventSelectAlert.SetActive(false);
		battleState = BattleState.Idle;
	}

	public void StartEnemyTurn()
	{
		phaseFlag = true;
	}
	public IEnumerator WinBattle()
	{
		yield return new WaitForSeconds(1f);
	}

	IEnumerator LoadNextEnemy()
	{
		yield return new WaitForSeconds(0.3f);

		StartCoroutine(EnemyFieldClear());

		yield return new WaitForSeconds(1f);
		enemyIndex++;
		currentEnemy = enemies[enemyIndex];
		enemyHealth = currentEnemy.GetHealth();

		AlertMessage("새로운 적이 나타났습니다.");
	}

	public void AlertMessage(string message)
	{
		GameObject onMessage = Instantiate(alertMessage, alertPoint);
		onMessage.GetComponent<AlertMessage>().SetText(message);
		StartCoroutine(onMessage.GetComponent<AlertMessage>().FadeAway());
	}



	
	public IEnumerator EnemyFieldClear()
	{
		if(field_4.IsFilled())
			field_4.GetServent().SetForce(0);

		if(field_5.IsFilled())
			field_5.GetServent().SetForce(0);

		if (field_6.IsFilled())
			field_6.GetServent().SetForce(0);



		yield return new WaitForSeconds(0.3f);
	}

	public IEnumerator InstantiateCardFromServent(CardData battleCardData)
	{
		GameObject selectedCardPrefab = null;

		switch (battleCardData.GetCardType())
		{
			case ECardType.Servent:
				selectedCardPrefab = serventCardPrefab;
				break;
			case ECardType.Spell:
				selectedCardPrefab = spellCardPrefab;
				break;

		}
		GameObject cardObject = Instantiate(selectedCardPrefab, Vector3.zero, Utils.QI);

		if (battleCardData.statusConditions != null)
			foreach (EStatusCondition status in battleCardData.statusConditions)
			{ cardObject.GetComponent<Card>().AddStatusCondition(status); }

		cardObject.GetComponent<Card>().InitiateActionInBattle();

		cardObject.GetComponent<Card>().Init(
			(card, eventData) =>
			{
				if (card.locked)
				{ return; }

				if (eventData.button == PointerEventData.InputButton.Right)
				{ DiscardCard(card); }
			}
			,
			(card, eventData) => {

				if (card.locked)
				{ return; }

				CardBeginDrag(card.gameObject);
			},
			(card, eventData) => {

				if (card.locked)
				{ return; }
				card.transform.localScale = new Vector3(0.4f, 0.4f, 1);
				card.transform.position = card.originPRS.pos;
				CardOnDrag(card.gameObject);
			},
			(card, eventData) => {
				if (card.locked)
				{ return; }
				StartCoroutine(CardEndDrag(card, ReturnMouseOnField()));
			}
			,
			(card, eventData) => {
				if (card.locked)
				{ return; }

				if (card.currentSequence != null && card.currentSequence.IsActive())
					card.currentSequence.Kill();


				card.currentSequence = DOTween.Sequence()
					.Append(card.transform.DOScale(new Vector3(0.7f, 0.7f, 1), 0.13f).SetEase(Ease.InOutQuad))
					.Append(card.transform.DOMoveY(card.originPRS.pos.y + 130, 0.13f).SetEase(Ease.OutCirc));
			}
			,
			(card, eventData) => {
				if (card.locked)
				{ return; }

				if (card.currentSequence != null && card.currentSequence.IsActive())
					card.currentSequence.Kill();

				card.currentSequence = DOTween.Sequence()
					.Append(card.transform.DOScale(new Vector3(0.4f, 0.4f, 1), 0.07f).SetEase(Ease.InOutQuad))
					.Append(card.transform.DOMove(card.originPRS.pos, 0.07f).SetEase(Ease.OutCirc));
			}
		);

		cardObject.transform.SetParent(canvas.transform);
		cardObjectList.Add(cardObject);

		cardObject.GetComponent<Card>().SetCard(battleCardData, cardImageList[cardHashMap[battleCardData.GetCardNum()]]);

		cardObject.GetComponent<Card>().SetCardOrder(handList.Count);
		handList.Add(battleCardData);
		CardAlignmentAlt();

		GameObject bullet = Instantiate(missile, clickedServent.transform.position, Utils.QI);
		BezierMissile missileScript = bullet.GetComponent<BezierMissile>();

		missileScript.masterPos = clickedServent.transform.position;
		missileScript.enemyPos = camera.ScreenToWorldPoint(cardObject.transform.position);
		yield return new WaitForSeconds(0.5f);
	}

	public GameObject InstantiateCard(CardData battleCardData)
	{
		GameObject selectedCardPrefab = null;

		switch (battleCardData.GetCardType())
		{
			case ECardType.Servent: selectedCardPrefab = serventCardPrefab; break;
			case ECardType.Spell:   selectedCardPrefab = spellCardPrefab;  break;
		}

		GameObject cardObject = Instantiate(selectedCardPrefab, Vector3.zero, Utils.QI);

		if (battleCardData.statusConditions != null)
			foreach (EStatusCondition status in battleCardData.statusConditions)
				cardObject.GetComponent<Card>().AddStatusCondition(status);

		cardObject.GetComponent<Card>().InitiateActionInBattle();

		cardObject.GetComponent<Card>().Init(
			(card, eventData) =>
			{
				if (card.locked) return;
				if (eventData.button == PointerEventData.InputButton.Right)
					DiscardCard(card);
			},
			(card, eventData) =>
			{
				if (card.locked) return;
				CardBeginDrag(card.gameObject);
			},
			(card, eventData) =>
			{
				if (card.locked) return;
				card.transform.localScale = new Vector3(0.4f, 0.4f, 1);
				card.transform.position = card.originPRS.pos;
				CardOnDrag(card.gameObject);
			},
			(card, eventData) =>
			{
				if (card.locked) return;
				StartCoroutine(CardEndDrag(card, ReturnMouseOnField()));
			},
			(card, eventData) =>
			{
				if (card.locked) return;
				if (card.currentSequence != null && card.currentSequence.IsActive())
					card.currentSequence.Kill();

				card.currentSequence = DOTween.Sequence()
					.Append(card.transform.DOScale(new Vector3(0.7f, 0.7f, 1), 0.13f).SetEase(Ease.InOutQuad))
					.Append(card.transform.DOMoveY(card.originPRS.pos.y + 130, 0.13f).SetEase(Ease.OutCirc));
			},
			(card, eventData) =>
			{
				if (card.locked) return;
				if (card.currentSequence != null && card.currentSequence.IsActive())
					card.currentSequence.Kill();

				card.currentSequence = DOTween.Sequence()
					.Append(card.transform.DOScale(new Vector3(0.4f, 0.4f, 1), 0.07f).SetEase(Ease.InOutQuad))
					.Append(card.transform.DOMove(card.originPRS.pos, 0.07f).SetEase(Ease.OutCirc));
			}
		);

		cardObject.transform.SetParent(canvas.transform);
		cardObjectList.Add(cardObject);

		cardObject.GetComponent<Card>().SetCard(battleCardData, cardImageList[cardHashMap[battleCardData.GetCardNum()]]);

		cardObject.GetComponent<Card>().SetCardOrder(handList.Count);
		handList.Add(battleCardData);
		CardAlignmentAlt();

		ShotDrawMissile(cardObject.transform);
		return cardObject;
	}

	public void SearchCardInDeck(CardData targetCardData)
	{

		var cardDataToRemove = deckList.Find(cardData => cardData.GetCardNum() == targetCardData.GetCardNum());
		if (cardDataToRemove != null)
		{
			deckList.Remove(cardDataToRemove);
			InstantiateCard(targetCardData);
		}
	}

	private IEnumerator ShowActivatingCard(ECardType cardType , CardData cardData)
	{
		GameObject cardObject = new();
		switch (cardType)
		{
			case ECardType.Servent:
				cardObject = Instantiate(serventCardPrefab, camera.WorldToScreenPoint(enemyObject.transform.position), Utils.QI);
				break;
			case ECardType.Spell:
				cardObject = Instantiate(spellCardPrefab, camera.WorldToScreenPoint(enemyObject.transform.position), Utils.QI);
				break;
			case ECardType.Field:
				cardObject = Instantiate(fieldSpellCardPrefab, camera.WorldToScreenPoint(enemyObject.transform.position), Utils.QI);
				break;
			case ECardType.Enemy:
				cardObject = Instantiate(fieldSpellCardPrefab, camera.WorldToScreenPoint(enemyObject.transform.position), Utils.QI);
				cardObject.GetComponent<Card>().SetEnemyActionCard(cardData as EnemyServentCardData);
				break;
		}
		cardObject.transform.SetParent(canvas.transform);
		cardObject.GetComponent<Card>().InitiateActionInBattle();
		yield return new WaitForSeconds(0.3f);
	}


	private EEnemyAction SelectEnemyAction(int emptyCount, int attackableCount)
	{
		int summonWeight = (emptyCount > 0) ? emptyCount * 3 : 0;
		int attackWeight = (attackableCount > 0) ? attackableCount * 2 : 0;

		if (playerHealth <= 10) attackWeight = Mathf.CeilToInt(attackWeight * 1.5f);
		if (emptyCount >= 2) summonWeight *= 2;

		int totalWeight = summonWeight + attackWeight;
		if (totalWeight == 0) return EEnemyAction.None;

		int roll = Random.Range(0, totalWeight);

		if (roll < summonWeight) return EEnemyAction.Summon;
		roll -= summonWeight;

		if (roll < attackWeight) return EEnemyAction.Attack;
		roll -= attackWeight;

		return EEnemyAction.EnemyAbility;
	}

	IEnumerator EnemyAttackServant(Servant attacker, Servant defender)
	{
		StartCoroutine(DrawAttackLine(attacker.transform.position, defender.transform.position, circleSpeed));
		yield return StartCoroutine(StartParrySequence(circleSpeed, 0.80f, parryWindowTime));

		var damage = CalculateDamage(attacker, defender);

		GameObject leftActor = Instantiate(
				playerServentPrefabList[cardHashMap[defender.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);

		GameObject rightActor = Instantiate(
				enemyServentPrefabList[cardHashMap[attacker.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);


		battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(leftActor);
		leftActor.GetComponent<Servant>().OnBattleWindow();

		battleWindowRightSide.GetComponent<BattleWindow>().SetActor(rightActor);
		rightActor.GetComponent<Servant>().OnBattleWindow();



		ApplyDamage(attacker, defender, damage);
		attackDragLine.positionCount = 0;
		
		yield return new WaitForSeconds(2f);


		Color originColor = bigCircle.color;
		Color targetColor = new Color(1f, 0.3f, 0.3f, 0.5f);

		if (parryState == EParryState.Succecced)
		{ targetColor = new Color(0.3f, 0.3f, 1f, 0.5f); }



		bigCircle.DOColor(targetColor, 0.1f)
					 .SetLoops(3, LoopType.Yoyo)
					 .OnComplete(() => bigCircle.color = originColor);

		parryState = EParryState.Idle;

		yield return new WaitForSeconds(2f);
		yield return StartCoroutine(CheckEnemyCondition());
		attacker.AddAttackCount();
		yield return null;
	}
	void ApplyDamage(Servant attacker, Servant defender, DamageResult damage)
	{
		attacker.TakeDamage(damage.attackerDamage);
		defender.TakeDamage(damage.defenderDamage);
	}

	DamageResult CalculateDamage(Servant attacker, Servant defender)
	{
		int attackerForce = attacker.GetForce();
		int defenderForce = defender.GetForce();

		int attackerDamage = Math.Abs(defenderForce);
		int defenderDamage = Math.Abs(attackerForce);

		if (parryState == EParryState.Succecced)
			defenderDamage -= 1;

		if (attackerForce < 0)
			defenderDamage = 0;

		return new DamageResult(attackerDamage, defenderDamage);
	}
		

	IEnumerator EnemyAttackPlayer(Servant attacker, GameObject defender)
	{
		StartCoroutine(DrawAttackLine(attacker.transform.position, defender.transform.position, circleSpeed));
		yield return StartCoroutine(StartParrySequence(circleSpeed, 0.80f, parryWindowTime));
		
		int attackerForce = attacker.GetForce();

		attackerForce += playerDamageIncrease;
		attackerForce -= playerDamageDecrease;

		if (parryState == EParryState.Succecced)
		{ attackerForce -= 1; }

		if (attackerForce < 0)
		{ attackerForce = 0; }

		if (playerDamageBlock)
		{ attackerForce = 0; }

		GameObject leftActor = Instantiate(playerObject, new Vector3(), Utils.QI);

		GameObject rightActor = Instantiate(
				enemyServentPrefabList[cardHashMap[attacker.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);

		battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(leftActor);
		leftActor.GetComponent<SpriteRenderer>().sortingOrder = 104;
		battleWindowRightSide.GetComponent<BattleWindow>().SetActor(rightActor);
		rightActor.GetComponent<Servant>().OnBattleWindow();
		PlayerTakeAttack(attackerForce, parryState == EParryState.Succecced);


		attackDragLine.positionCount = 0;


		Color originColor = bigCircle.color;
		Color targetColor = new Color(1f, 0.3f, 0.3f, 0.5f);

		if (parryState == EParryState.Succecced)
		{ targetColor = new Color(0.3f, 0.3f, 1f, 0.5f); }



		bigCircle.DOColor(targetColor, 0.1f)
					 .SetLoops(3, LoopType.Yoyo)
					 .OnComplete(() => bigCircle.color = originColor);

		parryState = EParryState.Idle;

		yield return new WaitForSeconds(2f);

		yield return StartCoroutine(CheckEnemyCondition());
		attacker.AddAttackCount();
		yield return null;
	}

	public Vector3 ReturnMouseOnPosition()
	{
		switch (mouseOnArea)
		{
			case EMouseOnArea.Field_1:
				return field_1.gameObject.transform.position;

			case EMouseOnArea.Field_2:
				return field_2.gameObject.transform.position;

			case EMouseOnArea.Field_3:
				return field_3.gameObject.transform.position;

			case EMouseOnArea.Field_4:
				return field_4.gameObject.transform.position;

			case EMouseOnArea.Field_5:
				return field_5.gameObject.transform.position;

			case EMouseOnArea.Field_6:
				return field_6.gameObject.transform.position;

			case EMouseOnArea.Enemy:
				return enemyObject.transform.position;

			case EMouseOnArea.Player:
				return playerObject.transform.position;

			case EMouseOnArea.AnyWhere:
				return camera.ScreenToWorldPoint(Input.mousePosition);

			default:
				return camera.ScreenToWorldPoint(Input.mousePosition); 
		}
	}


	public Field ReturnMouseOnField()
	{
		switch(mouseOnArea)
		{
			case EMouseOnArea.Field_1:
			return field_1;

			case EMouseOnArea.Field_2:
			return field_2;

			case EMouseOnArea.Field_3:
			return field_3;

			case EMouseOnArea.Field_4:
			return field_4;

			case EMouseOnArea.Field_5:
			return field_5;

			case EMouseOnArea.Field_6:
			return field_6;

			default:
			return null;
		}
	}

	public IEnumerator ActivateCardDesc(CardData battleCardData, Transform target, int stackIndex)
	{
		GameObject cardObject = Instantiate(serventCardPrefab, alertPoint.position, Utils.QI);
		cardObject.transform.localScale = Vector3.zero;
		cardObject.transform.SetParent(canvas.transform);

		cardObject.GetComponent<Card>().SetCard(battleCardData, cardImageList[cardHashMap[battleCardData.GetCardNum()]]);
		Sequence seq = DOTween.Sequence();
		seq.Append(cardObject.transform.DOScale(new Vector3(0.7f, 0.7f, 1), 0.5f).SetEase(Ease.InOutQuad))
		.Append(cardObject.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack))
		.AppendCallback(() => phaseFlag = true);

		yield return new WaitForSeconds(0.3f);
	}

	public bool CheckCardUsable()
	{
		if (mouseOnArea == EMouseOnArea.Hole)
		{ return true; }

		if (ReturnMouseOnField() == null)
		{ return false; }

		return false;
	}
	public bool CheckCardUsable(CardData cardData, int currentCost, Field targetField)
	{
		if(mouseOnArea == EMouseOnArea.Hole)
		{return true;}

		if(currentCost != 0)
		{return false;}

		if(targetField == null)
		{return false;}

		return true;
	}

	public void CardBeginDrag(GameObject cardObject)
	{
		if(cardObject.GetComponent<Card>().GetCardData().GetCardTargetType() == ECardTargetType.NoneTargeting)
		{
			foreach(GameObject gameObject in anyWhereAreas)
			{gameObject.SetActive(true);}
		}

		foreach(GameObject card in cardObjectList)
		{card.GetComponent<Card>().SetLock(true);}

		cardObject.GetComponent<Card>().SetLock(false);
	}

	public void CardOnDrag(GameObject cardObject)
	{
		if(cardObject.GetComponent<Card>().GetCardData().GetCardType() == ECardType.Servent)
		{
			DrawDragLine(cardObject.transform.position,
			CheckServentSummonable(cardObject.GetComponent<Card>().GetCardData(),
			cardObject.GetComponent<Card>().GetCurrentCost(),ReturnMouseOnField()));
		}
		else
		{
			SpellCardData spellCardData = cardObject.GetComponent<Card>().GetCardData() as SpellCardData;
			DrawDragLine(cardObject.transform.position, spellCardData.IsCardUsable(this) && CheckCardUsable());
		}
	}


	public bool CheckServentSummonable(CardData cardData, int currentCost, Field targetField)
	{
		if(mouseOnArea == EMouseOnArea.Hole)
		{return true;}

		if(currentCost != 0)
		{return false;}

		if(mouseOnArea == EMouseOnArea.Field_4)
		{return false;}

		if(mouseOnArea == EMouseOnArea.Field_5)
		{return false;}

		if(mouseOnArea == EMouseOnArea.Field_6)
		{return false;}

		if (mouseOnArea == EMouseOnArea.Player)
		{ return false; }

		if (mouseOnArea == EMouseOnArea.Enemy)
		{ return false; }

		if (targetField == null)
		{ return false; }

		if (targetField.locked)
		{ return false; }

		if (targetField.IsFilled())
		{return false;}

		return true;
	}

	public void PlayServentDeathSound()
	{soundEffect.PlayOneShot(serventDeath);}

	public bool CheckAttackable(Servant servent)
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

		if (hit.collider == null)
			return false;

		GameObject targetObject = hit.collider.gameObject;
		if (!targetObject.CompareTag("Servent") && !targetObject.CompareTag("Enemy"))
		{ return false; }

		if (targetObject.CompareTag("Servent") && targetObject.GetComponent<Servant>().Equals(servent))
		{ return false; }

		return servent.IsAttackable();
	}

	private IEnumerator ShowEnemyActionCard(EnemyServentCardData enemyServentCardData, Transform target)
	{
		GameObject cardObject = Instantiate(enemyCardPrefab, camera.ScreenToWorldPoint(enemyObject.transform.position), Utils.QI);
		cardObject.transform.SetParent(canvas.transform);
		cardObject.GetComponent<Card>().SetEnemyActionCard(enemyServentCardData);
		cardObject.GetComponent<Card>().InitiateActionInBattle();
		yield return new WaitForSeconds(0.3f);
		cardObject.GetComponent<Card>().SendMissile(alertPoint, target);
		yield return new WaitForSeconds(1.8f);
	}

	private IEnumerator ShowEnemyActionCard(string ablityName, string abilityDesc)
	{
		GameObject cardObject = Instantiate(enemyCardPrefab, camera.ScreenToWorldPoint(enemyObject.transform.position), Utils.QI);
		cardObject.transform.SetParent(canvas.transform);
		cardObject.GetComponent<Card>().SetEnemyActionCard(ablityName, abilityDesc);
		cardObject.GetComponent<Card>().InitiateActionInBattle();
		yield return new WaitForSeconds(0.3f);
		cardObject.GetComponent<Card>().SendMissile(alertPoint, hole.transform);
		yield return new WaitForSeconds(1.8f);
	}

	public void DiscardCard(Card card)
	{
		handList.RemoveAt(card.GetCardOrder());
		cardObjectList.Remove(card.gameObject);
		AddTrash(card.GetCardData());
		card.SendMissile(alertPoint, hole.transform);
		costCount++;

		List<CardData> newHandList = new List<CardData>();

		foreach (CardData cardData in handList)
		{ newHandList.Add(cardData); }

		for (int i = 0; i < cardObjectList.Count; ++i)
		{ cardObjectList[i].GetComponent<Card>().SetCardOrder(i); }

		handList = newHandList;
		CardAlignment();
	}


	public IEnumerator CardEndDrag(Card card, Field targetField)
	{
		foreach(GameObject gameObject in anyWhereAreas)
		{gameObject.SetActive(false);}


		DeleteDragLine();

		if(mouseOnArea == EMouseOnArea.Hole)
		{DiscardCard(card);}
		else
		{
			if(card.GetCardType() == ECardType.Servent)
			{
				ServantCardData serventCardData = card.GetCardData() as ServantCardData;
				if(CheckServentSummonable(serventCardData, card.GetComponent<Card>().GetCurrentCost(), targetField))
				{
					targetField.locked = true;
					costCount -= serventCardData.GetCardCost();

					cardObjectList.Remove(card.gameObject);
					handList.RemoveAt(card.GetCardOrder());

					card.SendMissile(alertPoint, ReturnMouseOnField().transform);
					foreach(GameObject cardObject in cardObjectList)
					{cardObject.GetComponent<Card>().SetLock(true);}
					
					yield return new WaitForSeconds(1.2f);
					
					foreach(GameObject cardObject in cardObjectList)
					{cardObject.GetComponent<Card>().SetLock(false);}

					GameObject serventObject = Instantiate(
							playerServentPrefabList[cardHashMap[serventCardData.GetCardNum()]],
							targetField.transform.position,
							Utils.QI);

					targetField.Summon(
						serventObject.GetComponent<Servant>(), serventCardData);

					serventObject.GetComponent<Servant>().InitWithEffect();
					summonedServants.Add(serventObject.GetComponent<Servant>());
					soundEffect.PlayOneShot(serventSummon);


					activatingServant = serventObject.GetComponent<Servant>();
					yield return StartCoroutine(serventCardData.SummonEffectExecute(this));
					activatingServant = null;

					for (int i = 0; i < cardObjectList.Count; ++i)
					{cardObjectList[i].GetComponent<Card>().SetCardOrder(i);}

					CardAlignment();
					yield return StartCoroutine(NotifyServentSummon(serventObject.GetComponent<Servant>()));
					yield return StartCoroutine(CheckServentsCondition());
				}
			}
			else
			{
				SpellCardData spellCardData = card.GetCardData() as SpellCardData;

				if(spellCardData.IsCardUsable(this) && card.GetComponent<Card>().GetCurrentCost() == 0)
				{
					costCount -= spellCardData.GetCardCost();
					// StartCoroutine(ActivateSpell(card.GetCardData(), targetField));

					AddTrash(card.GetCardData());
					handList.RemoveAt(card.GetCardOrder());
					cardObjectList.Remove(card.gameObject);

					card.SendMissile(alertPoint, hole.transform);

					for (int i = 0; i < cardObjectList.Count; ++i)
					{ cardObjectList[i].GetComponent<Card>().SetCardOrder(i); }

					CardAlignment();
					yield return new WaitForSeconds(0.5f);
					yield return StartCoroutine(spellCardData.ActivationEffectExecute(this));
					yield return StartCoroutine(CheckServentsCondition());
				}

			}
		}

		foreach(GameObject cardObject in cardObjectList)
		{cardObject.GetComponent<Card>().SetLock(false);}

	}

	public void LockControl()
	{
		foreach (GameObject cardObject in cardObjectList)
		{ cardObject.GetComponent<Card>().SetLock(true); }
	}

	public void UnlockControl()
	{
		foreach (GameObject cardObject in cardObjectList)
		{ cardObject.GetComponent<Card>().SetLock(false); }
	}

	public void DiscardAllHands()
	{
		// 뒤에서부터 안전하게 버리기
		for (int i = cardObjectList.Count - 1; i >= 0; --i)
		{
			var card = cardObjectList[i].GetComponent<Card>();
			DiscardCard(card);
		}
	}
	public void DrawCard()
	{
		if(deckList.Count == 0 && trashList.Count == 0)
		{return;}


		List<CardData> targetList;

		if(deckList.Count != 0)
		{targetList = deckList;}
		else
		{
			PlayerTakeDamage(1);
			targetList = trashList;
		}

		CardData cardData = targetList[targetList.Count - 1];

		targetList.RemoveAt(targetList.Count - 1);

		InstantiateCard(cardData);
	}


	public void ActivateCardEffect(Servant servent)
	{
		foreach (Servant summonedServent in summonedServants)
		{ summonedServent.SetLock(true); }	

		Destroy(clickedServentInfo);
		clickedServent.AddActivationCount();
		StartCoroutine(ActivateCardEffectCo(servent));
	}

	public IEnumerator ActivateCardEffectCo(Servant servent)
	{
		activatingServant = servent;
		yield return StartCoroutine(servent.GetCardData().ActivationEffectExecute(this));
		activatingServant = null;
		clickedServent = null;
		yield return StartCoroutine(CheckServentsCondition());
		foreach (Servant summonedServent in summonedServants)
		{ summonedServent.SetLock(false); }
	}



	public void SetMouseOnField(EMouseOnArea mouseOnArea)
	{this.mouseOnArea = mouseOnArea;}

	public void ResetMouseOnField()
	{mouseOnArea = EMouseOnArea.None;}

	public void SelectTarget(GameObject field)
	{ missileTarget = field; }
	
	void OnDestroy()
	{
		if (Inst == this) Inst = null;
	}

	public void CardAlignmentAlt()
	{
		if (handList.Count == 0) return;

		List<PRS> originCardPRSs = RoundAlignment(
			cardAreaBorderLeft,
			cardAreaBorderRight,
			cardObjectList.Count,
			0.5f,
			Vector3.one * 2.3f
		);

		for (int i = 0; i < cardObjectList.Count; ++i)
		{
			var targetCard = cardObjectList[i];
			var cardComp = targetCard.GetComponent<Card>();

			cardComp.originPRS = originCardPRSs[i];

			targetCard.transform.DOMove(originCardPRSs[i].pos, 0.3f).SetEase(Ease.InOutQuad);
			targetCard.transform.DORotateQuaternion(originCardPRSs[i].rot, 0.3f).SetEase(Ease.InOutQuad);
			cardComp.UpdateCardCost(costCount);
		}
	}
	public void CardAlignment()
	{
		if (handList.Count == 0)
			return;

		List<PRS> originCardPRSs = RoundAlignment(
			cardAreaBorderLeft,
			cardAreaBorderRight,
			cardObjectList.Count,
			0.5f,
			Vector3.one * 2.3f);

		for (int i = 0; i < cardObjectList.Count; ++i)
		{
			var targetCard = cardObjectList[i];
			var cardComp = targetCard.GetComponent<Card>();

			// 목표 PRS 저장
			cardComp.originPRS = originCardPRSs[i];

			// 💡 DOTween으로 부드럽게 이동/회전/스케일 적용
			targetCard.transform.DOMove(originCardPRSs[i].pos, 0.3f).SetEase(Ease.InOutQuad);
			targetCard.transform.DORotateQuaternion(originCardPRSs[i].rot, 0.3f).SetEase(Ease.InOutQuad);

			// 카드 코스트 갱신
			cardComp.UpdateCardCost(costCount);
		}
	}

	List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objectCount, float height, Vector3 scale)
	{
		float[] objLerps = new float[objectCount];
		List<PRS> results = new List<PRS>(objectCount);

		switch(objectCount)
		{
			case 1: objLerps = new float[] {0.5f}; break;
			case 2: objLerps = new float[] {0.27f, 0.73f}; break;
			case 3: objLerps = new float[] {0.1f, 0.5f, 0.9f}; break;
			default:
				float interval = 1f/ (objectCount - 1);
				for(int i = 0; i < objectCount; ++i)
					objLerps[i] = interval * i;
				break;
		}

		for(int i = 0; i < objectCount; ++i)
		{
			var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
			var targetRot = Quaternion.identity;
			if(objectCount >= 4)
			{
				float curve = Mathf.Sqrt(Mathf.Pow(height,2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
				curve = height >= 0 ? curve : - curve;
				targetPos.y += curve;
				targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
			}
			results.Add(new PRS(targetPos, targetRot, scale));
		}
		return results;
	}



	public void DeleteDragLine()
	{
		cardDragLine.positionCount = 0;
		cardDragLine.endColor = Color.blue;
	}

	public void DealDamageToEnemy(int damage)
	{
		GameObject damageText = Instantiate(floatingTextPrefab);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(30);

		enemyHealth -= damage;
	}


	public void PlayerTakeDamage(int damage)
	{
		GameObject damageText = Instantiate(floatingTextPrefab);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(30);

		playerHealth -= damage;
	}



	public void PlayerTakeAttack(int damage, bool guarded)
	{
		StartCoroutine(ShowBattleWindow(damage, 0));  

		GameObject damageText = Instantiate(floatingTextPrefab, battleWindowLeftSideFloatTextLocation);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(30);

		if(guarded)
		damageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);

		playerHealth -= damage;

	}
	public void ServentTakeAttack(int defenderDamage, int attackerDamage, bool guarded)
	{
		StartCoroutine(ShowBattleWindow(defenderDamage, attackerDamage));
		GameObject defenderDamageText = Instantiate(floatingTextPrefab, battleWindowLeftSideFloatTextLocation);
		defenderDamageText.GetComponent<FloatingDamageText>().SetDamageText(defenderDamage);
		defenderDamageText.GetComponent<FloatingDamageText>().SetFont(150);

		GameObject attackerDamageText = Instantiate(floatingTextPrefab, battleWindowRightSideFloatTextLocation);
		attackerDamageText.GetComponent<FloatingDamageText>().SetDamageText(attackerDamage);
		attackerDamageText.GetComponent<FloatingDamageText>().SetFont(150);

		if (guarded)
			defenderDamageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);
	}




	public IEnumerator BattlePhase()
	{
		Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

		Servant attacker = currentAttacker;
		GameObject defender = hit.collider.gameObject;


		attacker.AddAttackCount();

		if (defender.CompareTag("Enemy")) // 직접 공격시
		{
			int attackerForce = attacker.GetForce();

			attackerForce += enemyDamageIncrease;
			attackerForce -= enemyDamageDecrease;

			if (attackerForce < 0)
			{ attackerForce = 0; }

			if (enemyDamageBlock)
			{ attackerForce = 0; }

			GameObject leftActor = Instantiate(
					playerServentPrefabList[cardHashMap[attacker.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);

			GameObject rightActor = Instantiate(enemyObject, new Vector3(), Utils.QI);


			attacker.ChangeState(EServentState.Attack, false, 0.1f);
			leftActor.GetComponent<Servant>().ChangeState(EServentState.Attack);

			battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(leftActor);
			leftActor.GetComponent<Servant>().OnBattleWindow();
			battleWindowRightSide.GetComponent<BattleWindow>().SetActor(rightActor);
			rightActor.GetComponent<SpriteRenderer>().sortingOrder = 104;

			yield return StartCoroutine(ShowBattleWindow(0, attackerForce));
			enemyHealth -= attackerForce;
			
			attacker.ChangeState(EServentState.Idle, false, 0.1f);

			yield return StartCoroutine(CheckEnemyCondition());
		}
		else if (defender.CompareTag("Servent")) // 소환수 공격시
		{
			int attackerForce = attacker.GetForce();
			int defenderForce = defender.GetComponent<Servant>().GetForce();

			int attackerDamage = Math.Abs(defenderForce);
			int defenderDamage = Math.Abs(attackerForce);

			originalDefenderForce = defenderForce;
			currentAttacker = attacker;
			currentDefender = defender.GetComponent<Servant>();

			GameObject leftActor = Instantiate(
			playerServentPrefabList[cardHashMap[attacker.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);
			leftActor.GetComponent<Servant>().ChangeState(EServentState.Attack);

			GameObject rightActor = Instantiate(
					enemyServentPrefabList[cardHashMap[defender.GetComponent<Servant>().GetCardData().GetCardNum()]],
					new Vector3(), Utils.QI);
			rightActor.GetComponent<Servant>().ChangeState(EServentState.Guard);


			battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(leftActor);
			leftActor.GetComponent<Servant>().OnBattleWindow();

			battleWindowRightSide.GetComponent<BattleWindow>().SetActor(rightActor);
			rightActor.GetComponent<Servant>().OnBattleWindow();



			//ServentTakeAttack(attackerDamage, defenderDamage, false);
			yield return StartCoroutine(ShowBattleWindow(attackerDamage, defenderDamage));
			
			attacker.ChangeState(EServentState.Idle, false, 0.1f);
			attacker.TakeDamage(attackerDamage);
			yield return StartCoroutine(attacker.GetCardData().AttackEffectExecute(this));
			defender.GetComponent<Servant>().TakeDamage(defenderDamage);
			yield return StartCoroutine(defender.GetComponent<Servant>().GetCardData().HitEffectExecute(this));
			
			yield return StartCoroutine(CheckServentsCondition());

			StartCoroutine(CheckEnemyCondition());
		}
	}
	//public IEnumerator CheckServentsCondition()
	//{
	//	Debug.Log(summonedServents.Count);
	//	summonedServents.RemoveAll(x => x == null);
	//	foreach (Servent servent in summonedServents)
	//	{
	//		if (servent.GetForce() <= 0)
	//		{
	//			yield return StartCoroutine(servent.GetCardData().DeathEffectExecute(this));
	//			yield return StartCoroutine(servent.DieCoroutine());
	//		}
	//	}
	//	summonedServents.RemoveAll(x => x == null);
	//}

	public IEnumerator CheckServentsCondition()
	{
		summonedServants.RemoveAll(x => x == null);

		List<Servant> deadServents = new List<Servant>();
		foreach (Servant servent in summonedServants)
		{
			if (servent.GetForce() <= 0)
				deadServents.Add(servent);
		}

		foreach (Servant dead in deadServents)
		{
			yield return StartCoroutine(dead.GetCardData().DeathEffectExecute(this));
			yield return StartCoroutine(NotifyServentDeath(dead));
			yield return StartCoroutine(dead.DieCoroutine());
		}

		summonedServants.RemoveAll(x => x == null);
	}

	public IEnumerator DrawAttackLine(Vector2 startPoint, Vector2 targetPoint, float duration)
	{
		Vector3[] point = new Vector3[lineCount];
		float posA = 10f;
		float posB = 10f;
		attackDragLine.positionCount = lineCount;
		for(int i = 0; i < lineCount; ++i)
		{
			float t;
			if (i == 0)
			{t = 0;}
			else
			{t = (float)i / (lineCount - 1);}
			
			point[i] = Bezier(startPoint,
			PointSetting(startPoint),
			PointSetting(targetPoint),
			targetPoint, t);
			point[i].z = 0;
		}
		attackDragLine.SetPositions(point);

		
		yield return new WaitForSeconds(duration);

		

		Vector3 PointSetting(Vector3 origin){
			float x, y;
			x = posA * Mathf.Cos(120 * Mathf.Deg2Rad) + origin.x;
			y = posB * Mathf.Sin(120 * Mathf.Deg2Rad) + origin.y;
	
			return new Vector3(x, y);
		}
		Vector3 Bezier(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
		{
			Vector3 M0 = Vector3.Lerp(P0, P1, t);
			Vector3 M1 = Vector3.Lerp(P1, P2, t);
			Vector3 M2 = Vector3.Lerp(P2, P3, t);

			Vector3 B0 = Vector3.Lerp(M0, M1, t);
			Vector3 B1 = Vector3.Lerp(M1, M2, t);

			return Vector3.Lerp(B0, B1, t);
		}

	}

	public void DrawAttackLine(Vector2 startPoint, bool isUsuable)
	{
		Vector3[] point = new Vector3[lineCount];
		float posA = 10f;
		float posB = 10f;
		attackDragLine.positionCount = lineCount;
		Vector3 targetPoint = new Vector3();

		if(isUsuable)
		{attackDragLine.endColor = Color.blue;}
		else
		{attackDragLine.endColor = Color.red;}
		

		switch(mouseOnArea)
		{
			case EMouseOnArea.None:
			targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);
			break;

			case EMouseOnArea.Field_1:
			targetPoint = field_1.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_2:
			targetPoint = field_2.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_3:
			targetPoint = field_3.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_4:
			targetPoint = field_4.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_5:
			targetPoint = field_5.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_6:
			targetPoint = field_6.GetLinePoint().position;
			break;

			case EMouseOnArea.Player:
			targetPoint = playerObject.transform.position;
			break;

			case EMouseOnArea.Enemy:
			targetPoint = enemyObject.transform.position;
			break;
			
			case EMouseOnArea.AnyWhere:
			targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);
			break;
			
			default:
			targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);
			break;
		}

		for(int i = 0; i < lineCount; ++i)
		{
			float t;
			if (i == 0)
			{t = 0;}
			else
			{t = (float)i / (lineCount - 1);}
			
			point[i] = Bezier(startPoint, PointSetting(startPoint),
			PointSetting(targetPoint),targetPoint, t);
			point[i].z = 0;
		}

		attackDragLine.SetPositions(point);

		Vector3 PointSetting(Vector3 origin){
			float x, y;
			x = posA * Mathf.Cos(120 * Mathf.Deg2Rad) + origin.x;
			y = posB * Mathf.Sin(120 * Mathf.Deg2Rad) + origin.y;
	
			return new Vector3(x, y);
		}
		Vector3 Bezier(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
		{
			Vector3 M0 = Vector3.Lerp(P0, P1, t);
			Vector3 M1 = Vector3.Lerp(P1, P2, t);
			Vector3 M2 = Vector3.Lerp(P2, P3, t);

			Vector3 B0 = Vector3.Lerp(M0, M1, t);
			Vector3 B1 = Vector3.Lerp(M1, M2, t);

			return Vector3.Lerp(B0, B1, t);
		}
	}


	public void CloseTrashCards()
	{
		for( int i = trashLayoutGroup.transform.childCount - 1; i >= 0 ; --i )
		{Destroy(trashLayoutGroup.transform.GetChild(i).gameObject );}


		foreach(GameObject cardObject in cardObjectList)
		{cardObject.GetComponent<Card>().SetLock(false);}

		trashWindow.GetComponent<Window>().OnOff();
	}

	public void AddTrash(CardData cardData)
	{    
		trashList.Add(cardData);
	}

	public void RemoveTrash(CardData cardData)
	{
		trashList.Remove(cardData);
	}

	public void DrawDragLine(Vector2 startPoint, bool isUsuable)
	{
		Vector3[] point = new Vector3[lineCount];
		float posA = 10f;
		float posB = 10f;
		cardDragLine.positionCount = lineCount;

		if(isUsuable)
		{cardDragLine.endColor = Color.blue;}
		else
		{cardDragLine.endColor = Color.red;}
		
		Vector3 targetPoint = new Vector3();

		switch(mouseOnArea)
		{
			case EMouseOnArea.None:
			targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);
			break;

			case EMouseOnArea.Field_1:
			targetPoint = field_1.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_2:
			targetPoint = field_2.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_3:
			targetPoint = field_3.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_4:
			targetPoint = field_4.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_5:
			targetPoint = field_5.GetLinePoint().position;
			break;

			case EMouseOnArea.Field_6:
			targetPoint = field_6.GetLinePoint().position;
			break;

			case EMouseOnArea.Hole:
			targetPoint = hole.transform.position;
			break;

			case EMouseOnArea.Player:
			targetPoint = playerObject.transform.position;
			break;

			case EMouseOnArea.Enemy:
			targetPoint = enemyObject.transform.position;
			break;
			
			case EMouseOnArea.AnyWhere:
			targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);
			break;
			
			default:
			targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);
			break;
		}
		startPoint = camera.ScreenToWorldPoint(startPoint);

		for(int i = 0; i < lineCount; ++i)
		{
			float t;
			if (i == 0)
			{t = 0;}
			else
			{t = (float)i / (lineCount - 1);}
			
			point[i] = Bezier(startPoint, PointSetting(startPoint),
			PointSetting(targetPoint),targetPoint, t);
			point[i].z = 0;
		}
		cardDragLine.SetPositions(point);
		

		Vector3 PointSetting(Vector3 origin){
			float x, y;
			x = posA * Mathf.Cos(120 * Mathf.Deg2Rad) + origin.x;
			y = posB * Mathf.Sin(120 * Mathf.Deg2Rad) + origin.y;
	
			return new Vector3(x, y);
		}
		
		Vector3 Bezier(Vector3 P0, Vector3 P1, Vector3 P2, Vector3 P3, float t)
		{
			Vector3 M0 = Vector3.Lerp(P0, P1, t);
			Vector3 M1 = Vector3.Lerp(P1, P2, t);
			Vector3 M2 = Vector3.Lerp(P2, P3, t);
			Vector3 B0 = Vector3.Lerp(M0, M1, t);
			Vector3 B1 = Vector3.Lerp(M1, M2, t);
			return Vector3.Lerp(B0, B1, t);
		}
	}

	public void BackToDungeon()
	{
		StartCoroutine(BackToDungeonRoutine());
	}

	IEnumerator BackToDungeonRoutine()
	{
		StartCoroutine(FadeOut());
		yield return new WaitForSeconds(1.5f);

		if(BattleData.nextScene != null)
		{SceneManager.LoadScene(BattleData.nextScene);}
		else{SceneManager.LoadScene("Dungeon");}
		
	}

	
	public void ParryCircle()
	{
		Vector3 targetScale = bigCircle.transform.localScale;
		smallCircle.transform.DOScale(targetScale, circleSpeed).SetEase(Ease.Linear)
		.OnComplete(() => {smallCircle.transform.localScale = new Vector3(0,0,0);});
	}

	public void ReadyServentAttack(Servant servent)
	{currentAttacker = servent;}

	public Servant GetSelectedServent()
	{ return selectedServant; }
	public void ClearLine()
	{
		attackDragLine.positionCount = 0;
		cardDragLine.positionCount = 0;
	}

	public List<Servant> GetServents(EServentType serventType)
	{
		if (serventType == EServentType.None)
			return summonedServants;

		List<Servant> serventList = new List<Servant>();
		
		foreach(Servant servent in summonedServants)
		{
			if(servent.GetServentType() == serventType)
			{serventList.Add(servent); }
		}
		
		return serventList;
	}

	public IEnumerator BackToHands()
	{
		StartCoroutine(InstantiateCardFromServent(clickedServent.GetCardData()));
		summonedServants.Remove(clickedServent);
		Destroy(clickedServent.gameObject);
		clickedServent = null;
		yield return new WaitForSeconds(1f);
	}


	IEnumerator NotifyServentSummon(Servant servent)
	{
		foreach(Servant summonedServent in summonedServants)
		{
			activatingServant = summonedServent;
			yield return StartCoroutine(summonedServent.GetCardData().NotifySummonEffectExecute(this, servent));
		}
		activatingServant = null;
		yield return null;
	}

	IEnumerator NotifyServentDeath(Servant servent)
	{
		foreach (Servant summonedServent in summonedServants)
		{
			activatingServant = summonedServent;
			yield return StartCoroutine(summonedServent.GetCardData().NotifyDeathEffectExecute(this, servent));
		}
		activatingServant = null;
		yield return null;
	}



}

public struct DamageResult
{
    public int attackerDamage;
    public int defenderDamage;

    public DamageResult(int attackerDamage, int defenderDamage)
    {
        this.attackerDamage = attackerDamage;
        this.defenderDamage = defenderDamage;
    }
}