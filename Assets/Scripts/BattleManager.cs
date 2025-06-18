using DG.Tweening;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum EEnemyAction{None, Summon, Attack, Ability}
public enum EServentType{None, Player, Enemy}
public enum ETurnState{None, Player, Enemy}
public enum ECardType{None ,Servent, Spell, Adventure}
public enum ECardRarity{None, Normal, Rare}
public enum EServentAttribute{None, Fire, Water, Earth, Wind, Dark, Light}
public enum ECardState{Nothing, CanMouseOver, CanMouseDrag}
public enum EMouseOnArea{None, Player, Enemy, Field_1, Field_2, Field_3, Field_4, Field_5, Field_6, AnyWhere, Hole}
public enum ECardTargetType{Selected, Select}
public enum EServentCondition{None, Void, Oblivion, Poison, Madness, Testament}
public enum EServentSize{Small, Middle, Big}
public enum EParryState{Idle, Parry, Succecced, Failed}

public class BattleManager : MonoBehaviour
{


	public AudioSource backGroundMusic;
	public AudioSource soundEffect;
	public AudioClip serventDeath;
	public AudioClip serventSummon;


	EParryState parryState;
	bool playerDamageBlock;
	bool enemyDamageBlock;
	int playerDamageDecrease;
	int enemyDamageDecrease;
	int playerDamageIncrease;
	int enemyDamageIncrease;



	

	ETurnState turnState;
	List<Enemy> enemies = new();
	Enemy currentEnemy;
	int enemyIndex = 0;

	public Image playerActor;
	public Image enemyActor;

	public List<Sprite> actorSpriteList;
	
	public GameObject floatingTextPrefab;
	public Transform enemyTransform;
	public Transform alertPoint; 
	public static BattleManager Inst{get; private set;}
	public Canvas canvas;
	public Camera camera;
	public RectTransform backgroundDetectArea;
	public RectTransform playerDetectArea;
	public RectTransform enemyDetectArea;
	public RectTransform holeDetectArea;
	public RectTransform fieldDetectArea_1;
	public RectTransform fieldDetectArea_2;
	public RectTransform fieldDetectArea_3;
	public RectTransform fieldDetectArea_4;
	public RectTransform fieldDetectArea_5;
	public RectTransform fieldDetectArea_6;
	public Field playerField;
	public Field enemyField;
	public Field field_1;
	public Field field_2;
	public Field field_3;
	public Field field_4;
	public Field field_5;
	public Field field_6;
	public GameObject hole;
	public List<GameObject> anyWhereAreas;

	public GameObject cardPrefab;
	public GameObject enemyPrefab;
	public List<GameObject> playerServentPrefabList;
	public List<GameObject> playerServentInfoList;
	public List<GameObject> enemyServentPrefabList;
	public List<GameObject> enemyServentInfoList;
	public Transform cardAreaBorderLeft;
	public Transform cardAreaBorderRight;
	public Transform selectedTargetLineEnd;

	public EMouseOnArea mouseOnArea;
	public List<BattleCardData> deckList;
	public List<BattleCardData> trashList;
	public List<BattleCardData> handList;
	private List<GameObject> cardObjectList;
	private Dictionary<ItemSO, int> inventory;
	WaitForSeconds delay05 = new WaitForSeconds(0.5f);
	WaitForSeconds delay07 = new WaitForSeconds(0.7f);
	public LineRenderer cardDragLine;
	public LineRenderer attackDragLine;
	public int lineCount;
	public List<GameObject> conditionMarkList;
	public List<GameObject> cardPrefabList;
	public List<GameObject> dummyCardPrefabList;

	public GameObject invisibleImage;

	private bool myTurn;
	private bool isLoading;


	public GameObject battleWindowLeftSide;
	public GameObject battleWindowRightSide;

	public Transform battleWindowLeftSideFloatTextLocation;

	public Transform battleWindowRightSideFloatTextLocation;

	public Transform battleWindowLeftSideFirstPosition;
	public Transform battleWindowLeftSideSecondPosition;
	public Transform battleWindowRightSideFirstPosition;
	public Transform battleWindowRightSideSecondPosition;


	public GameObject missile;
	public GameObject missileTarget;
	public Servent clickedServent;
	public GameObject clickedServentInfo;


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
	public GameObject cardSelectFrame;
	public GameObject cardSelectWindow;
	public GameObject trashWindow;
	public Image fadeImage;
	public Image flashImage;
	public SpriteRenderer smallCircle;
	public SpriteRenderer bigCircle;


	public GameObject alertMessage;

	private int selectedLimit;
	private bool isActionDone = false;
	bool isParryWindowActive = false;

	float parryWindowTime = 0.3f;
	float circleSpeed = 1f;
	public int playerHealth;
	public int enemyHealth;

	private List<int> currentAbilities;

	public void Dash()
	{
		StartCoroutine(ShowBattleWindow());   
	}

	private IEnumerator ShowBattleWindow()
	{
		//InOutElastic
		//OutBack
		foreach(GameObject card in cardObjectList)
		{card.GetComponent<BattleCardObject>().SetLock(true);}

		battleWindowLeftSide.transform.DOMove(battleWindowLeftSideSecondPosition.position,
		0.2f).SetEase(Ease.Linear);
		battleWindowRightSide.transform.DOMove(battleWindowRightSideSecondPosition.position,
		0.2f).SetEase(Ease.Linear);

		yield return new WaitForSeconds(0.2f);

		battleWindowLeftSide.transform.DOMove(battleWindowLeftSideSecondPosition.position + new Vector3(50, 0, 0),
		1.5f).SetEase(Ease.OutExpo);
		battleWindowRightSide.transform.DOMove(battleWindowRightSideSecondPosition.position + new Vector3(-50, 0, 0),
		1.5f).SetEase(Ease.OutExpo);
		yield return new WaitForSeconds(1.5f);

		battleWindowLeftSide.transform.DOMove(battleWindowLeftSideFirstPosition.position,
		0.2f).SetEase(Ease.InQuad);
		battleWindowRightSide.transform.DOMove(battleWindowRightSideFirstPosition.position,
		0.2f).SetEase(Ease.InQuad);

		yield return new WaitForSeconds(1f);
		isActionDone = true;

		foreach(GameObject card in cardObjectList)
		{card.GetComponent<BattleCardObject>().SetLock(false);}

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
		Inst = this;
		enemies = BattleData.enemies;
		GameSetup();
		isLoading = true;

		handList = new();
		selectedCards = new();
		mouseOnArea = EMouseOnArea.None;

		StartCoroutine(StartGameCo());
		StartCoroutine(FadeIn());
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
		
		float time = 0;
		Color color = fadeImage.color;
		
		while (time < 1f)
		{
			time += Time.deltaTime;
			color.a = Mathf.Lerp(1, 0, time / 1f); // 알파 값을 1 → 0으로 변경
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
			color.a = Mathf.Lerp(0, 1, time / 1f); // 알파 값을 0 → 1로 변경
			fadeImage.color = color;
			yield return null;
		}
	}

	public void RemoveSelectedCards(CardData cardData)
	{

		selectedCards.Remove(cardData);
	}

	public void CloseSelectedCards()
	{
		if(selectedCards.Count == selectedLimit)
		{
			isActionDone = true;
			cardSelectWindow.GetComponent<Window>().OnOff();

			for( int i = selectedCardLayoutGroup.transform.childCount - 1; i >= 0 ; --i )
			{Destroy(selectedCardLayoutGroup.transform.GetChild(i).gameObject );}
		}
		else
		{
			Debug.Log("카드를 선택하세요.");
		}
	}
	public void ShowSelectedCards(List<BattleCardData> targetList,ECardType cardType, int limit)
	{
		isActionDone = false;
		selectedLimit = limit;
		foreach(CardData cardData in targetList)
		{
			if(cardType == null ||cardData.GetCardType() == cardType)
			{
				GameObject cardObject = Instantiate(dummyCardPrefabList[cardData.GetCardNum()], selectedCardLayoutGroup.transform);
				GameObject cardFrameObject = Instantiate(cardSelectFrame, cardObject.transform);
				
				cardObject.GetComponent<DummyCard>().SetLock(true);
				cardFrameObject.GetComponent<CardSelectFrame>().SetCardData(cardData);
				cardFrameObject.transform.localPosition = new Vector3(0, 0, 0);
				cardFrameObject.transform.localScale = new Vector3(1, 1, 0);
			}

			
		}

		RectTransform rectTransform = selectedCardLayoutGroup.GetComponent<RectTransform>();

		int height = ((selectedCardLayoutGroup.transform.childCount / 2) * 680) +  550;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);

		cardSelectWindow.GetComponent<Window>().OnOff();
	}

	
	IEnumerator ActivateEnemyAbility(EnemyAbility enemyAbility)
	{
		yield return new WaitForSeconds(.3f);
		switch(enemyAbility.GetNum())
		{
			case "0": // 미지의 안개
			{
				if(field_1.GetFilled())
				{
					field_1.TakeDamage(1);
				}

				if(field_2.GetFilled())
				{
					field_2.TakeDamage(1);
				}

				if(field_3.GetFilled())
				{
					field_3.TakeDamage(1);
				}

				PlayerTakeDamage(1);
				break;
			}
			case "1": // 사나운 울음소리
			{
				List<Field> filledField = new();
				if(field_1.GetFilled())
				{filledField.Add(field_1);}

				if(field_2.GetFilled())
				{filledField.Add(field_2);}

				if(field_3.GetFilled())
				{filledField.Add(field_3);}

				int randomNum = Random.Range(0, filledField.Count);
				Field field = filledField[randomNum];

				field.TakeDamage(1);

				break;
			}

		}

	}




		
	IEnumerator ActivateSpell(SpellCardData cardData, Field selectedField)
	{
		yield return new WaitForSeconds(.5f);
		switch(cardData.GetSpellNum())
		{
			case 0: //듀플리케이트

			deckList.Add(selectedField.GetCardData());
			deckList.Add(selectedField.GetCardData());
			Shuffle();

			break;
		   

			case 3: //타오르는 심장
			selectedField.GainForce(selectedField.GetForce());
			break;

			case 4: //작은 것을 위한 희생
			
			int x = trashCount;
			
			foreach(BattleCardData card in trashList)
			{deckList.Add(card);}
			trashList.Clear();

			playerHealth -= x;
			break;

			case 5: //오직 침묵만이

			if(field_1.GetComponent<Field>().GetFilled())
			{field_1.GetComponent<Field>().Kill();}

			if(field_2.GetComponent<Field>().GetFilled())
			{field_2.GetComponent<Field>().Kill();}

			if(field_3.GetComponent<Field>().GetFilled())
			{field_3.GetComponent<Field>().Kill();}

			if(field_4.GetComponent<Field>().GetFilled())
			{field_4.GetComponent<Field>().Kill();}

			if(field_5.GetComponent<Field>().GetFilled())
			{field_5.GetComponent<Field>().Kill();}

			if(field_6.GetComponent<Field>().GetFilled())
			{field_6.GetComponent<Field>().Kill();}
			break;

			case 9: // 마스크월드
			{
				if(field_1.GetFilled())
				{field_1.GainForce(1);}

				if(field_2.GetFilled())
				{field_2.GainForce(1);}

				if(field_3.GetFilled())
				{field_3.GainForce(1);}

				if(field_4.GetFilled())
				{field_4.GainForce(1);}

				if(field_5.GetFilled())
				{field_5.GainForce(1);}

				if(field_6.GetFilled())
				{field_6.GainForce(1);}
				
				break;
			}

			case 10: // 투사의 의지
			{
				selectedField.GainForce(selectedField.GetForce());
				selectedField.SetSuicide(true);
				break;
			}

			case 11: // 절규하는 투사
			{
				selectedField.GainForce(selectedField.GetForce());
				selectedField.AddCondition(EServentCondition.Madness);
				break;
			}

		}

	}

	public void HealPlayer(int value)
	{
		playerHealth += value;
	}

	IEnumerator CheckEnemyCondition(float delay)
	{
		if(enemyHealth <= 0)
		{
			enemyHealth = 0;
			yield return new WaitForSeconds(delay);

			if(enemyIndex == enemies.Count - 1)
			{
				AlertMessage("전투에서 승리했습니다.");

				invisibleImage.SetActive(true);


				PlayerData.saveData.health = playerHealth;


				foreach (GameObject card in cardObjectList)
				{card.GetComponent<BattleCardObject>().HideAndReveal(true);}

				yield return new WaitForSeconds(0.3f);
				StartCoroutine(EnemyFieldClear());
				yield return new WaitForSeconds(1f);
				BackToDungeon();
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
				// GameObject damageText = Instantiate(floatingTextPrefab, alertPoint);
				// damageText.GetComponent<FloatingDamageText>().SetDamageText("Failed..!");
				// damageText.GetComponent<FloatingDamageText>().SetFont(150);
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

	public IEnumerator StartGameCo()
	{
		//GameSetup();
		isLoading = true;

		yield return new WaitForSeconds(0.35f);
		StartCoroutine(StartTurnCo());
	}

	public void EndBattle()
	{
		
	}

	void GameSetup()
	{
		trashCount = 0;
		deckCount = 0;
		costCount = 0;
		playerHealth = PlayerData.saveData.health;

		currentEnemy = enemies[enemyIndex];
		enemyHealth = currentEnemy.GetHealth();

		Dictionary<BattleCardData, int> deck = new Dictionary<BattleCardData, int>();
		List<CardData> cardDatabase = DataController.Inst.LoadCardDatabase();
		Dictionary<string, int> myDeck = PlayerData.saveData.deck;


		foreach(KeyValuePair<string, int> value in myDeck)
		{deck.Add(cardDatabase[Convert.ToInt32(value.Key)] as BattleCardData, value.Value);}

		deckList = new();
		cardObjectList = new();
		trashList = new();
		
		foreach(KeyValuePair<BattleCardData, int> value in deck)
		{
			for(int i = 0; i < value.Value; ++i)
			{deckList.Add(value.Key);}
		};

		Shuffle();

		currentAbilities = new();

		myTurn = true;
	}

	private void Shuffle()
	{
		for(int i = 0; i < 100; ++i)
		{
			int a = Random.Range(0, deckList.Count);
			int b = Random.Range(0, deckList.Count);
			BattleCardData c = deckList[a];
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


	IEnumerator CheckBattleAbility(Field start, Field end)
	{
		yield return new WaitForSeconds(0.5f);
		switch(end.GetCardData().GetServentNum())
		{
			case 1000:
			end.SetHealth(0);
			Debug.Log("뚜쉬뚜쉬");
			yield return new WaitForSeconds(0.5f);
			break;
		}
	}

	IEnumerator CheckDeathAbility(int serventNum)
	{
		yield return new WaitForSeconds(1f);
	}

	public void SetEnemyDamageBlock(bool value)
	{ this.enemyDamageBlock = value; }

	IEnumerator StartTurnCo()
	{        
		isLoading = true;
		turnState = ETurnState.None;

		field_1.GetComponent<Field>().SetAttacked(false);
		field_2.GetComponent<Field>().SetAttacked(false);
		field_3.GetComponent<Field>().SetAttacked(false);
		field_4.GetComponent<Field>().SetAttacked(false);
		field_5.GetComponent<Field>().SetAttacked(false);
		field_6.GetComponent<Field>().SetAttacked(false);

		enemyDamageBlock = false;

		yield return new WaitForSeconds(0.4f);
		foreach(GameObject card in cardObjectList)
		{card.GetComponent<BattleCardObject>().HideAndReveal(false);}
		yield return new WaitForSeconds(0.4f);

		if(myTurn)
		{
			if(handList.Count < 5)
			{
				int p = 5 - handList.Count;
				for(int i = 0; i < p; ++i)
				{
					yield return new WaitForSeconds(0.35f);
					DrawCard();
				}
			}
			else
			{DrawCard();}
		}
		yield return new WaitForSeconds(0.3f);
		turnState = ETurnState.Player;


		yield return delay07;
		isLoading = false;
	}

	public void StartEnemyTurn()
	{
		costCount = 0;
		if(turnState == ETurnState.Player)
		StartCoroutine(EnemyTurnCo());
	}
	public IEnumerator WinBattle()
	{
		yield return new WaitForSeconds(1f);
	}

	IEnumerator LoadNextEnemy()
	{
		AlertMessage("적을 쓰러트렸습니다.");
		yield return new WaitForSeconds(0.3f);

		StartCoroutine(EnemyFieldClear());

		yield return new WaitForSeconds(1f);
		enemyIndex++;
		currentEnemy = enemies[enemyIndex];
		enemyHealth = currentEnemy.GetHealth();

		AlertMessage("새로운 적이 나타났습니다.");
	}

	void AlertMessage(string message)
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

	public GameObject InstantiateCard(BattleCardData battleCardData)
	{
		GameObject cardPrefab = cardPrefabList[battleCardData.GetCardNum()];
		GameObject cardObject = Instantiate(cardPrefab, new Vector3(), Utils.QI);
		cardObject.transform.SetParent(canvas.transform);
		cardObjectList.Add(cardObject);


		cardObject.GetComponent<BattleCardObject>().Setup(battleCardData);

		cardObject.GetComponent<BattleCardObject>().SetCardOrder(handList.Count);
		handList.Add(battleCardData);
		CardAlignmentAlt();

		ShotDrawMissile(cardObject.transform);
		return cardObject;
	}

	public void SearchCardInDeck(BattleCardData targetCardData)
	{

		var cardDataToRemove = deckList.Find(cardData => cardData.GetCardNum() == targetCardData.GetCardNum());
		if (cardDataToRemove != null)
		{
			deckList.Remove(cardDataToRemove);
			InstantiateCard(targetCardData);
		}
	}



	public IEnumerator EnemyTurnCo()
	{
		turnState = ETurnState.Enemy;
		foreach(GameObject card in cardObjectList)
		{card.GetComponent<BattleCardObject>().HideAndReveal(true);}

		yield return new WaitForSeconds(0.3f);
		int actionToken = currentEnemy.GetActionToken();

		for(int i = 0; i < actionToken; ++i)
		{
			currentAbilities.Clear();
			isActionDone = false;

			if(currentEnemy.GetEnemyAbility() != null)
			{currentAbilities.Add(0);}

			List<Field> filledField = new();
			if(field_4.GetFilled())
			{
				filledField.Add(field_4);

				if(field_4.canUseAbility)
				currentAbilities.Add(1);
			}

			if(field_5.GetFilled())
			{
				filledField.Add(field_5);
				if(field_5.canUseAbility)
				currentAbilities.Add(2);
			}

			if(field_6.GetFilled())
			{
				filledField.Add(field_6);
				if(field_6.canUseAbility)
				currentAbilities.Add(3);
			}

			switch(SelectEnemyAction())
			{
				case EEnemyAction.Summon:
				{
					AlertMessage("적이 동료를 부릅니다.");
					List<Field> unfilledField = new();

					if(!field_4.GetFilled())
					unfilledField.Add(field_4);

					if(!field_5.GetFilled())
					unfilledField.Add(field_5);

					if(!field_6.GetFilled())
					unfilledField.Add(field_6);
					int randomNum = Random.Range(0, unfilledField.Count);

					Field field = unfilledField[randomNum];
					field.locked = true;
					List<EnemyServentCardData> serventList = currentEnemy.GetServentList();

					EnemyServentCardData randomServent = serventList[Random.Range(0, serventList.Count)];

					field.Summon(randomServent,
					Instantiate(enemyServentPrefabList[0],
					field.transform.position , Utils.QI));
					soundEffect.PlayOneShot(serventSummon);
					field.locked = false;

					isActionDone = true;
					break;
				}
				case EEnemyAction.Attack:
				{
					AlertMessage("적이 공격합니다.");
					
					List<Field> fields = new List<Field>(){playerField};

					if(field_1.GetFilled())
					fields.Add(field_1);

					if(field_2.GetFilled())
					fields.Add(field_2);

					if(field_3.GetFilled())
					fields.Add(field_3);

					List<Field> unAttackedField = new();

					foreach(Field field in filledField)
					{
						if(!field.GetAttacked())
						unAttackedField.Add(field);
					}

					Field startField = unAttackedField[Random.Range(0, unAttackedField.Count)];
					Field targetField = fields[Random.Range(0, fields.Count)];

					StartCoroutine(EnemyAttack(startField, targetField));
					break;
				}
				case EEnemyAction.Ability:
				{
					AlertMessage("적이 능력을 사용합니다.");
					int randomNum = currentAbilities[Random.Range(0, currentAbilities.Count)];
					switch(randomNum)
					{
						case 0:
						Debug.Log("대장이 능력을 사용합니다.");
						break;

						case 1:
						Debug.Log("1번 필드의 적이 능력을 사용합니다.");
						break;

						case 2:
						Debug.Log("2번 필드의 적이 능력을 사용합니다.");
						break;

						case 3:
						Debug.Log("3번 필드의 적이 능력을 사용합니다.");
						break;
					}

					isActionDone = true;
					break;
				}
				case EEnemyAction.None:
				{
					AlertMessage("적이 아무것도 할 수 없습니다.");
					isActionDone = true;
					break;
				}
			}

			yield return new WaitForSeconds(2.5f);
			
			yield return new WaitUntil(() => isActionDone);
			
		}
		isActionDone = false;

		StartCoroutine(StartTurnCo());
		
	}

	private EEnemyAction SelectEnemyAction()
	{
		List<Field> filledField = new();
		List<Field> attackedField = new();


		bool enemySummonable = true;
		bool enemyAttackable = true;
		bool abilityUsuable = currentAbilities.Count != 0;

		int probability = 0;
		
		if(field_4.GetFilled())
		{filledField.Add(field_4);}

		if(field_5.GetFilled())
		{filledField.Add(field_5);}

		if(field_6.GetFilled())
		{filledField.Add(field_6);}

		if(filledField.Count == 3)
		{enemySummonable = false;}

		probability += filledField.Count * 3;

		if(field_4.GetAttacked())
		{attackedField.Add(field_4);}

		if(field_5.GetAttacked())
		{attackedField.Add(field_5);}

		if(field_6.GetAttacked())
		{attackedField.Add(field_6);}

		if(attackedField.Count == filledField.Count || filledField.Count == 0)
		{enemyAttackable = false;}



		int randomNum = Random.Range(1,10);

		if(enemySummonable && probability < randomNum)
		return EEnemyAction.Summon;

		randomNum = Random.Range(0,2);

		if(enemyAttackable && randomNum == 1)
		return EEnemyAction.Attack;

		if(abilityUsuable)
		return EEnemyAction.Ability;

		return EEnemyAction.None;
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
		
		if(targetField == playerField)
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

			

			PlayerTakeAttack(attackerForce, parryState == EParryState.Succecced);
			startField.SetAttacked(true);
		}else
		{
			int attackerForce = startField.GetForce();
			int defenderForce = targetField.GetForce();

			int attackerDamage = Math.Abs(defenderForce);
			int defenderDamage = Math.Abs(attackerForce);

			if(parryState == EParryState.Succecced)
			{defenderDamage -= 1;}

			if(attackerForce < 0)
			{defenderDamage = 0;}

			startField.TakeDamage(attackerDamage);
			targetField.TakeDamage(defenderDamage);

			ServentTakeAttack(defenderDamage, parryState == EParryState.Succecced);

			if(startField.GetPenetrate())
			{
				defenderDamage = Math.Abs(defenderForce - attackerForce);
				
				if(playerDamageBlock)
				{defenderDamage = 0;}
				
				PlayerTakeDamage(defenderDamage);
			}
			startField.SetAttacked(true);
		}
		attackDragLine.positionCount = 0;
		

		Color originColor = bigCircle.color;
		Color targetColor = new Color(1f, 0.3f, 0.3f, 0.5f);


		if(parryState == EParryState.Succecced)
		{targetColor = new Color(0.3f, 0.3f, 1f, 0.5f);}
		


		bigCircle.DOColor(targetColor, 0.1f) // 빨간색으로 변경  
					 .SetLoops(3, LoopType.Yoyo) // 2번 반복  
					 .OnComplete(() => bigCircle.color = originColor); // 원래 색으로 복귀  
		// FlashMultipleTimes();

		parryState = EParryState.Idle;
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
		myTurn = !myTurn;
		StartCoroutine(StartTurnCo());
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

		{
			List<PreRequisite> preRequisites = cardData.GetPreRequisites();

			if(preRequisites == null)
			return true;

			bool flag = false;

			int count;


			foreach(PreRequisite value in preRequisites)
			{
				count = 0;
				switch(value.preRequisite)
				{
					
					case EPreRequisite.None:
					return true;

					case EPreRequisite.SelectedServent:
					if(ReturnMouseOnField().GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{return true;}
						else
						{
							if(value.serventAttribute == ReturnMouseOnField().GetServentAttribute())
							{return true;}
						}
						
					}
					return false;
					

					case EPreRequisite.AllServentCount:
					if(field_1.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_1.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_2.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_2.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_3.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_3.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_4.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_4.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_5.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_5.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_6.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_1.GetServentAttribute())
							{count++;}
						}
						
					}

					flag = count == value.count;
					break;

					case EPreRequisite.AllServentCountOver:
					count = 0;

					if(field_1.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_1.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_2.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_2.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_3.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_3.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_4.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_4.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_5.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_5.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_6.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_1.GetServentAttribute())
							{count++;}
						}
						
					}

					flag = count > value.count;
					break;

					case EPreRequisite.AllServentCountUnder:
					count = 0;

					if(field_1.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_1.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_2.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_2.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_3.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_3.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_4.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_4.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_5.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_5.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_6.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_1.GetServentAttribute())
							{count++;}
						}
						
					}

					flag = count < value.count;
					break;

					case EPreRequisite.PlayerServentCount:
					count = 0;
					

					if(field_1.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_1.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_2.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_2.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_3.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_3.GetServentAttribute())
							{count++;}
						}
						
					}

					flag = count == value.count;
					break;

					case EPreRequisite.PlayerServentCountOver:
					count = 0;

					if(field_1.GetFilled())
					{
						if(value.serventAttribute == field_1.GetServentAttribute() ||
						 value.serventAttribute == EServentAttribute.None)
						{count++;}
						
					}
					if(field_2.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_2.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_3.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_3.GetServentAttribute())
							{count++;}
						}
						
					}

					flag = count > value.count;
					break;

					case EPreRequisite.PlayerServentCountUnder:
					count = 0;

					if(field_1.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_1.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_2.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_2.GetServentAttribute())
							{count++;}
						}
						
					}
					if(field_3.GetFilled())
					{
						if(value.serventAttribute == EServentAttribute.None)
						{count++;}
						else
						{
							if(value.serventAttribute == field_3.GetServentAttribute())
							{count++;}
						}
						
					}

					flag = count < value.count;
					break;

					case EPreRequisite.TrashCountOver:

					if(value.cardType == ECardType.None)
					{flag = trashCount > value.count;}

					if(value.cardType == ECardType.Servent)
					{
						int serventCardCount = 0;

						foreach(CardData card in trashList)
						{
							if(card.GetCardType() == ECardType.Servent)
							{serventCardCount++;}
						}
						flag = serventCardCount > value.count;
					}

					if(value.cardType == ECardType.Spell)
					{
						int spellCardCount = 0;

						foreach(CardData card in trashList)
						{
							if(card.GetCardType() == ECardType.Spell)
							{spellCardCount++;}
						}
						flag = spellCardCount > value.count;

						
					}
					
					break;

					case EPreRequisite.PlayerHPCount:
					flag = playerHealth == value.count;
					break;

					case EPreRequisite.PlayerHPCountOver:
					flag = playerHealth > value.count;
					break;

					case EPreRequisite.PlayerHPCountUnder:
					flag = playerHealth < value.count;
					break;

					case EPreRequisite.DeckCountOver:
					{

						switch(value.cardType)
						{
							case ECardType.None:
							{
								if(value.cardNum == 0)
								{flag = deckCount > value.count;}
								else
								{
									int cardCount = 0;
									foreach(CardData card in deckList)
									{
										if(card.GetCardNum() == value.cardNum)
										cardCount++;
									}
									flag = cardCount > value.count;
								}  
								break;
							}

							case ECardType.Servent:
							{
								int serventCardCount = 0;

								foreach(CardData card in deckList)
								{
									if(card.GetCardType() == ECardType.Servent)
									{serventCardCount++;}
								}
								flag = serventCardCount > value.count;
								break;
							}

							case ECardType.Spell:
							{
								int spellCardCount = 0;

								foreach(CardData card in deckList)
								{
									if(card.GetCardType() == ECardType.Spell)
									{spellCardCount++;}
								}
								flag = spellCardCount > value.count;
								break;
							}
						}
						break;
					}
				}

				if(!flag)
				{return flag;}
			}
			return flag;
		}
	}

	public void CardBeginDrag(GameObject cardObject)
	{
		if(cardObject.GetComponent<BattleCardObject>().GetCardData().GetCardTargetType() == ECardTargetType.Selected)
		{
			foreach(GameObject gameObject in anyWhereAreas)
			{gameObject.SetActive(true);}
		}

		foreach(GameObject card in cardObjectList)
		{card.GetComponent<BattleCardObject>().SetLock(true);}
		cardObject.GetComponent<BattleCardObject>().SetLock(false);
	}

	public void CardOnDrag(GameObject cardObject)
	{
		if(cardObject.GetComponent<BattleCardObject>().GetCardData().GetCardType() == ECardType.Servent)
		{
			DrawDragLine(cardObject.transform.position,
			CheckServentSummonable(cardObject.GetComponent<BattleCardObject>().GetCardData(),
			cardObject.GetComponent<BattleCardObject>().GetCurrentCost(),ReturnMouseOnField())
			);
		}
		else{

			SpellCardData spellCardData = cardObject.GetComponent<BattleCardObject>().GetCardData() as SpellCardData;
			//DrawDragLine(cardObject.transform.position,
			//CheckCardUsable(cardObject.GetComponent<BattleCardObject>().GetCardData(),
			//cardObject.GetComponent<BattleCardObject>().GetCurrentCost(),ReturnMouseOnField())
			//);
			DrawDragLine(cardObject.transform.position,
				spellCardData.IsSpellUsable(this) && CheckCardUsable());


		}
	}

	public bool IsOnEmptyField()
	{ return !ReturnMouseOnField().GetFilled(); }

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

	public IEnumerator CardEndDrag(BattleCardObject card, Field targetField)
	{
		foreach(GameObject gameObject in anyWhereAreas)
		{gameObject.SetActive(false);}

		


		DeleteDragLine();

		isActionDone = false;

		if(mouseOnArea == EMouseOnArea.Hole)
		{
			handList.RemoveAt(card.GetCardOrder());
			cardObjectList.Remove(card.gameObject);
			AddTrash(card.GetCardData());
			card.SendMissile(alertPoint, hole.transform);
			costCount++;

			List<BattleCardData> newHandList = new List<BattleCardData>();

			foreach(BattleCardData cardData in handList)
			{newHandList.Add(cardData);}

			for(int i = 0; i < cardObjectList.Count; ++i)
			{cardObjectList[i].GetComponent<BattleCardObject>().SetCardOrder(i);}

			handList = newHandList;
			CardAlignmentAlt();
		}
		else
		{
			if(card.GetCardType() == ECardType.Servent)
			{
				ServentCardData serventCardData = card.GetCardData() as ServentCardData;
				if(CheckServentSummonable(serventCardData, card.GetComponent<BattleCardObject>().GetCurrentCost(), targetField))
				{
					targetField.locked = true;
					costCount -= serventCardData.GetCardCost();

					cardObjectList.Remove(card.gameObject);
					handList.RemoveAt(card.GetCardOrder());

					card.SendMissile(alertPoint, ReturnMouseOnField().transform);
					foreach(GameObject cardObject in cardObjectList)
					{cardObject.GetComponent<BattleCardObject>().SetLock(true);}
					
					yield return new WaitForSeconds(1.5f);
					
					foreach(GameObject cardObject in cardObjectList)
					{cardObject.GetComponent<BattleCardObject>().SetLock(false);}

					targetField.Summon(
						serventCardData,
						Instantiate(
							playerServentPrefabList[serventCardData.GetServentNum()],
							targetField.transform.position,
							Utils.QI
						)
					);

					soundEffect.PlayOneShot(serventSummon);

					//StartCoroutine(ActivateSummonAbility(
					//	serventCardData,
					//	card.GetComponent<BattleCardObject>().GetCurrentCost(),
					//	targetField
					//));
					StartCoroutine(serventCardData.SummonEffectExecute(this));

					for (int i = 0; i < cardObjectList.Count; ++i)
					{cardObjectList[i].GetComponent<BattleCardObject>().SetCardOrder(i);}

					CardAlignmentAlt();
				}
			}
			else
			{
				SpellCardData spellCardData = card.GetCardData() as SpellCardData;

				if(spellCardData.IsSpellUsable(this) && card.GetComponent<BattleCardObject>().GetCurrentCost() == 0)
				{
					costCount -= spellCardData.GetCardCost();
					// StartCoroutine(ActivateSpell(card.GetCardData(), targetField));
					spellCardData.SpellEffectExecute(this);

					StartCoroutine(spellCardData.SpellEffectExecute(this));


					AddTrash(card.GetCardData());
					handList.RemoveAt(card.GetCardOrder());
					cardObjectList.Remove(card.gameObject);

					card.SendMissile(alertPoint, hole.transform);

					for (int i = 0; i < cardObjectList.Count; ++i)
					{ cardObjectList[i].GetComponent<BattleCardObject>().SetCardOrder(i); }

					CardAlignmentAlt();
					StartCoroutine(spellCardData.SpellEffectExecute(this));
				}

			}

				//if (CheckCardUsable(card.GetCardData(), card.GetComponent<BattleCardObject>().GetCurrentCost(), targetField))
				//{
				//	switch (card.GetCardType())
				//	{
				//		case ECardType.Spell:
							
				//			break;
				//	}
				//}

			
		}

		foreach(GameObject cardObject in cardObjectList)
		{cardObject.GetComponent<BattleCardObject>().SetLock(false);}
	}
	public void DrawCard()
	{
		if(deckList.Count == 0 && trashList.Count == 0)
		{return;}


		List<BattleCardData> targetList;

		if(deckList.Count != 0)
		{targetList = deckList;}
		else
		{
			PlayerTakeDamage(1);
			targetList = trashList;
		}

		BattleCardData cardData = targetList[targetList.Count - 1];

		cardPrefab = cardPrefabList[cardData.GetCardNum()];



		
		GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
		cardObject.transform.SetParent(canvas.transform);
		cardObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
		cardObjectList.Add(cardObject);
		
		cardObject.GetComponent<BattleCardObject>().Setup(cardData);
		
		cardObject.GetComponent<BattleCardObject>().SetCardOrder(handList.Count);
		handList.Add(cardData);


		

		targetList.RemoveAt(targetList.Count - 1);
		

		CardAlignmentAlt();
		ShotDrawMissile(cardObject.transform);
	}

	// public void DrawCard()
	// {

	//     if(deckList.Count == 0 && trashList.Count == 0)
	//     {return;}

	//     GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
	//     cardObject.SetActive(false);
	//     CardData cardData = deckList[deckList.Count - 1];

	//     cardObjectList.Add(cardObject);
	//     handList.Add(cardData);

	//     deckList.RemoveAt(deckList.Count - 1);

	//     CardAlignmentAlt();

	//     StartCoroutine(CreateMissile(hole, cardObjectList[cardObjectList.Count - 1]));
	//     cardObject.SetActive(true);
	//     CardAlignment();
	// }


	public void SetMouseOnField(EMouseOnArea mouseOnArea)
	{this.mouseOnArea = mouseOnArea;}

	public void ResetMouseOnField()
	{mouseOnArea = EMouseOnArea.None;}

	public void SelectTarget(GameObject field)
	{missileTarget = field;}

	public void CardAlignmentAlt()
	{
		if(handList.Count == 0)
		{return;}

		List<PRS> originCardPRSs = new List<PRS>();

		originCardPRSs = RoundAlignment(cardAreaBorderLeft, cardAreaBorderRight, cardObjectList.Count, 0.5f, Vector3.one * 2.3f);
		for(int i = 0; i < cardObjectList.Count; ++i)
		{
			var targetCard = cardObjectList[i];
			targetCard.GetComponent<BattleCardObject>().originPRS = originCardPRSs[i];
			targetCard.transform.position = originCardPRSs[i].pos;

			targetCard.GetComponent<BattleCardObject>().UpdateCardCost(costCount);
		}

	}
	List<PRS> GetCardAlignment(Vector3 leftBoundary, Vector3 rightBoundary, int cardCount, float spacing)
	{
		
		List<PRS> result = new List<PRS>();

		for (int i = 0; i < cardCount; ++i)
		{
			float t = (float)i / (cardCount - 1); // Normalize index
			Vector3 position = Vector3.Lerp(leftBoundary, rightBoundary, t);
			Quaternion rotation = Quaternion.identity;
			Vector3 scale = Vector3.one; // Default scale
			result.Add(new PRS(position, rotation, scale));
		}

		return result;
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


	public void CardAlignment()
	{

		List<PRS> originCardPRSs = RoundAlignment(cardAreaBorderLeft, cardAreaBorderRight, cardObjectList.Count, 0.5f, Vector3.one * 2.3f);
		for(int i = 0; i < cardObjectList.Count; ++i)
		{
			var targetCard = cardObjectList[i];
			targetCard.GetComponent<BattleCardObject>().originPRS = originCardPRSs[i];
			targetCard.GetComponent<BattleCardObject>().MoveTransform(targetCard.GetComponent<BattleCardObject>().originPRS, true, 0.7f);

		}
	}
	public void DeleteDragLine()
	{
		cardDragLine.positionCount = 0;
		cardDragLine.endColor = Color.blue;
	}

	public void EnemyTakeDamage(int damage)
	{
		
		GameObject damageText = Instantiate(floatingTextPrefab, enemyDetectArea);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(150);

		enemyHealth -= damage;

		StartCoroutine(CheckEnemyCondition(0.3f));
	}

	public void EnemyTakeAttack(int damage)
	{
		StartCoroutine(ShowBattleWindow());
		GameObject damageText = Instantiate(floatingTextPrefab, battleWindowRightSideFloatTextLocation);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(150);

		enemyHealth -= damage;

		StartCoroutine(CheckEnemyCondition(2.2f));
	}

	public void PlayerTakeDamage(int damage)
	{
		GameObject damageText = Instantiate(floatingTextPrefab, playerDetectArea);
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
	public void ServentTakeAttack(int damage, bool guarded)
	{

		Dash();
		GameObject damageText = Instantiate(floatingTextPrefab, battleWindowLeftSideFloatTextLocation);
		damageText.GetComponent<FloatingDamageText>().SetDamageText(damage);
		damageText.GetComponent<FloatingDamageText>().SetFont(150);

		if(guarded)
		damageText.GetComponent<FloatingDamageText>().SetColor(Color.blue);
	}


	

	public void EndAttackLine(EMouseOnArea mouseOnArea, bool isUsuable)
	{
		if(ReturnMouseOnField() == ReturnMouseOnField(mouseOnArea))
		{return;}

		if(ReturnMouseOnField() == enemyField)
		{
			int attackerForce = ReturnMouseOnField(mouseOnArea).GetForce();

			attackerForce += enemyDamageIncrease;
			attackerForce -= enemyDamageDecrease;

			if(attackerForce < 0)
			{attackerForce = 0;}

			if(enemyDamageBlock)
			{attackerForce = 0;}
			

			EnemyTakeAttack(attackerForce);

			
			
			ReturnMouseOnField(mouseOnArea).SetAttacked(true);

			
		}else
		{
			if(isUsuable)
			{
				int attackerForce = ReturnMouseOnField(mouseOnArea).GetForce();
				int defenderForce = ReturnMouseOnField().GetForce();

				int attackerDamage = Math.Abs(defenderForce);
				int defenderDamage = Math.Abs(attackerForce);

				ReturnMouseOnField(mouseOnArea).TakeDamage(attackerDamage);
				ReturnMouseOnField().TakeDamage(defenderDamage);

				if(ReturnMouseOnField(mouseOnArea).GetPenetrate())
				{
					defenderDamage = Math.Abs(defenderForce - attackerForce);

					if(enemyDamageBlock)
					{
						defenderDamage = 0;
						EnemyTakeDamage(defenderDamage);
					}
					else if(defenderDamage == 0)
					{}
					else
					{EnemyTakeDamage(defenderDamage);}

				}
				
				StartCoroutine(CheckBattleAbility(ReturnMouseOnField(mouseOnArea), ReturnMouseOnField()));
				
				ReturnMouseOnField(mouseOnArea).SetAttacked(true);

			}
		}
		attackDragLine.positionCount = 0;
		
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
			targetPoint = camera.ScreenToWorldPoint(playerDetectArea.position);
			break;

			case EMouseOnArea.Enemy:
			targetPoint = camera.ScreenToWorldPoint(enemyDetectArea.position);
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

	public void ShowTrashCards()
	{
		foreach(CardData cardData in trashList)
		{
			GameObject cardObject = Instantiate(dummyCardPrefabList[cardData.GetCardNum()], trashLayoutGroup.transform);
			GameObject cardFrameObject = Instantiate(cardSelectFrame, cardObject.transform);
			
			cardObject.GetComponent<DummyCard>().SetLock(true);
			cardFrameObject.GetComponent<CardSelectFrame>().SetCardData(cardData);
			cardFrameObject.transform.localPosition = new Vector3(0, 0, 0);
			cardFrameObject.transform.localScale = new Vector3(1, 1, 0);
		}

		foreach(GameObject cardObject in cardObjectList)
		{cardObject.GetComponent<BattleCardObject>().SetLock(true);}

		RectTransform rectTransform = trashLayoutGroup.GetComponent<RectTransform>();

		int height = ((trashLayoutGroup.transform.childCount / 2) * 480) +  550;
		rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);

		trashWindow.GetComponent<Window>().OnOff();
	}

	public void CloseTrashCards()
	{
		for( int i = trashLayoutGroup.transform.childCount - 1; i >= 0 ; --i )
		{Destroy(trashLayoutGroup.transform.GetChild(i).gameObject );}


		foreach(GameObject cardObject in cardObjectList)
		{cardObject.GetComponent<BattleCardObject>().SetLock(false);}

		trashWindow.GetComponent<Window>().OnOff();
	}

	public void AddTrash(BattleCardData cardData)
	{    
		trashList.Add(cardData);
	}

	public void RemoveTrash(BattleCardData cardData)
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
			targetPoint = holeDetectArea.position;
			break;

			case EMouseOnArea.Player:
			targetPoint = camera.ScreenToWorldPoint(playerDetectArea.position);
			break;

			case EMouseOnArea.Enemy:
			targetPoint = camera.ScreenToWorldPoint(enemyDetectArea.position);
			break;
			
			case EMouseOnArea.AnyWhere:
			//targetPoint = selectedTargetLineEnd.position;
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
