using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public enum ECardType{None ,Servent, Spell}
public enum EServentAttribute{None, Fire, Water, Earth, Wind, Dark, Light}
public enum ECardState{Nothing, CanMouseOver, CanMouseDrag}
public enum EMouseOnArea{None, Player, Enemy, Field_1, Field_2, Field_3, Field_4, Field_5, Field_6, AnyWhere, Hole}
public enum ECardTargetType{Selected, Select}
public enum EServentCondition{None, Void, Oblivion, Poison, Madness, Testament}
public enum EServentSize{Small, Middle, Big}

public class BattleManagerAlt : MonoBehaviour
{
    bool playerDamageBlock;
    bool enemyDamageBlock;
    int playerDamageDecrease;
    int enemyDamageDecrease;
    int playerDamageIncrease;
    int enemyDamageIncrease;


    public Transform alertPoint; 
    public static BattleManagerAlt Inst{get; private set;}
    void Awake() => Inst = this;
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
    public Field player;
    public Field enemy;
    public Field field_1;
    public Field field_2;
    public Field field_3;
    public Field field_4;
    public Field field_5;
    public Field field_6;
    public GameObject hole;
    public GameObject aura;


    public List<GameObject> anyWhereAreas;
    //Prefab
    public GameObject cardPrefab;
    public List<GameObject> serventPrefabList;
    public List<GameObject> serventInfoList;

    //나중에 리스트로 카드와 소환수 Prefab 하나하나 만들어 넣고 번호 순서대로 넣을 예정

    public GameObject draggedCard;
    public Button monsterAbilityButton;
    public Button monsterDetailButton;
    public GameObject monsterConditionPanel;
    public GameObject monsterDetailPanel;
    public Transform cardSpawnPoint;
    public Transform cardAreaBorderLeft;
    public Transform cardAreaBorderRight;
    public Transform selectedTargetLineEnd;

    public EMouseOnArea mouseOnArea;
    public TMP_Text parryText;
    private List<CardData> deckList;
    private List<CardData> trashList;
    private List<CardData> handList;
    private List<GameObject> cardObjectList;
    private Dictionary<ItemSO, int> inventory;
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);
    public LineRenderer cardDragLine;
    public LineRenderer attackDragLine;
    public int lineCount;
    private CardTargetingSystem targetingSystem;
    enum EParryState{Idle, Parry}
    public List<GameObject> conditionMarkList;
    public List<GameObject> cardPrefabList;
    public List<GameObject> dummyCardPrefabList;

    // 현재 지불해놓은 코스트의 수

    private int turn;
    //진행된 턴의 수

    private bool myTurn;
    public bool isLoading;
    public int startCardCount;
    public bool fastMode;
    private EParryState parryState;
    private bool justGuard;

    public GameObject missile;
    public GameObject missileTarget;
    public Servent clickedServent;
    public GameObject clickedServentInfo;
    public int shot = 1;
    private CardEffectSystem effectSystem;

    public TMP_Text costCountText;
    public TMP_Text deckCountText;
    public TMP_Text trashCountText;
    public TMP_Text playerHealthText;
    public TMP_Text enemyHealthText;


    private int costCount;
    private int deckCount;
    private int trashCount;

    private int playerHealth;
    private int enemyHealth;

    public List<CardData> selectedCards;

    public GridLayoutGroup selectedCardLayoutGroup;
    public GridLayoutGroup trashLayoutGroup;

    public GameObject cardSelectFrame;
    public GameObject cardSelectWindow;
    public GameObject trashWindow;


    public Scrollbar scrollbar;

    private int selectedLimit;
    private bool isActionDone = false;

    // public void StartAction()
    // {isActionDone = false;}

    public void ActionDone()
    {isActionDone = true;}




    void Start()
    {
        GameSetup();
        isLoading = true;

        handList = new();
        selectedCards = new();
        mouseOnArea = EMouseOnArea.None;

        StartCoroutine(StartGameCo());
    }

    public bool AddSelectedCards(CardData cardData)
    {
        bool foo = selectedCards.Count < selectedLimit;

        if(foo)
        {selectedCards.Add(cardData);}

        return foo;
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
            {
                Destroy( selectedCardLayoutGroup.transform.GetChild(i).gameObject );
            }
        }
        else
        {
            Debug.Log("카드를 선택하세요.");
        }
    }

    public void ShowSelectedCards(List<CardData> targetList,ECardType cardType, int limit)
    {
        isActionDone = false;
        selectedLimit = limit;
        foreach(CardData cardData in targetList)
        {
            if(cardType == null ||cardData.GetCardType() == cardType)
            {
                GameObject cardObject = Instantiate(cardPrefabList[cardData.GetCardNum()], selectedCardLayoutGroup.transform);
                GameObject cardFrameObject = Instantiate(cardSelectFrame, cardObject.transform);
                
                cardObject.GetComponent<Card>().SetLock(true);
                cardFrameObject.GetComponent<CardSelectFrame>().SetCardData(cardData);
                cardFrameObject.transform.localPosition = new Vector3(0, 0, 0);
                cardFrameObject.transform.localScale = new Vector3(1, 1, 0);
            }

            
        }

        cardSelectWindow.GetComponent<Window>().OnOff();
    }

    IEnumerator ActivateTreatmentAbility(CardData cardData, Field field)
    {
        switch(cardData.GetServentNum())
        {

        }
        yield return new WaitUntil(() => isActionDone);
    }





    IEnumerator ActivateSummonAbility(CardData cardData, int currentCost, Field field)
    {
        yield return new WaitForSeconds(.3f);
        if(CheckCardUsable(cardData, currentCost, field))
        switch(cardData.GetServentNum())
        {
            case 1: //바이올렛 리치 로드
            {
                ShowSelectedCards(trashList, ECardType.Spell, 1);
                yield return new WaitUntil(() => isActionDone);
                
                CardData card = selectedCards[0];
                RemoveTrash(card);
                cardPrefab = cardPrefabList[card.GetCardNum()];

                GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
                cardObject.transform.SetParent(canvas.transform);
                cardObjectList.Add(cardObject);
                
                cardObject.GetComponent<Card>().Setup(card);
                
                cardObject.GetComponent<Card>().SetCardOrder(handList.Count);
                handList.Add(card);

                selectedCards = new();

                isActionDone = false;

                CardAlignmentAlt();
                
                ShotDrawMissile(cardObject.transform);
                break;
            }

            case 2: //암흑요리사
            {
                CardData card = new Stew();
                CardData targetCard = new();
                int count = 0;

                foreach(CardData value in deckList)
                {
                    if(value.GetCardNum() == 7)
                    {
                        targetCard = value;
                        count++;
                    }
                }

                if(count > 3)
                {count = 2;}

                for(int i = 0; i < count; ++i)
                {
                    deckList.Remove(targetCard);
                    cardPrefab = cardPrefabList[card.GetCardNum()];

                    GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
                    cardObject.transform.SetParent(canvas.transform);
                    cardObjectList.Add(cardObject);
                    
                    cardObject.GetComponent<Card>().Setup(card);
                    
                    cardObject.GetComponent<Card>().SetCardOrder(handList.Count);
                    handList.Add(card);
                }
                

                CardAlignmentAlt();
                break;
            }

            case 4: //불의 정령 크림슨
            {
                CardData card = new WaterHeize();
                CardData targetCard = new();

                foreach(CardData value in deckList)
                {
                    if(value.GetCardNum() == 14)
                    targetCard = value;
                }
                deckList.Remove(targetCard);
                cardPrefab = cardPrefabList[card.GetCardNum()];

                GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
                cardObject.transform.SetParent(canvas.transform);
                cardObjectList.Add(cardObject);
                
                cardObject.GetComponent<Card>().Setup(card);
                
                cardObject.GetComponent<Card>().SetCardOrder(handList.Count);
                handList.Add(card);

                CardAlignmentAlt();

                ShotDrawMissile(cardObject.transform);
                break;
            }

            

            

            case 5: //물의 정령 헤이즈
            {
                CardData card = new FireCrimson();
                CardData targetCard = new();

                foreach(CardData value in deckList)
                {
                    if(value.GetCardNum() == 13)
                    targetCard = value;
                    
                }
                deckList.Remove(targetCard);
                cardPrefab = cardPrefabList[card.GetCardNum()];

                GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
                cardObject.transform.SetParent(canvas.transform);
                cardObjectList.Add(cardObject);
                
                cardObject.GetComponent<Card>().Setup(card);
                
                cardObject.GetComponent<Card>().SetCardOrder(handList.Count);
                handList.Add(card);

                CardAlignmentAlt();
                
                ShotDrawMissile(cardObject.transform);
                break;
            }

            case 6: //바람의 정령 크래스트
            {
                if(field_1.GetFilled())
                {
                    if(field_1.GetServentAttribute() == EServentAttribute.Wind)
                    {field_1.GainForce(1);}
                }

                if(field_2.GetFilled())
                {
                    if(field_2.GetServentAttribute() == EServentAttribute.Wind)
                    {field_2.GainForce(1);}
                }

                if(field_3.GetFilled())
                {
                    if(field_3.GetServentAttribute() == EServentAttribute.Wind)
                    {field_3.GainForce(1);}
                }

                break;
            }
        }
        isActionDone = false;
    }

        
    IEnumerator ActivateSpell(CardData cardData)
    {
        yield return new WaitForSeconds(.5f);
        switch(cardData.GetSpellNum())
        {
            case 0: //듀플리케이트

            deckList.Add(ReturnMouseOnField().GetCardData());
            deckList.Add(ReturnMouseOnField().GetCardData());
            Shuffle();

            break;

            case 1: //엘리멘탈 부스트
            {
                 List<EServentAttribute> attributes = new();
                if(field_1.GetFilled())
                {
                    if(!attributes.Contains(field_1.GetServentAttribute()))
                    {attributes.Add(field_1.GetServentAttribute());}
                }

                if(field_2.GetFilled())
                {
                    if(!attributes.Contains(field_2.GetComponent<Field>().GetServentAttribute()))
                    {attributes.Add(field_2.GetComponent<Field>().GetServentAttribute());}
                }

                if(field_3.GetFilled())
                {
                    if(!attributes.Contains(field_3.GetServentAttribute()))
                    {attributes.Add(field_3.GetServentAttribute());}
                }

                int value = attributes.Count;

                if(field_1.GetFilled())
                {field_1.GainForce(value);}

                if(field_2.GetFilled())
                {field_2.GainForce(value);}

                if(field_3.GetFilled())
                {field_3.GainForce(value);}
                break;
            }
           

            case 2: //악을 멸하는 등불
            if(field_1.GetServentAttribute() == EServentAttribute.Dark)
            {field_1.Kill();}

            if(field_2.GetServentAttribute() == EServentAttribute.Dark)
            {field_2.Kill();}

            if(field_3.GetServentAttribute() == EServentAttribute.Dark)
            {field_3.Kill();}

            if(field_4.GetComponent<Field>().GetServentAttribute() == EServentAttribute.Dark)
            {field_4.GetComponent<Field>().Kill();}

            if(field_5.GetComponent<Field>().GetServentAttribute() == EServentAttribute.Dark)
            {field_5.GetComponent<Field>().Kill();}

            if(field_6.GetComponent<Field>().GetServentAttribute() == EServentAttribute.Dark)
            {field_6.GetComponent<Field>().Kill();}

            break;

            case 3: //타오르는 심장
            ReturnMouseOnField().GainForce(ReturnMouseOnField().GetForce());
            break;

            case 4: //작은 것을 위한 희생
            
            int x = trashCount;
            
            foreach(CardData card in trashList)
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

            case 6: // 스튜
            playerHealth += 1; 
            break;

            case 7: // 피의 대가
            playerHealth -= 1; 
            DrawCard();
            break;

            case 8: // 무너진 계약
            DrawCard();
            DrawCard();
            DrawCard();

            enemyDamageBlock = true;
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
                ReturnMouseOnField().GainForce(ReturnMouseOnField().GetForce());
                ReturnMouseOnField().SetSuicide(true);
                break;
            }

            case 11: // 절규하는 투사
            {
                ReturnMouseOnField().GainForce(ReturnMouseOnField().GetForce());
                ReturnMouseOnField().AddCondition(EServentCondition.Madness);
                break;
            }
            


        }

    }

    void Update()
    {
        UpdateCondition();
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            CloseServentInfo();
        }

        // if(Input.GetKeyDown(KeyCode.Z))
        // {ShowAura();}

        // if(backgroundDetectArea.rect.Contains(backgroundDetectArea.InverseTransformPoint(Input.mousePosition)))
        // {
            
        //     if(fieldDetectArea_1.rect.Contains(fieldDetectArea_1.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Field_1;}
        //     else if(fieldDetectArea_2.rect.Contains(fieldDetectArea_2.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Field_2;}
        //     else if(fieldDetectArea_3.rect.Contains(fieldDetectArea_3.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Field_3;}
        //     else if(fieldDetectArea_4.rect.Contains(fieldDetectArea_4.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Field_4;}
        //     else if(fieldDetectArea_5.rect.Contains(fieldDetectArea_5.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Field_5;}
        //     else if(fieldDetectArea_6.rect.Contains(fieldDetectArea_6.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Field_6;}
        //     else if(playerDetectArea.rect.Contains(playerDetectArea.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Player;}
        //     else if(enemyDetectArea.rect.Contains(enemyDetectArea.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Enemy;}
        //     else if(holeDetectArea.rect.Contains(holeDetectArea.InverseTransformPoint(Input.mousePosition)))
        //     {mouseOnArea = EMouseOnArea.Hole;}
        //     else
        //     {mouseOnArea = EMouseOnArea.None;}
        // }
        // else
        // {
        //     mouseOnArea = EMouseOnArea.None;
        // }
    }

    public void UpdateAllFieldStatus()
    {
        player.GetComponent<Field>().UpdateHealth();
        enemy.GetComponent<Field>().UpdateHealth();

        field_1.GetComponent<Field>().UpdateHealth();
        field_2.GetComponent<Field>().UpdateHealth();
        field_3.GetComponent<Field>().UpdateHealth();
        field_4.GetComponent<Field>().UpdateHealth();
        field_5.GetComponent<Field>().UpdateHealth();
        field_6.GetComponent<Field>().UpdateHealth();

        player.GetComponent<Field>().UpdateCondition();
        enemy.GetComponent<Field>().UpdateCondition();

        field_1.GetComponent<Field>().UpdateCondition();
        field_2.GetComponent<Field>().UpdateCondition();
        field_3.GetComponent<Field>().UpdateCondition();
        field_4.GetComponent<Field>().UpdateCondition();
        field_5.GetComponent<Field>().UpdateCondition();
        field_6.GetComponent<Field>().UpdateCondition();
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

    IEnumerator CreateMissile(GameObject start, GameObject target) {
        int _shot = shot;
        while (_shot > 0) {
            _shot--;
            

            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
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

    // public void ShowServentInfo(Servent servent)
    // {
    //     CloseServentInfo();
    //     servent.ShowInfo();
    //     clickedServentInfo = Instantiate(serventInfoList[0], Input.mousePosition, Utils.QI);
    //     Debug.Log("되나?");
    //     Vector3 vector = clickedServentInfo.transform.position;
    //     vector.x += clickedServentInfo.GetComponent<RectTransform>().rect.width * 0.7f;
    //     clickedServentInfo.transform.position = vector;
    //     // yield return new WaitForSeconds(0.1f);
    //     clickedServentInfo.GetComponent<ServentInfoWindow>().OnOff(true);
    //     clickedServentInfo.transform.SetParent(canvas.transform);
    //     clickedServent = servent;
    // }

        public IEnumerator ShowServentInfo(Servent servent)
    {
        clickedServentInfo = Instantiate(serventInfoList[servent.GetServentNum()], Input.mousePosition, Utils.QI);
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

        for(int i = 0; i < 5; ++i)
        {
            yield return new WaitForSeconds(0.35f);
            DrawCard();
        }
        //StartCoroutine(StartTurnCo());
    }

    // void ActivateSpell(CardData cardData)
    // {

    //     switch(cardData.GetCardNum())
    //     {
    //         case 0:// 엘리멘탈 부스트
    //         List<EServentAttribute> attributes = new();
    //         if(field_1.GetComponent<Field>().GetFilled())
    //         {
    //             if(!attributes.Contains(field_1.GetComponent<Field>().GetServentAttribute()))
    //             {attributes.Add(field_1.GetComponent<Field>().GetServentAttribute());}
    //         }

    //         if(field_2.GetComponent<Field>().GetFilled())
    //         {
    //             if(!attributes.Contains(field_2.GetComponent<Field>().GetServentAttribute()))
    //             {attributes.Add(field_2.GetComponent<Field>().GetServentAttribute());}
    //         }

    //         if(field_3.GetComponent<Field>().GetFilled())
    //         {
    //             if(!attributes.Contains(field_3.GetComponent<Field>().GetServentAttribute()))
    //             {attributes.Add(field_3.GetComponent<Field>().GetServentAttribute());}
    //         }

    //         int value = attributes.Count;

    //         field_1.GetComponent<Field>().GainForce(value);
    //         field_2.GetComponent<Field>().GainForce(value);
    //         field_3.GetComponent<Field>().GainForce(value);
    //         break;
    //         // 자신의 소환수들의 속성의 종류 수만큼 자신의 모든 소환수는 포스를 얻는다.

    //         case 1: // 달의 축복
    //         selectedTarget.GetComponent<Field>().ResetCondition();
    //         break;
    //         //소환수 하나에게 걸려있는 상태를 모두 해제한다.

    //         case 2: // 정당한 거래
    //         DrawCard();
    //         player.GetComponent<Field>().LoseForce(2);
    //         break;
    //         //덱을 1장 뽑고 체력을 2 잃는다.

    //         case 3: //눈부신 빛

    //         break;
    //         //소환된 자신의 빛 속성 소환수가 있다면 상대 소환수 전부에게 [실명]을 부여한다.

    //         case 4: //사소한 건망증
    //         selectedTarget.GetComponent<Field>().AddCondition(EServentCondition.Oblivion);
    //         break;
    //         //소환수 하나에게 [망각]을 부여한다.

    //         case 5: //잔혹한 진실
            
    //         break;
    //         //자신의 소환수 하나에게 [자폭]을 부여한다.

    //         case 6: //
    //         break;
    //         //자신의 소환수 하나를 소멸시키고 그 소환수의 포스 수만큼 드로우한다.

    //         case 7: // 등가교환
    //         break;
    //         //자신의 소환수 하나와 마주보고 있는 소환수의 포스를 서로 바꾼다.

    //         case 8: //상승기류
    //         break;
    //         //자신의 바람 속성 소환수 하나를 패로 되돌린다.

    //         case 9:
    //         break;
    //         //자신의 소환수 하나의 포스를 2배로 한다. 그 소환수는 이 턴이 끝나면 소멸한다.
    //     }

    //     FieldManager.Inst.UpdateAllFieldStatus();
    // }

    // 효과를 만


     void GameSetup()
    {
        trashCount = 0;
        deckCount = 0;
        costCount = 0;
        playerHealth = 30;
        enemyHealth = 30;


        Dictionary<CardData, int> deck = new Dictionary<CardData, int>();
        List<CardData> cardDatabase = DataController.Inst.LoadCardDatabase();
        Dictionary<string, int> myDeck = DataController.Inst.LoadDeck();

        foreach(KeyValuePair<string, int> value in myDeck)
        {deck.Add(cardDatabase[Convert.ToInt32(value.Key)], value.Value);}
        deckList = new();
        cardObjectList = new();
        trashList = new();
        
        foreach(KeyValuePair<CardData, int> value in deck)
        {
            for(int i = 0; i < value.Value; ++i)
            {deckList.Add(value.Key);}
        };

        // Deck Shuffle
        Shuffle();

        if(fastMode)
            delay05 = new WaitForSeconds(0.05f);



        myTurn = true;
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
        deckCount = deckList.Count;
        trashCount = trashList.Count;

        costCountText.text = "Cost: " + costCount.ToString();
        deckCountText.text = "Deck: " + deckCount.ToString();
        trashCountText.text = "Trash: " + trashCount.ToString();

        playerHealthText.text = "PCHealth: "+ playerHealth.ToString();
        enemyHealthText.text = "EnemyHealth: "+ enemyHealth.ToString();

        field_1.UpdateHealth();
        field_2.UpdateHealth();
        field_3.UpdateHealth();
        field_4.UpdateHealth();
        field_5.UpdateHealth();
        field_6.UpdateHealth();
    }

    IEnumerator StartTurnCo()
    {        
        isLoading = true;

        player.GetComponent<Field>().UpdateHealth();
        enemy.GetComponent<Field>().UpdateHealth();

        field_1.GetComponent<Field>().UpdateHealth();
        field_2.GetComponent<Field>().UpdateHealth();
        field_3.GetComponent<Field>().UpdateHealth();
        field_4.GetComponent<Field>().UpdateHealth();
        field_5.GetComponent<Field>().UpdateHealth();
        field_6.GetComponent<Field>().UpdateHealth();

        player.GetComponent<Field>().SetAttacked(false);
        enemy.GetComponent<Field>().SetAttacked(false);

        field_1.GetComponent<Field>().SetAttacked(false);
        field_2.GetComponent<Field>().SetAttacked(false);
        field_3.GetComponent<Field>().SetAttacked(false);
        field_4.GetComponent<Field>().SetAttacked(false);
        field_5.GetComponent<Field>().SetAttacked(false);
        field_6.GetComponent<Field>().SetAttacked(false);

        if(myTurn)
        {
            if(handList.Count < 5)
            {
                int p = 5 - handList.Count;
                for(int i = 0; i < p; ++i)
                {
                    DrawCard();
                    yield return delay05;
                }
            }
            else
            {DrawCard();}
        }

        yield return delay07;
        isLoading = false;
    }

    public void StartEnemyTurn()
    {
        StartCoroutine(EnemyTurnCo());
    }

    public IEnumerator EnemyTurnCo()
    {
        int actionToken = 0;


        Debug.Log("적이 동료를 부릅니다.");
        yield return new WaitForSeconds(1f);


        Debug.Log("적이 이상한 주술을 사용합니다.");

    }

    public void CalcAlign(float cardIndex, int cardCount, GameObject card)
    {
        float maxRotation = 10; //The absolute value of the rotation for the leftmost and rightmost cards (in degrees)
        float xOffset = 0; //The horizontal center of the card fan (in worldspace units)
        float xRange = 10; //The horizontal range of the card fan (in worldspace units)
        float yOffset = -10; //The vertical center of the card fan (in worldspace units)
        float yRange = 10f; //The vertical range of the card fan (in worldspace units)

        float alignResult = 0.5f;
        if(cardCount >= 2) alignResult = cardIndex / (cardCount - 1.0f);
        float rotZ = Mathf.Lerp(-maxRotation, maxRotation, alignResult);
        float xPos = Mathf.Lerp(xOffset-xRange, xOffset+xRange, alignResult);

        if(alignResult > 0.5) alignResult = 1 - alignResult;
        alignResult *= 2;
        float yPos = Mathf.Lerp(yOffset - yRange, yOffset + yRange, alignResult);

        card.transform.position = camera.WorldToScreenPoint(new Vector3(xPos, yPos, 0));
        card.transform.eulerAngles = new Vector3(0, 0, rotZ);
    }

    // public IEnumerator EnemyTurnCo()
    // {
    //     int enemyTokens = 0;

    //     //소환 확률 배정
    //     for(int i = 0; i < enemyTokens; ++i)
    //     {
    //         List<Field> filledField = new();

    //         int probability = 0;
    //         if(field_4.GetFilled())
    //         {
    //             filledField.Add(field_4);
    //             probability += 3;
    //         }

    //         if(field_5.GetFilled())
    //         {
    //             filledField.Add(field_5);
    //             probability += 3;
    //         }

    //         if(field_6.GetFilled())
    //         {
    //             filledField.Add(field_6);
    //             probability += 3;
    //         }

    //         int p = Random.Range(1, 10);
            
    //         /*
    //             적의 성향에 따라서 행동의 우선 순위를 정할 수 있음
    //             공격적인 성향. 플레이어의 비어있는 필드에 우선적으로 소환해서 플레이어를 공격함
    //             방어적인 성향. 플레이어가 소환수를 소환한 필드에 마주보게 소환해서 플레이어의 소환수를 우선적으로 제거함
    //             중간. 플레이어 필드 상황에 상관없이 랜덤으로 소환하고 공격
    //         */

    //         if(p > probability)
    //         {
    //             List<Field> dumb = new();

    //             if
    //             (field_1.GetFilled()
    //             && field_2.GetFilled()
    //             && field_3.GetFilled())
    //             {
    //                 dumb.Add(field_4);
    //                 dumb.Add(field_5);
    //                 dumb.Add(field_6);
    //             }
    //             else if
    //             (!field_1.GetFilled()
    //             && field_2.GetFilled()
    //             && field_3.GetFilled())
    //             {dumb.Add(field_4);}
    //             else if
    //             (field_1.GetFilled()
    //             && !field_2.GetFilled()
    //             && field_3.GetFilled())
    //             {dumb.Add(field_5);}
    //             else if
    //             (field_1.GetFilled()
    //             && field_2.GetFilled()
    //             && !field_3.GetFilled())
    //             {dumb.Add(field_6);}
    //             else if
    //             (!field_1.GetComponent<Field>().GetFilled()
    //             && !field_2.GetComponent<Field>().GetFilled()
    //             && field_3.GetComponent<Field>().GetFilled())
    //             {
    //                 dumb.Add(field_4);
    //                 dumb.Add(field_5);
    //             }
    //             else if
    //             (!field_1.GetComponent<Field>().GetFilled()
    //             && field_2.GetComponent<Field>().GetFilled()
    //             && !field_3.GetComponent<Field>().GetFilled())
    //             {
    //                 dumb.Add(field_4);
    //                 dumb.Add(field_6);
    //             }
    //             else if
    //             (field_1.GetComponent<Field>().GetFilled()
    //             && !field_2.GetComponent<Field>().GetFilled()
    //             && !field_3.GetComponent<Field>().GetFilled())
    //             {
    //                 dumb.Add(field_5);
    //                 dumb.Add(field_6);
    //             }
    //             else if
    //             (!field_1.GetComponent<Field>().GetFilled()
    //             && !field_2.GetComponent<Field>().GetFilled()
    //             && !field_3.GetComponent<Field>().GetFilled())
    //             {
    //                 dumb.Add(field_4);
    //                 dumb.Add(field_5);
    //                 dumb.Add(field_6);
    //             }

    //             foreach(Field gameObject in filledField)
    //             {dumb.Remove(gameObject);}

    //             int randomNum = Random.Range(0, dumb.Count);
    //             if(dumb.Count != 0)
    //             {
    //                 Field field = dumb[randomNum];
    //                 field.Summon(new CrescentLancer(), Instantiate(serventPrefabList[0], field.transform.position , Utils.QI));
    //                 SummonServent(0, dumb[randomNum]);
    //             }else{}
                
                



    //         }//몬스터 소환
    //         else
    //         {
    //             int foo;
    //             int randomNum = Random.Range(0, 6);
    //             List<Field> filledPlayerFields = new();
    //             List<Field> filledEnemyFields = new();

    //             if(field_1.GetComponent<Field>().GetFilled())
    //             {filledPlayerFields.Add(field_1);}

    //             if(field_2.GetComponent<Field>().GetFilled())
    //             {filledPlayerFields.Add(field_2);}

    //             if(field_3.GetComponent<Field>().GetFilled())
    //             {filledPlayerFields.Add(field_3);}

    //             if(field_4.GetComponent<Field>().GetFilled())
    //             {filledEnemyFields.Add(field_4);}

    //             if(field_5.GetComponent<Field>().GetFilled())
    //             {filledEnemyFields.Add(field_5);}

    //             if(field_6.GetComponent<Field>().GetFilled())
    //             {filledEnemyFields.Add(field_6);}



                

    //             switch(randomNum)
    //             {
    //                 case 0: // Gain Force
    //                 foo = Random.Range(0, filledEnemyFields.Count);
    //                 filledEnemyFields[foo].GetComponent<Field>().GainForce(1);
    //                 break;

    //                 case 1: // Positive Ability
    //                 foo = Random.Range(0, filledEnemyFields.Count);
    //                 break;

    //                 case 2: // Lose Force
    //                 break;

    //                 case 3: // Negative Ability
    //                 break;
    //             }
    //         }

    //         /*
    //             포스 상승 버프
    //             포스 저하 디버프
    //             소환된 몬스터를 제물로 바치고 그 포스만큼 회복
    //             버프 특성 부여
    //             적에게 디버프 특성 부여
    //         */
    //     }

    //     yield return delay07;

    //     // 그 후 모든 몬스터 공격

    //     /*
    //         직공
    //         가로막는 적 공격
    //         공격 안함
    //         가로막지않는 적 공격
    //     */


    // }

    public IEnumerator EnemyAttack(GameObject start, GameObject target)
    {
        start.GetComponent<Field>().GetCardData();

        parryState = EParryState.Parry;
        parryText.text = "Parry";
        yield return new WaitForSeconds(1f);
        parryState = EParryState.Idle;
        parryText.text = "Idle";

        if(justGuard)
        {Debug.Log("패링 성공");}

        justGuard = false;
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
            return enemy;

            case EMouseOnArea.Player:
            return player;

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
            return enemy;

            case EMouseOnArea.Player:
            return player;

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

        return false;
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
            cardObject.GetComponent<Card>().GetCurrentCost(),ReturnMouseOnField())
            );
        }
        else{
            DrawDragLine(cardObject.transform.position,
            CheckCardUsable(cardObject.GetComponent<Card>().GetCardData(),
            cardObject.GetComponent<Card>().GetCurrentCost(),ReturnMouseOnField())
            );
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

        if(targetField == field_4)
        {return false;}

        if(targetField == field_5)
        {return false;}

        if(targetField == field_6)
        {return false;}

        if(targetField == player || targetField == enemy)
        {return false;}


        if(targetField.GetFilled())
        {return false;}
        return true;

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

    public IEnumerator CardEndDrag(Card card, Field targetField)
    {
        foreach(GameObject gameObject in anyWhereAreas)
        {gameObject.SetActive(false);}

        foreach(GameObject cardObject in cardObjectList)
        {cardObject.GetComponent<Card>().SetLock(false);}


        DeleteDragLine();

        isActionDone = false;

        if(mouseOnArea == EMouseOnArea.Hole)
        {
            handList.RemoveAt(card.GetCardOrder());
            cardObjectList.Remove(card.gameObject);
            AddTrash(card.GetCardData());
            card.SendMissile(alertPoint, hole.transform);
            costCount++;

            List<CardData> newHandList = new List<CardData>();

            foreach(CardData cardData in handList)
            {newHandList.Add(cardData);}

            for(int i = 0; i < cardObjectList.Count; ++i)
            {cardObjectList[i].GetComponent<Card>().SetCardOrder(i);}

            handList = newHandList;
            CardAlignmentAlt();
        }
        else
        {
            if(card.GetCardType() == ECardType.Servent)
            {
                if(CheckServentSummonable(card.GetCardData(), card.GetComponent<Card>().GetCurrentCost(), targetField))
                {
                    targetField.locked = true;
                    costCount -= card.GetCardData().GetCardCost();
                    
                    cardObjectList.Remove(card.gameObject);
                    card.SendMissile(alertPoint, ReturnMouseOnField().transform);
                    //ServentPrefab 생성
                    yield return new WaitForSeconds(1.5f);  
                    //field에 ServentData넣기
                    targetField.Summon(card.GetCardData(), Instantiate(serventPrefabList[card.GetCardData().GetServentNum()], targetField.transform.position , Utils.QI));

                    StartCoroutine(ActivateSummonAbility(card.GetCardData(), card.GetComponent<Card>().GetCurrentCost(),targetField));
                    
                    handList.RemoveAt(card.GetCardOrder());
                    List<CardData> newHandList = new List<CardData>();

                    foreach(CardData cardData in handList)
                    {newHandList.Add(cardData);}

                    for(int i = 0; i < cardObjectList.Count; ++i)
                    {cardObjectList[i].GetComponent<Card>().SetCardOrder(i);}

                    handList = newHandList;
                    CardAlignmentAlt();
                }
            }


            if(CheckCardUsable(card.GetCardData(), card.GetComponent<Card>().GetCurrentCost(), targetField))
            {
                
                switch(card.GetCardType())
                {
                    case ECardType.Spell:
                    costCount -= card.GetCardData().GetCardCost();
                    StartCoroutine(ActivateSpell(card.GetCardData()));
                    
                    AddTrash(card.GetCardData());
                    handList.RemoveAt(card.GetCardOrder());
                    cardObjectList.Remove(card.gameObject);
                    card.SendMissile(alertPoint, hole.transform);

                    List<CardData> newHandList = new List<CardData>();

                    foreach(CardData cardData in handList)
                    {newHandList.Add(cardData);}

                    for(int i = 0; i < cardObjectList.Count; ++i)
                    {cardObjectList[i].GetComponent<Card>().SetCardOrder(i);}

                    handList = newHandList;
                    CardAlignmentAlt();
                    break;

                    default:
                    break;
                }
            }
            else
            {
                foreach(GameObject cardObject in cardObjectList)
                {cardObject.GetComponent<Card>().SetLock(false);}
            }
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
        {targetList = trashList;}

        CardData cardData = targetList[targetList.Count - 1];

        cardPrefab = cardPrefabList[cardData.GetCardNum()];



        
        GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
        cardObject.transform.SetParent(canvas.transform);
        cardObjectList.Add(cardObject);
        
        cardObject.GetComponent<Card>().Setup(cardData);
        
        cardObject.GetComponent<Card>().SetCardOrder(handList.Count);
        handList.Add(cardData);


        

        targetList.RemoveAt(targetList.Count - 1);
        

        CardAlignmentAlt();
        ShotDrawMissile(cardObject.transform);
        // StartCoroutine(CreateMissile(hole, cardObjectList[cardObjectList.Count - 1]));
        
        // CardAlignment();
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
            targetCard.GetComponent<Card>().originPRS = originCardPRSs[i];
            targetCard.transform.position = originCardPRSs[i].pos;


            // CalcAlign(cardObjectList.Count, i, targetCard);
            // targetCard.GetComponent<Card>().originPRS = new PRS(targetCard.transform.position,
            //                                                     targetCard.transform.rotation,
            //                                                     targetCard.transform.localScale);
            targetCard.GetComponent<Card>().UpdateCardCost(costCount);
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

    // public void UpdateHandAlignment(int highlightedIndex)
    // {
    //     if (handList.Count == 0) return;

    //     List<PRS> positions = GetCardAlignment(cardAreaBorderLeft.position, cardAreaBorderRight.position, handList.Count, 0.5f);
    //     float offset = 50.0f; // Highlighted 카드 주변으로 밀리는 거리

    //     for (int i = 0; i < handList.Count; i++)
    //     {
    //         var targetPRS = positions[i];
    //         // cardObjectList[i].GetComponent<Card>().originPRS = positions[i];

    //         if (i < highlightedIndex)
    //         {
    //             targetPRS.pos.x -= offset;
    //         }
    //         else if (i > highlightedIndex)
    //         {
    //             targetPRS.pos.x += offset;
    //         }

    //         cardObjectList[i].GetComponent<Card>().MoveTransform(targetPRS, true, 0.2f); // DOTween으로 애니메이션
    //     }
    // }

    public void ShowAura()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();

        sequence.Append(aura.transform.DOScale(Vector3.one, 0.3f)).SetEase(Ease.InOutQuad)
        .AppendInterval(0.9f)
        .Append(aura.transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InOutQuad));
    }

    //위치 선정, 패 정렬, 미사일 발사, 카드 POP 
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
        List<PRS> originCardPRSs = new List<PRS>();

        originCardPRSs = RoundAlignment(cardAreaBorderLeft, cardAreaBorderRight, cardObjectList.Count, 0.5f, Vector3.one * 2.3f);
        for(int i = 0; i < cardObjectList.Count; ++i)
        {
            var targetCard = cardObjectList[i];
            targetCard.GetComponent<Card>().originPRS = originCardPRSs[i];
            targetCard.GetComponent<Card>().MoveTransform(targetCard.GetComponent<Card>().originPRS, true, 0.7f);

        }
    }
    public void DeleteDragLine()
    {
        cardDragLine.positionCount = 0;
        cardDragLine.endColor = Color.blue;
    }

    public void EndAttackLine(EMouseOnArea mouseOnArea, bool isUsuable)
    {
        if(ReturnMouseOnField() == ReturnMouseOnField(mouseOnArea))
        {return;}

        if(ReturnMouseOnField() == enemy)
        {
            int attackerForce = ReturnMouseOnField(mouseOnArea).GetForce();

            attackerForce += enemyDamageIncrease;
            attackerForce -= enemyDamageDecrease;

            if(enemyDamageBlock)
            {attackerForce = 0;}
            
            enemyHealth -= attackerForce;

            
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
                    {defenderDamage = 0;}
                    
                    
                    enemyHealth -= defenderDamage;
                }

            }
        }
        attackDragLine.positionCount = 0;
        ReturnMouseOnField(mouseOnArea).SetAttacked(true);
        
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



            // case EMouseOnArea.Hole:
            // targetPoint = holeDetectArea.position;
            // break;

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
        attackDragLine.SetPositions(point);
        

        // if (mouseOnField != null) {
        //      // 현재 드래그 중인 카드 가져오기
        //     if (draggedCard != null) {
        //         targetingSystem.UpdateLineRendererColor(dragLine, draggedCard.GetComponent<Card>()
        //         .GetCardData().GetCardNum(), mouseOnField);
        //     }
        //}

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
            GameObject cardObject = Instantiate(cardPrefabList[cardData.GetCardNum()], trashLayoutGroup.transform);
            GameObject cardFrameObject = Instantiate(cardSelectFrame, cardObject.transform);
            
            cardObject.GetComponent<Card>().SetLock(true);
            cardFrameObject.GetComponent<CardSelectFrame>().SetCardData(cardData);
            cardFrameObject.transform.localPosition = new Vector3(0, 0, 0);
            cardFrameObject.transform.localScale = new Vector3(1, 1, 0);

        }

        foreach(GameObject cardObject in cardObjectList)
        {cardObject.GetComponent<Card>().SetLock(true);}

        trashWindow.GetComponent<Window>().OnOff();
    }

    public void CloseTrashCards()
    {
        for( int i = trashLayoutGroup.transform.childCount - 1; i >= 0 ; --i )
        {Destroy( trashLayoutGroup.transform.GetChild(i).gameObject );}


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



    public void DrawDragLine(Vector2 startPoint, Boolean isUsuable)
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
        

        // if (mouseOnField != null) {
        //      // 현재 드래그 중인 카드 가져오기
        //     if (draggedCard != null) {
        //         targetingSystem.UpdateLineRendererColor(dragLine, draggedCard.GetComponent<Card>()
        //         .GetCardData().GetCardNum(), mouseOnField);
        //     }
        //}

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
}
