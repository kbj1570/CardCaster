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


public class BattleManager : MonoBehaviour
{
	public AudioSource backGroundMusic;
	public AudioSource soundEffect;
	public AudioClip serventDeath;
	public AudioClip serventSummon;

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


	List<Enemy> enemies = new();
	Enemy currentEnemy;
	int enemyIndex = 0;

	public GameObject runawayButton;
	public GameObject floatingTextPrefab;
	public Transform alertPoint; 
	public static BattleManager Inst{get; private set;}
	public Canvas canvas;
	public Camera camera;
	public Field playerField;
	public Field enemyField;
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

	public Servent clickedServent;

	public TMP_Text costCountText;
	public TMP_Text deckCountText;
	public TMP_Text trashCountText;
	public TMP_Text playerHealthText;
	public TMP_Text enemyHealthText;

	private int costCount;
	private int deckCount;
	private int trashCount;

	
	public List<CardData> selectedCards;
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


	private bool phaseFlag;
	private bool isActionDone = false;
	bool isParryWindowActive = false;

	float parryWindowTime = 0.3f;
	float circleSpeed = 1f;


	public int playerHealth;
	public int enemyHealth;



	private IEnumerator ShowBattleWindow()
	{
		
		foreach(Field field in GetAllFields())
		{
			if(field.GetFilled())
			{
				field.HideForce(true);
			}
		}

		foreach (GameObject card in cardObjectList)
		{card.GetComponent<Card>().SetLock(true);}

		battleWindowLeftSide.transform.DOMove(battleWindowLeftSideSecondPosition.position,
		0.2f).SetEase(Ease.Linear);
		battleWindowRightSide.transform.DOMove(battleWindowRightSideSecondPosition.position,
		0.2f).SetEase(Ease.Linear);

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

		

		

		foreach (Field field in GetAllFields())
		{
			if (field.GetFilled())
			{
				field.HideForce(false);
			}
		}
		yield return new WaitForSeconds(1f);


		battleWindowLeftSide.GetComponent<BattleWindow>().ClearActor();
		battleWindowRightSide.GetComponent<BattleWindow>().ClearActor();

		foreach (GameObject card in cardObjectList)
		{ card.GetComponent<Card>().SetLock(false); }
		isActionDone = true;
	}

	public List<Field> GetPlayerFields()
	{ return new List<Field> { field_1, field_2, field_3 }; }
	public List<Field> GetEnemyFields()
	{ return new List<Field> { field_4, field_5, field_6 }; }
	public List<Field> GetAllFields()
	{ return new List<Field> { field_1, field_2, field_3, field_4, field_5, field_6 }; }

	public void ActionDone()
	{isActionDone = true;}

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

	void Awake()
	{
		Debug.Log("Awake Start");
		Inst = this;
		enemies = BattleData.enemies;
		GameSetup();

		reward = new();
		handList = new();
		selectedCards = new();
		mouseOnArea = EMouseOnArea.None;



		StartCoroutine(FadeIn());
		StartCoroutine(GameLoop());
	}

	public bool AddSelectedCards(CardData cardData)
	{
		bool foo = selectedCards.Count < selectedLimit;

		if(foo)
		{selectedCards.Add(cardData);}

		return foo;
	}
	private IEnumerator FadeIn() 
	{
		Debug.Log("FadeIn Start");
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

	//public void ShowSelectedCards(List<BattleCardData> targetList,ECardType cardType, int limit)
	//{
	//	isActionDone = false;
	//	selectedLimit = limit;
	//	foreach(CardData cardData in targetList)
	//	{
	//		if(cardType == null ||cardData.GetCardType() == cardType)
	//		{
	//			GameObject cardObject = Instantiate(dummyCardPrefabList[cardHashMap[cardData.GetCardNum()]], selectedCardLayoutGroup.transform);
	//			GameObject cardFrameObject = Instantiate(cardSelectFrame, cardObject.transform);
				
	//			cardObject.GetComponent<DummyCard>().SetLock(true);
	//			cardFrameObject.GetComponent<CardSelectFrame>().SetCardData(cardData);
	//			cardFrameObject.transform.localPosition = new Vector3(0, 0, 0);
	//			cardFrameObject.transform.localScale = new Vector3(1, 1, 0);
	//		}

			
	//	}

	//	RectTransform rectTransform = selectedCardLayoutGroup.GetComponent<RectTransform>();

	//	int height = ((selectedCardLayoutGroup.transform.childCount / 2) * 680) +  550;
	//	rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);

	//	cardSelectWindow.GetComponent<Window>().OnOff();
	//}

	

	public void HealPlayer(int value)
	{
		playerHealth += value;
	}

	IEnumerator CheckEnemyCondition(float delay)
	{
		if(enemyHealth <= 0)
		{
			enemyHealth = 0;

			AlertMessage("적을 쓰러트렸습니다.");
			yield return new WaitForSeconds(delay);


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
		UpdateCondition();

		
		
		if(Input.GetKeyDown(KeyCode.Mouse0))
		{CloseServentInfo();}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			if(parryState != EParryState.Parry)
			return;

			if(isParryWindowActive)
			{
				GameObject damageText = Instantiate(floatingTextPrefab, alertPoint);
				damageText.GetComponent<FloatingDamageText>().SetDamageText("Guard!!");
				damageText.GetComponent<FloatingDamageText>().SetFont(150);
				damageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);
				parryState = EParryState.Succecced;
				
			}
			else
			{
				parryState = EParryState.Failed;
			}
			StopCoroutine(ParryWindowCoroutine());
			isActionDone = true;
			isParryWindowActive = false;
		}
	}

	public void StartParryWindow()
	{
		StartCoroutine(ParryWindowCoroutine());
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

		missileScript.master = camera.ScreenToWorldPoint(startPoint.position);
		missileScript.enemy = targetPoint.position;
	}

	public void ShotDrawMissile(Transform targetPoint)
	{
		GameObject bullet = Instantiate(missile, hole.transform.position, Utils.QI);
		BezierMissile missileScript = bullet.GetComponent<BezierMissile>();

		missileScript.master = hole.transform.position;
		missileScript.enemy = camera.ScreenToWorldPoint(targetPoint.position);
	}



	public void ShotMissile(Transform startPoint)
	{
		GameObject bullet = Instantiate(missile, startPoint.position, Utils.QI);
		BezierMissile missileScript = bullet.GetComponent<BezierMissile>();
		missileScript.master = startPoint.position;
		missileScript.enemy = hole.transform.position;
	}

	public IEnumerator ShowServentInfo(Servent servent)
	{
		if(servent.GetServentType() == EServentType.Player)
		{clickedServentInfo = Instantiate(playerServentInfoList[servent.GetServentNum()], Input.mousePosition, Utils.QI);}
		else if(servent.GetServentType() == EServentType.Enemy)
		{clickedServentInfo = Instantiate(enemyServentInfoList[servent.GetServentNum() - 1000], Input.mousePosition, Utils.QI);}
		
		Vector3 vector = clickedServentInfo.transform.position;
		vector.x += clickedServentInfo.GetComponent<RectTransform>().rect.width * 0.7f;
		clickedServentInfo.transform.position = vector;
		yield return new WaitForSeconds(0.1f);
		clickedServentInfo.GetComponent<ServentInfoWindow>().OnOff(true);
		clickedServentInfo.transform.SetParent(canvas.transform);
		clickedServent = servent; 
	}
	public void CloseServentInfo()
	{
		if(clickedServent == null)
		{return;}

		if(clickedServent != null)
		{
			clickedServent = null;
			Destroy(clickedServentInfo.gameObject);
		}
	}

	public IEnumerator GameLoop()
	{
		Debug.Log("Loop Start");
		yield return new WaitForSeconds(1f);
		while (true)
		{
			yield return StartCoroutine(PlayerTurn());
			yield return StartCoroutine(EnemyTurn());
		}
	}

	void GameSetup()
	{
		Debug.Log("SetUp Start");
		trashCount = 0;
		deckCount = 0;
		costCount = 0;
		playerHealth = PlayerData.saveData.health;

		currentEnemy = enemies[enemyIndex];
		enemyHealth = currentEnemy.GetHealth();
		cardHashMap = DataController.Inst.LoadCardHashMap();

		Dictionary<CardData, int> deck = new Dictionary<CardData, int>();
		List<CardData> cardDatabase = DataController.Inst.LoadCardDatabase();
		Dictionary<string, int> myDeck = PlayerData.saveData.deck;


		battleWindowLeftSide.GetComponent<BattleWindow>().SetBackGround(DungeonData.dungeon.GetDungeonNum());
		battleWindowRightSide.GetComponent<BattleWindow>().SetBackGround(DungeonData.dungeon.GetDungeonNum());


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
		if(deckList == null)
		return;
		
		deckCount = deckList.Count;
		trashCount = trashList.Count;

		costCountText.text = "Cost: " + costCount.ToString();
		deckCountText.text = "Deck: " + deckCount.ToString();
		trashCountText.text = "Trash: " + trashCount.ToString();
		playerHealthText.text = "PC: "+ playerHealth.ToString();
		enemyHealthText.text = "Enemy: "+ enemyHealth.ToString();

		field_1.UpdateHealth();
		field_2.UpdateHealth();
		field_3.UpdateHealth();
		field_4.UpdateHealth();
		field_5.UpdateHealth();
		field_6.UpdateHealth();

		if(enemyHealth <= 0)
		{
			enemyHealth = 0;
			StartCoroutine(WinBattle());
		}

		if (playerHealth <= 0)
		{
			playerHealth = 0;

			StopAllCoroutines();

			StartCoroutine(GameOver());
		}
	}

	public void SetEnemyDamageBlock(bool value)
	{ this.enemyDamageBlock = value; }

	public IEnumerator DrawPhase()
	{
		Debug.Log("Draw Phase");
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
		Debug.Log("StandBy Phase");
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
		Debug.Log("Main Phase");
		/* 플레이어가 행동하는 단계
		 * 행동을 모두 마치고 메인 페이즈를 마친다.
		 * 행동이란 소환, 마법 사용, 공격, 패 버리기 등을 포함한다.
		 * 행동을 모두 마쳤다면 메인 페이즈를 마친다.
		 */

		foreach (GameObject card in cardObjectList)
		{ card.GetComponent<Card>().SetLock(false); }

		yield return new WaitUntil(() => isActionDone);
	}
	public IEnumerator EndPhase()
	{
		Debug.Log("End Phase");

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
		field_1.GetComponent<Field>().SetAttacked(false);
		field_2.GetComponent<Field>().SetAttacked(false);
		field_3.GetComponent<Field>().SetAttacked(false);
		field_4.GetComponent<Field>().SetAttacked(false);
		field_5.GetComponent<Field>().SetAttacked(false);
		field_6.GetComponent<Field>().SetAttacked(false);

		enemyDamageBlock = false;

		foreach(GameObject card in cardObjectList)
		{card.GetComponent<Card>().HideAndReveal(false); }
		yield return new WaitForSeconds(0.4f);

		yield return StartCoroutine(DrawPhase());
		yield return StartCoroutine(StandByPhase());
		yield return StartCoroutine(MainPhase());
		yield return StartCoroutine(EndPhase());
	}

	IEnumerator EnemyTurn()
	{
		yield return
		StartCoroutine(StandByPhase());
		yield return
		StartCoroutine(MainPhase());
		yield return
		StartCoroutine(EndPhase());

		yield return new WaitForSeconds(0.3f);
		int actionToken = currentEnemy.GetActionToken();

		for (int i = 0; i < actionToken; ++i)
		{
			List<Field> filledField = new();
			List<Field> emptyField = new();
			List<Field> attackableField = new();

			Field[] enemyFields = { field_4, field_5, field_6 };

			for (int idx = 0; idx < enemyFields.Length; idx++)
			{
				if (enemyFields[idx].GetFilled())
				{
					filledField.Add(enemyFields[idx]);
					if (!enemyFields[idx].GetAttacked())
						attackableField.Add(enemyFields[idx]);

				}
				else
				{ emptyField.Add(enemyFields[idx]); }
			}

			isActionDone = false;
			EEnemyAction action = SelectEnemyAction(emptyField.Count, attackableField.Count);

			switch (action)
			{
				case EEnemyAction.Summon:
					if (emptyField.Count > 0)
					{

						Field field = emptyField[Random.Range(0, emptyField.Count)];
						field.locked = true;

						List<EnemyServentCardData> serventList = currentEnemy.GetServentList();
						EnemyServentCardData randomServent = serventList[Random.Range(0, serventList.Count)];

						StartCoroutine(ShowEnemyActionCard(randomServent, field.transform));
						yield return new WaitForSeconds(2f);

						GameObject serventObject =
							Instantiate(enemyServentPrefabList[cardHashMap[randomServent.GetCardNum()]], field.transform.position, Utils.QI);
						field.Summon(randomServent, serventObject.GetComponent<Servent>());
						serventObject.GetComponent<Servent>().InitWithEffect();

						soundEffect.PlayOneShot(serventSummon);
						field.locked = false;
					}
					isActionDone = true;
					break;

				case EEnemyAction.Attack:
					if (attackableField.Count > 0)
					{
						List<Field> playerTargets = new List<Field> { playerField };
						if (field_1.GetFilled()) playerTargets.Add(field_1);
						if (field_2.GetFilled()) playerTargets.Add(field_2);
						if (field_3.GetFilled()) playerTargets.Add(field_3);

						Field startField = attackableField[Random.Range(0, attackableField.Count)];
						Field targetField = playerTargets[Random.Range(0, playerTargets.Count)];

						StartCoroutine(EnemyAttack(startField, targetField));
					}
					else
					{ isActionDone = true; }
					break;

				case EEnemyAction.None:
					AlertMessage("적이 아무것도 할 수 없습니다.");
					isActionDone = true;
					break;
			}
			yield return new WaitUntil(() => isActionDone);
			yield return new WaitForSeconds(2.5f);
		}
		isActionDone = false;
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
		onMessage.GetComponent<PopUpMessage>().SetText(message);
	}



	
	public IEnumerator EnemyFieldClear()
	{
		if(field_4.GetFilled())
		field_4.SetHealth(0);

		if(field_5.GetFilled())
		field_5.SetHealth(0);

		if(field_6.GetFilled())
		field_6.SetHealth(0);
		
		yield return new WaitForSeconds(0.3f);
	}

	public GameObject InstantiateCard(CardData battleCardData)
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
				cardObject = Instantiate(serventCardPrefab, camera.WorldToScreenPoint(enemyField.transform.position), Utils.QI);
				break;
			case ECardType.Spell:
				cardObject = Instantiate(spellCardPrefab, camera.WorldToScreenPoint(enemyField.transform.position), Utils.QI);
				break;
			case ECardType.Field:
				cardObject = Instantiate(fieldSpellCardPrefab, camera.WorldToScreenPoint(enemyField.transform.position), Utils.QI);
				break;
			case ECardType.Enemy:
				cardObject = Instantiate(fieldSpellCardPrefab, camera.WorldToScreenPoint(enemyField.transform.position), Utils.QI);
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

		return EEnemyAction.Ability;
	}
	IEnumerator EnemyAttack(Field startField,Field targetField)
	{
		parryState  = EParryState.Parry;
		isActionDone = false;

		StartCoroutine(DrawAttackLine(startField.GetLinePoint().position
		,targetField.GetLinePoint().position, circleSpeed));

		ParryCircle();
		yield return new WaitForSeconds(circleSpeed - parryWindowTime);
		StartParryWindow();
		yield return new WaitUntil(() => isActionDone);

		if (targetField == playerField)
		{
			int attackerForce = startField.GetForce();

			attackerForce += playerDamageIncrease;
			attackerForce -= playerDamageDecrease;

			if(parryState == EParryState.Succecced)
			{attackerForce -= 1;}

			if(attackerForce < 0)
			{attackerForce = 0;}

			if(playerDamageBlock)
			{attackerForce = 0;}

			GameObject leftActor = Instantiate(playerObject, new Vector3(), Utils.QI);

			GameObject rightActor = Instantiate(
					enemyServentPrefabList[cardHashMap[startField.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);

			battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(leftActor);
			leftActor.GetComponent<SpriteRenderer>().sortingOrder = 104;
			battleWindowRightSide.GetComponent<BattleWindow>().SetActor(rightActor);
			rightActor.GetComponent<Servent>().OnBattleWindow();
			PlayerTakeAttack(attackerForce, parryState == EParryState.Succecced);
		}
		else
		{
			int attackerForce = startField.GetForce();
			int defenderForce = targetField.GetForce();

			int attackerDamage = Math.Abs(defenderForce);
			int defenderDamage = Math.Abs(attackerForce);

			if(parryState == EParryState.Succecced)
			{defenderDamage -= 1;}

			if(attackerForce < 0)
			{defenderDamage = 0;}

			GameObject leftActor = Instantiate(
					playerServentPrefabList[cardHashMap[targetField.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);

			GameObject rightActor = Instantiate(
					enemyServentPrefabList[cardHashMap[startField.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);


			battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(leftActor);
			leftActor.GetComponent<Servent>().OnBattleWindow();

			battleWindowRightSide.GetComponent<BattleWindow>().SetActor(rightActor);
			rightActor.GetComponent<Servent>().OnBattleWindow();

			ServentTakeAttack(defenderDamage, attackerDamage, parryState == EParryState.Succecced);

			yield return new WaitForSeconds(2f);

			startField.TakeAttack(attackerDamage);
			targetField.TakeAttack(defenderDamage);

			if (startField.GetPenetrate())
			{
				defenderDamage = Math.Abs(defenderForce - attackerForce);
				
				if(playerDamageBlock)
				{defenderDamage = 0;}
				
				PlayerTakeDamage(defenderDamage);
			}
		}
		attackDragLine.positionCount = 0;
		

		Color originColor = bigCircle.color;
		Color targetColor = new Color(1f, 0.3f, 0.3f, 0.5f);

		if(parryState == EParryState.Succecced)
		{targetColor = new Color(0.3f, 0.3f, 1f, 0.5f);}
		


		bigCircle.DOColor(targetColor, 0.1f)
					 .SetLoops(3, LoopType.Yoyo)
					 .OnComplete(() => bigCircle.color = originColor);

		parryState = EParryState.Idle;

		yield return new WaitForSeconds(2f);

		StartCoroutine(CheckEnemyCondition(2f));
		startField.SetAttacked(true);
		isActionDone = false;
		yield return new WaitForSeconds(1f);
	}

	public Field ReturnMouseOnField(EMouseOnArea value)
	{
		 switch(value)
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

			case EMouseOnArea.Enemy:
			return enemyField;

			case EMouseOnArea.Player:
			return playerField;

			case EMouseOnArea.AnyWhere:
			return null;

			default:
			return null;
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
			case EMouseOnArea.Hole:
			return null;

			case EMouseOnArea.Enemy:
			return enemyField;

			case EMouseOnArea.Player:
			return playerField;

			case EMouseOnArea.AnyWhere:
			return field_1;

			default:
			return null;
		}
	}

	public void EndTurn()
	{
		StartCoroutine(PlayerTurn());
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
		if(cardObject.GetComponent<Card>().GetCardData().GetCardTargetType() == ECardTargetType.Selected)
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

		if(targetField == null)
		{return false;}

		if(targetField.locked)
		{return false;}

		if(targetField == field_4)
		{return false;}

		if(targetField == field_5)
		{return false;}

		if(targetField == field_6)
		{return false;}

		if(targetField == playerField || targetField == enemyField)
		{return false;}

		if(targetField.GetFilled())
		{return false;}

		return true;
	}


	public void PlayServentDeathSound()
	{
		soundEffect.PlayOneShot(serventDeath);
	}

	public bool CheckAttackable(EMouseOnArea start)
	{
		if(ReturnMouseOnField() == null)
		return false;

		if(ReturnMouseOnField(start).GetAttacked())
		return false;

		if(ReturnMouseOnField() == ReturnMouseOnField(EMouseOnArea.Enemy))
		return true;

		if(ReturnMouseOnField() == ReturnMouseOnField(EMouseOnArea.Player))
		return false;

		if(ReturnMouseOnField() == ReturnMouseOnField(start))
		return false;

		return ReturnMouseOnField(start).GetFilled() && ReturnMouseOnField().GetFilled();
	}

	private IEnumerator ShowEnemyActionCard(EnemyServentCardData enemyServentCardData, Transform target)
	{
		GameObject cardObject = Instantiate(enemyCardPrefab, enemyField.transform.position, Utils.QI);
		cardObject.transform.SetParent(canvas.transform);
		cardObject.GetComponent<Card>().SetEnemyActionCard(enemyServentCardData);
		cardObject.GetComponent<Card>().InitiateActionInBattle();
		yield return new WaitForSeconds(0.3f);
		cardObject.GetComponent<Card>().SendMissile(alertPoint, target);
		yield return new WaitForSeconds(1.8f);
	}

	private IEnumerator ShowEnemyActionCard(string ablityName, string abilityDesc)
	{
		GameObject cardObject = Instantiate(enemyCardPrefab, enemyField.transform.position, Utils.QI);
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

		isActionDone = false;

		if(mouseOnArea == EMouseOnArea.Hole)
		{
			DiscardCard(card);
		}
		else
		{
			if(card.GetCardType() == ECardType.Servent)
			{
				ServentCardData serventCardData = card.GetCardData() as ServentCardData;
				if(CheckServentSummonable(serventCardData, card.GetComponent<Card>().GetCurrentCost(), targetField))
				{
					targetField.locked = true;
					costCount -= serventCardData.GetCardCost();

					cardObjectList.Remove(card.gameObject);
					handList.RemoveAt(card.GetCardOrder());

					card.SendMissile(alertPoint, ReturnMouseOnField().transform);
					foreach(GameObject cardObject in cardObjectList)
					{cardObject.GetComponent<Card>().SetLock(true);}
					
					yield return new WaitForSeconds(1.5f);
					
					foreach(GameObject cardObject in cardObjectList)
					{cardObject.GetComponent<Card>().SetLock(false);}

					GameObject serventObject = Instantiate(
							playerServentPrefabList[cardHashMap[serventCardData.GetCardNum()]],
							targetField.transform.position,
							Utils.QI
						);

					targetField.Summon(
						serventCardData,
						serventObject.GetComponent<Servent>()
					);

					serventObject.GetComponent<Servent>().InitWithEffect();

					soundEffect.PlayOneShot(serventSummon);

					//StartCoroutine(ActivateSummonAbility(
					//	serventCardData,
					//	card.GetComponent<BattleCardObject>().GetCurrentCost(),
					//	targetField
					//));
					StartCoroutine(serventCardData.SummonEffectExecute(this));

					for (int i = 0; i < cardObjectList.Count; ++i)
					{cardObjectList[i].GetComponent<Card>().SetCardOrder(i);}

					CardAlignment();
				}
			}
			else
			{
				SpellCardData spellCardData = card.GetCardData() as SpellCardData;

				if(spellCardData.IsCardUsable(this) && card.GetComponent<Card>().GetCurrentCost() == 0)
				{
					costCount -= spellCardData.GetCardCost();
					// StartCoroutine(ActivateSpell(card.GetCardData(), targetField));

					yield return StartCoroutine(spellCardData.ActivationEffectExecute(this));


					AddTrash(card.GetCardData());
					handList.RemoveAt(card.GetCardOrder());
					cardObjectList.Remove(card.gameObject);

					card.SendMissile(alertPoint, hole.transform);

					for (int i = 0; i < cardObjectList.Count; ++i)
					{ cardObjectList[i].GetComponent<Card>().SetCardOrder(i); }

					CardAlignment();
				}

			}
		}

		foreach(GameObject cardObject in cardObjectList)
		{cardObject.GetComponent<Card>().SetLock(false);}
	}

	public void DiscardAllHands()
	{
		int count = cardObjectList.Count;

		for(int i = 0; i < count; ++i)
		{
			DiscardCard(cardObjectList[cardObjectList.Count].GetComponent<Card>());
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



	public void SetMouseOnField(EMouseOnArea mouseOnArea)
	{this.mouseOnArea = mouseOnArea;}

	public void ResetMouseOnField()
	{mouseOnArea = EMouseOnArea.None;}

	public void SelectTarget(GameObject field)
	{missileTarget = field;}

	public void CardAlignmentAlt() { if (handList.Count == 0) { return; } List<PRS> originCardPRSs = new List<PRS>(); originCardPRSs = RoundAlignment(cardAreaBorderLeft, cardAreaBorderRight, cardObjectList.Count, 0.5f, Vector3.one * 2.3f); for (int i = 0; i < cardObjectList.Count; ++i) { var targetCard = cardObjectList[i]; targetCard.GetComponent<Card>().originPRS = originCardPRSs[i]; targetCard.transform.position = originCardPRSs[i].pos; targetCard.GetComponent<Card>().UpdateCardCost(costCount); } }

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
			//targetCard.transform.DOScale(originCardPRSs[i].scale, 0.3f).SetEase(Ease.InOutQuad);

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
		damageText.GetComponent<FloatingDamageText>().SetFont(150);

		enemyHealth -= damage;
	}

	public void AttackToEnemy(int damage)
	{
		StartCoroutine(ShowBattleWindow());
		GameObject damageText = Instantiate(floatingTextPrefab, battleWindowRightSideFloatTextLocation);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(150);

		enemyHealth -= damage;
	}

	public void PlayerTakeDamage(int damage)
	{
		GameObject damageText = Instantiate(floatingTextPrefab);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(150);

		playerHealth -= damage;
	}



	public void PlayerTakeAttack(int damage, bool guarded)
	{
		StartCoroutine(ShowBattleWindow());  

		GameObject damageText = Instantiate(floatingTextPrefab, battleWindowLeftSideFloatTextLocation);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(150);

		if(guarded)
		damageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);

		playerHealth -= damage;

	}
	public void ServentTakeAttack(int defenderDamage, int attackerDamage, bool guarded)
	{

		StartCoroutine(ShowBattleWindow());
		GameObject defenderDamageText = Instantiate(floatingTextPrefab, battleWindowLeftSideFloatTextLocation);
		defenderDamageText.GetComponent<FloatingDamageText>().SetDamageText(defenderDamage);
		defenderDamageText.GetComponent<FloatingDamageText>().SetFont(150);


		GameObject attackerDamageText = Instantiate(floatingTextPrefab, battleWindowRightSideFloatTextLocation);
		attackerDamageText.GetComponent<FloatingDamageText>().SetDamageText(attackerDamage);
		attackerDamageText.GetComponent<FloatingDamageText>().SetFont(150);

		if (guarded)
			defenderDamageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);

	}


	

	public IEnumerator EndAttackLine(EMouseOnArea mouseOnArea, bool isUsuable)
	{
		Field attackerField = ReturnMouseOnField(mouseOnArea);
		Field defenderField = ReturnMouseOnField();


		attackDragLine.positionCount = 0;
		if (defenderField != attackerField)
		{
			if (defenderField == enemyField)
			{
				int attackerForce = attackerField.GetForce();

				attackerForce += enemyDamageIncrease;
				attackerForce -= enemyDamageDecrease;

				if (attackerForce < 0)
				{ attackerForce = 0; }

				if (enemyDamageBlock)
				{ attackerForce = 0; }

				GameObject leftActor = Instantiate(
						playerServentPrefabList[cardHashMap[attackerField.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);

				GameObject rightActor = Instantiate(enemyObject, new Vector3(), Utils.QI);

				battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(leftActor);
				leftActor.GetComponent<Servent>().OnBattleWindow();
				battleWindowRightSide.GetComponent<BattleWindow>().SetActor(rightActor);
				rightActor.GetComponent<SpriteRenderer>().sortingOrder = 104;


				//battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(Instantiate(
				//	playerServentPrefabList[cardHashMap[attackerField.GetCardData().GetCardNum()]], new Vector3(), Utils.QI));
				//battleWindowRightSide.GetComponent<BattleWindow>().SetActor(Instantiate(enemyObject, new Vector3(), Utils.QI));

				AttackToEnemy(attackerForce);

				yield return new WaitForSeconds(2f);


				attackerField.SetAttacked(true);

				StartCoroutine(CheckEnemyCondition(2f));

			}
			else
			{
				if (isUsuable)
				{
					int attackerForce = attackerField.GetForce();
					int defenderForce = defenderField.GetForce();

					int attackerDamage = Math.Abs(defenderForce);
					int defenderDamage = Math.Abs(attackerForce);

					GameObject leftActor = Instantiate(
					playerServentPrefabList[cardHashMap[attackerField.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);

					GameObject rightActor = Instantiate(
							enemyServentPrefabList[cardHashMap[defenderField.GetCardData().GetCardNum()]], new Vector3(), Utils.QI);


					battleWindowLeftSide.GetComponent<BattleWindow>().SetActor(leftActor);
					leftActor.GetComponent<Servent>().OnBattleWindow();

					battleWindowRightSide.GetComponent<BattleWindow>().SetActor(rightActor);
					rightActor.GetComponent<Servent>().OnBattleWindow();



					ServentTakeAttack(attackerDamage, defenderDamage, false);
					yield return new WaitForSeconds(2f);

					attackerField.TakeAttack(attackerDamage);
					defenderField.TakeAttack(defenderDamage);

					if (attackerField.GetPenetrate())
					{
						defenderDamage = Math.Abs(defenderForce - attackerForce);

						if (enemyDamageBlock)
						{
							defenderDamage = 0;
							DealDamageToEnemy(defenderDamage);
						}
						else if (defenderDamage == 0)
						{ }
						else
						{
							phaseFlag = false;
							DealDamageToEnemy(defenderDamage);
							
							yield return StartCoroutine(ActivateCardDesc(new CrescentLancer(), enemyField.transform, 1));

						}

					}


					StartCoroutine(CheckEnemyCondition(2f));
					attackerField.SetAttacked(true);

				}
			}
		}



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
			targetPoint = playerField.transform.position;
			break;

			case EMouseOnArea.Enemy:
			targetPoint = enemyField.transform.position;
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
			targetPoint = playerField.transform.position;
			break;

			case EMouseOnArea.Enemy:
			targetPoint = enemyField.transform.position;
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
		SceneManager.LoadScene("Dungeon");
	}

	
	public void ParryCircle()
	{
		Vector3 targetScale = bigCircle.transform.localScale;
		smallCircle.transform.DOScale(targetScale, circleSpeed).SetEase(Ease.Linear)
		.OnComplete(() => 
		{
			smallCircle.transform.localScale = new Vector3(0,0,0);
			isActionDone = true;
		});
	}
}
