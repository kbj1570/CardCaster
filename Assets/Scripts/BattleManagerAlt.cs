using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.UI;
using DG.Tweening;

public enum ECardType{Servent, Spell}
public enum EServentAttribute{Fire, Water, Earth, Wind, Darkness, Lightness}
public enum ECardState{Nothing, CanMouseOver, CanMouseDrag}
public enum EMouseOnArea{None, Player, Enemy, Field_1, Field_2, Field_3, Field_4, Field_5, Field_6, Hole}
public enum ECardTargetType{Selected, Select}
public enum EServentCondition{Void, Oblivion, Poison}
public enum EServentSize{Small, Middle, Big}

public class BattleManagerAlt : MonoBehaviour
{
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
    public GameObject player;
    public GameObject enemy;
    public GameObject testField;
    public Field field_1;
    public Field field_2;
    public Field field_3;
    public Field field_4;
    public Field field_5;
    public Field field_6;
    public GameObject hole;

    public GameObject aura;
    
    
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
    public EMouseOnArea mouseOnArea;
    public TMP_Text parryText;
    private List<CardData> deckList;
    private List<CardData> trashList;
    private List<CardData> handList;
    private List<GameObject> cardObjectList;
    private Dictionary<ItemSO, int> inventory;
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);
    public LineRenderer dragLine;
    public int lineCount;
    private CardTargetingSystem targetingSystem;
    enum EParryState{Idle, Parry}
    public List<GameObject> conditionMarkList;
    public List<GameObject> cardPrefabList;

    private int currentCost;
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

    private int costCount;
    private int deckCount;
    private int trashCount;



    void Start()
    {
        GameSetup();
        isLoading = true;

        handList = new();
        mouseOnArea = EMouseOnArea.None;
        
        // effectSystem = gameObject.AddComponent<CardEffectSystem>();
        // effectSystem.Initialize();
        // StartCoroutine(StartTurnCo());
    }

        
    void ActivateSpell(CardData cardData)
    {effectSystem.ExecuteCardEffect(cardData.GetCardNum(), this);}

    void Update()
    {
        UpdateCondition();
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            CloseServentInfo();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            DrawCard();
            
            // if(parryState.Equals(EParryState.Parry))
            // {justGuard = true;}

            
            // StartCoroutine(CreateMissile(player, enemy));
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

    void ShotMissile()
    {
        GameObject bullet = Instantiate(missile, hole.transform.position, Utils.QI);
            bullet.transform.SetParent(canvas.transform);
            bullet.GetComponent<BezierMissile>().master = hole;
            bullet.GetComponent<BezierMissile>().enemy = missileTarget;

        while(true)
        {bullet.GetComponent<BezierMissile>().Move();}
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
        clickedServentInfo = Instantiate(serventInfoList[0], Input.mousePosition, Utils.QI);
        Debug.Log("되나?");
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
        GameSetup();
        isLoading = true;

        for(int i = 0; i < startCardCount; ++i)
        {
            yield return delay05;
            DrawCard();
        }
        StartCoroutine(StartTurnCo());
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
        for(int i = 0; i < 100; ++i)
        {
            int a = Random.Range(0, deckList.Count);
            int b = Random.Range(0, deckList.Count);
            CardData c = deckList[a];
            deckList[a] = deckList[b];
            deckList[b] = c;
        }


        if(fastMode)
            delay05 = new WaitForSeconds(0.05f);



        myTurn = true;
    }

    public void UpdateCondition()
    {
        deckCount = deckList.Count;
        trashCount = trashList.Count;

        costCountText.text = "Cost: " + costCount.ToString();
        deckCountText.text = "Deck: " + deckCount.ToString();
        trashCountText.text = "Trash: " + trashCount.ToString();
    }

    IEnumerator StartTurnCo()
    {        
        isLoading = true;
        ResetCost();

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

    public IEnumerator EnemyTurnCo()
    {
        int enemyTokens = 2;

        //소환 확률 배정
        for(int i = 0; i < enemyTokens; ++i)
        {
            List<Field> filledField = new();

            int probability = 0;
            if(field_4.GetComponent<Field>().GetFilled())
            {
                filledField.Add(field_4);
                probability += 3;
            }

            if(field_5.GetComponent<Field>().GetFilled())
            {
                filledField.Add(field_5);
                probability += 3;
            }

            if(field_6.GetComponent<Field>().GetFilled())
            {
                filledField.Add(field_6);
                probability += 3;
            }

            int p = Random.Range(1, 10);
            
            /*
                적의 성향에 따라서 행동의 우선 순위를 정할 수 있음
                공격적인 성향. 플레이어의 비어있는 필드에 우선적으로 소환해서 플레이어를 공격함
                방어적인 성향. 플레이어가 소환수를 소환한 필드에 마주보게 소환해서 플레이어의 소환수를 우선적으로 제거함
                중간. 플레이어 필드 상황에 상관없이 랜덤으로 소환하고 공격
            */

            if(p > probability)
            {
                List<Field> dumb = new();

                if
                (field_1.GetComponent<Field>().GetFilled()
                && field_2.GetComponent<Field>().GetFilled()
                && field_3.GetComponent<Field>().GetFilled())
                {
                    dumb.Add(field_4);
                    dumb.Add(field_5);
                    dumb.Add(field_6);
                }
                else if
                (!field_1.GetComponent<Field>().GetFilled()
                && field_2.GetComponent<Field>().GetFilled()
                && field_3.GetComponent<Field>().GetFilled())
                {dumb.Add(field_4);}
                else if
                (field_1.GetComponent<Field>().GetFilled()
                && !field_2.GetComponent<Field>().GetFilled()
                && field_3.GetComponent<Field>().GetFilled())
                {dumb.Add(field_5);}
                else if
                (field_1.GetComponent<Field>().GetFilled()
                && field_2.GetComponent<Field>().GetFilled()
                && !field_3.GetComponent<Field>().GetFilled())
                {dumb.Add(field_6);}
                else if
                (!field_1.GetComponent<Field>().GetFilled()
                && !field_2.GetComponent<Field>().GetFilled()
                && field_3.GetComponent<Field>().GetFilled())
                {
                    dumb.Add(field_4);
                    dumb.Add(field_5);
                }
                else if
                (!field_1.GetComponent<Field>().GetFilled()
                && field_2.GetComponent<Field>().GetFilled()
                && !field_3.GetComponent<Field>().GetFilled())
                {
                    dumb.Add(field_4);
                    dumb.Add(field_6);
                }
                else if
                (field_1.GetComponent<Field>().GetFilled()
                && !field_2.GetComponent<Field>().GetFilled()
                && !field_3.GetComponent<Field>().GetFilled())
                {
                    dumb.Add(field_5);
                    dumb.Add(field_6);
                }
                else if
                (!field_1.GetComponent<Field>().GetFilled()
                && !field_2.GetComponent<Field>().GetFilled()
                && !field_3.GetComponent<Field>().GetFilled())
                {
                    dumb.Add(field_4);
                    dumb.Add(field_5);
                    dumb.Add(field_6);
                }

                foreach(Field gameObject in filledField)
                {dumb.Remove(gameObject);}

                int randomNum = Random.Range(0, dumb.Count);

                dumb[randomNum].GetComponent<Field>().Summon(null);


            }//몬스터 소환
            else
            {
                int foo;
                int randomNum = Random.Range(0, 6);
                List<Field> filledPlayerFields = new();
                List<Field> filledEnemyFields = new();

                if(field_1.GetComponent<Field>().GetFilled())
                {filledPlayerFields.Add(field_1);}

                if(field_2.GetComponent<Field>().GetFilled())
                {filledPlayerFields.Add(field_2);}

                if(field_3.GetComponent<Field>().GetFilled())
                {filledPlayerFields.Add(field_3);}

                if(field_4.GetComponent<Field>().GetFilled())
                {filledEnemyFields.Add(field_4);}

                if(field_5.GetComponent<Field>().GetFilled())
                {filledEnemyFields.Add(field_5);}

                if(field_6.GetComponent<Field>().GetFilled())
                {filledEnemyFields.Add(field_6);}



                

                switch(randomNum)
                {
                    case 0: // Gain Force
                    foo = Random.Range(0, filledEnemyFields.Count);
                    filledEnemyFields[foo].GetComponent<Field>().GainForce(1);
                    break;

                    case 1: // Positive Ability
                    foo = Random.Range(0, filledEnemyFields.Count);
                    break;

                    case 2: // Lose Force
                    break;

                    case 3: // Negative Ability
                    break;
                }
            }

            /*
                포스 상승 버프
                포스 저하 디버프
                소환된 몬스터를 제물로 바치고 그 포스만큼 회복
                버프 특성 부여
                적에게 디버프 특성 부여
            */
        }

        yield return delay07;

        // 그 후 모든 몬스터 공격

        /*
            직공
            가로막는 적 공격
            공격 안함
            가로막지않는 적 공격
        */


    }

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

    public void EndTurn()
    {
        myTurn = !myTurn;
        StartCoroutine(StartTurnCo());
    }

    public void CardOnDrag(GameObject cardObject)
    {
        foreach(GameObject card in cardObjectList)
        {card.GetComponent<Card>().SetLock(true);}
        cardObject.GetComponent<Card>().SetLock(false);
        // 사용가능한지 판단하는 코드


        // 사용여부에 따라서 라인의 색이 달라진다.
        DrawDragLine(cardObject.transform.position);
    }

    public void CardEndDrag(Card card)
    {
        DeleteDragLine();
        bool foo = true;
        bool mouseOnHole = false;

        Field targetField = null;
        switch(mouseOnArea)
        {
            case EMouseOnArea.Field_1:
            targetField = field_1;
            break;
            case EMouseOnArea.Field_2:
            targetField = field_2;
            break;
            case EMouseOnArea.Field_3:
            targetField = field_3;
            break;
            case EMouseOnArea.Field_4:
            targetField = field_4;
            break;
            case EMouseOnArea.Field_5:
            targetField = field_5;
            break;
            case EMouseOnArea.Field_6:
            targetField = field_6;
            break;
            case EMouseOnArea.Hole:
            foo = false;
            mouseOnHole = true;
            break;

            default:
            foo = false;
            break;
        }

        if(mouseOnHole)
        {
            handList.RemoveAt(card.GetCardOrder());
            cardObjectList.Remove(card.gameObject);
            trashList.Add(card.GetCardData());
            Destroy(card.gameObject);
            costCount++;
            // CardAlignmentAlt();
        }

        if(foo)
        {
            handList.RemoveAt(card.GetCardOrder());
            cardObjectList.Remove(card.gameObject);
            Destroy(card.gameObject);
            //ServentPrefab 생성

            SummonServent(0, targetField);
            //field에 ServentData넣기
            targetField.Summon(card.GetCardData());
            // CardAlignmentAlt();
        }

        foreach(GameObject cardObject in cardObjectList)
        {cardObject.GetComponent<Card>().SetLock(false);}

    }

    public void SummonServent(int serventID, Field field)
    {
        GameObject serventObject = Instantiate(serventPrefabList[0], field.transform.position , Utils.QI);
//      serventObject.transform.SetParent(field.transform);
    }

    //만들어야 하는 리스트?
    //덱 리스트(오브젝트 없이 데이터만)
    //패 리스트(오브젝트)
    //패 리스트(데이터) <- 굳이 필요한가?
    //트래쉬 리스트
    public void DrawCard()
    {

        if(deckList.Count == 0 && trashList.Count == 0)
        {return;}

        List<CardData> targetList;

        if(deckList.Count != 0)
        {targetList = deckList;}
        else
        {targetList = trashList;}



        
        GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
        cardObject.transform.SetParent(canvas.transform);
        cardObjectList.Add(cardObject);
        CardData cardData = targetList[targetList.Count - 1];
        cardObject.GetComponent<Card>().Setup(cardData);
        
        cardObject.GetComponent<Card>().SetCardOrder(handList.Count);
        handList.Add(cardData);


        

        targetList.RemoveAt(targetList.Count - 1);

        CardAlignmentAlt();
        // StartCoroutine(CreateMissile(hole, cardObjectList[cardObjectList.Count - 1]));
        cardObject.SetActive(true);
        
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

    public void ResetCost()
    {currentCost = 0;}

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

    public void UpdateHandAlignment(int highlightedIndex)
    {
        if (handList.Count == 0) return;

        List<PRS> positions = GetCardAlignment(cardAreaBorderLeft.position, cardAreaBorderRight.position, handList.Count, 0.5f);
        float offset = 50.0f; // Highlighted 카드 주변으로 밀리는 거리

        for (int i = 0; i < handList.Count; i++)
        {
            var targetPRS = positions[i];
            // cardObjectList[i].GetComponent<Card>().originPRS = positions[i];

            if (i < highlightedIndex)
            {
                targetPRS.pos.x -= offset;
            }
            else if (i > highlightedIndex)
            {
                targetPRS.pos.x += offset;
            }

            cardObjectList[i].GetComponent<Card>().MoveTransform(targetPRS, true, 0.2f); // DOTween으로 애니메이션
        }
    }

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
    {dragLine.positionCount = 0;}

    public void DrawDragLine(Vector2 startPoint)
    {
        Vector3[] point = new Vector3[lineCount];
        float posA = 10f;
        float posB = 10f;
        dragLine.positionCount = lineCount;
        
        Vector3 targetPoint = new Vector3();

        switch(mouseOnArea)
        {
            case EMouseOnArea.None:
            targetPoint = camera.ScreenToWorldPoint(Input.mousePosition);
            break;

            case EMouseOnArea.Field_1:
            targetPoint = field_1.GetLinePoint().transform.position;
            break;

            case EMouseOnArea.Field_2:
            targetPoint = field_2.GetLinePoint().position;
            break;

            case EMouseOnArea.Field_3:
            targetPoint = field_3.GetLinePoint().position;
            break;

            case EMouseOnArea.Field_4:
            targetPoint = fieldDetectArea_4.position;
            break;

            case EMouseOnArea.Field_5:
            targetPoint = fieldDetectArea_5.position;
            break;

            case EMouseOnArea.Field_6:
            targetPoint = fieldDetectArea_6.position;
            break;

            case EMouseOnArea.Hole:
            targetPoint = holeDetectArea.position;
            break;

            case EMouseOnArea.Player:
            targetPoint = playerDetectArea.position;
            break;

            case EMouseOnArea.Enemy:
            targetPoint = enemyDetectArea.position;
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
        dragLine.SetPositions(point);
        

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
