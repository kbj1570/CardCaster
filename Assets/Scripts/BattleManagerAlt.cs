using System.Collections;
using UnityEngine;
using TMPro;
using System;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using UnityEngine.UI;

public class BattleManagerAlt : MonoBehaviour
{
    public static BattleManagerAlt Inst{get; private set;}
    void Awake() => Inst = this;
    public Canvas canvas;
    public GameObject player;
    public GameObject enemy;
    public GameObject testField;
    public GameObject field_1;
    public GameObject field_2;
    public GameObject field_3;
    public GameObject field_4;
    public GameObject field_5;
    public GameObject field_6;
    public GameObject hole;
    public GameObject cardPrefab;
    public GameObject itemWindow;
    public GameObject selectedTarget;
    public Button monsterAbilityButton;
    public Button monsterDetailButton;
    public GameObject monsterConditionPanel;
    public GameObject monsterDetailPanel;


    public Transform cardSpawnPoint;
    public Transform cardAreaBorderLeft;
    public Transform cardAreaBorderRight;
    public Field mouseOnField;
    public TMP_Text parryText;
    private List<CardData> deckList;
    private List<CardData> trashList;
    private List<CardData> handList;
    private List<GameObject> cardObjectList;
    private Dictionary<ItemSO, int> inventory;
    WaitForSeconds delay05 = new WaitForSeconds(0.5f);
    WaitForSeconds delay07 = new WaitForSeconds(0.7f);



    public List<GameObject> conditionMarkList;

    /*
    상태 리스트
    0.공허
    1.망각
    */

    private int currentCost;
    // 현재 지불해놓은 코스트의 수

    private int turn;
    //진행된 턴의 수

    private bool myTurn;
    public bool isLoading;
    public int startCardCount;
    public bool fastMode;
    enum EParryState{Idle, Parry}
    private EParryState parryState;
    private bool justGuard;

    public GameObject missile;
    public GameObject missileTarget;
    public Servent clickedServent;
    public int shot = 1;




    void Start()
    {
        GameSetup();
        isLoading = true;

        handList = new();

        
        // StartCoroutine(StartTurnCo());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(DrawCard());
            // if(parryState.Equals(EParryState.Parry))
            // {justGuard = true;}

            
            // StartCoroutine(CreateMissile(player, enemy));
        }
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
            GameObject bullet = Instantiate(missile, start.transform.position, Utils.QI);
            bullet.transform.SetParent(canvas.transform);
            bullet.GetComponent<BezierMissile>().master = start;
            bullet.GetComponent<BezierMissile>().enemy = missileTarget;

            yield return new WaitForSeconds(0.1f);
        }
        yield return null;
    }

    public void ShowServentInfo(Servent servent)
    {
        CloseServentInfo();
        servent.ShowInfo();
        clickedServent = servent;
    }
    public void CloseServentInfo()
    {
        if(clickedServent != null)
        {
            clickedServent.CloseInfo();
            clickedServent = null;
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

    void ActivateSpell(CardData cardData)
    {

        switch(cardData.GetCardNum())
        {
            case 0:// 엘리멘탈 부스트
            List<EMonsterAttribute> attributes = new();
            if(field_1.GetComponent<Field>().GetFilled())
            {
                if(!attributes.Contains(field_1.GetComponent<Field>().GetMonsterAttribute()))
                {attributes.Add(field_1.GetComponent<Field>().GetMonsterAttribute());}
            }

            if(field_2.GetComponent<Field>().GetFilled())
            {
                if(!attributes.Contains(field_2.GetComponent<Field>().GetMonsterAttribute()))
                {attributes.Add(field_2.GetComponent<Field>().GetMonsterAttribute());}
            }

            if(field_3.GetComponent<Field>().GetFilled())
            {
                if(!attributes.Contains(field_3.GetComponent<Field>().GetMonsterAttribute()))
                {attributes.Add(field_3.GetComponent<Field>().GetMonsterAttribute());}
            }

            int value = attributes.Count;

            field_1.GetComponent<Field>().GainForce(value);
            field_2.GetComponent<Field>().GainForce(value);
            field_3.GetComponent<Field>().GainForce(value);
            break;
            // 자신의 소환수들의 속성의 종류 수만큼 자신의 모든 소환수는 포스를 얻는다.

            case 1: // 달의 축복
            selectedTarget.GetComponent<Field>().ResetCondition();
            break;
            //소환수 하나에 걸려있는 상태를 모두 해제한다.

            case 2: // 공정한 거래
            DrawCard();
            player.GetComponent<Field>().LoseForce(2);
            break;
            //덱을 1장 뽑고 체력을 2 잃는다.

            case 3: //눈부신 빛
            bool foo = false;

            if(foo)
            {

            }

            break;
            //소환된 자신의 빛 속성 소환수가 있다면 상대 소환수 전부에게 [실명]을 부여한다.

            case 4: //사소한 건망증

            break;
            //소환수 하나에게 [망각]을 부여한다.

            case 5: //잔혹한 진실
            
            break;
            //자신의 소환수 하나에게 [자폭]을 부여한다.



        }

        FieldManager.Inst.UpdateAllFieldStatus();

    }

     void GameSetup()
    {
        Dictionary<CardData, int> deck = new Dictionary<CardData, int>();
        List<CardData> cardDatabase = DataController.Inst.LoadCardDatabase();
        Dictionary<string, int> dumb = DataController.Inst.LoadDeck();

        foreach(KeyValuePair<string, int> value in dumb)
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
                {DrawCard();}
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
            List<GameObject> filledField = new();

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
                List<GameObject> dumb = new();

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

                foreach(GameObject gameObject in filledField)
                {dumb.Remove(gameObject);}

                int randomNum = Random.Range(0, dumb.Count);

                dumb[randomNum].GetComponent<Field>().Summon(null);


            }//몬스터 소환
            else
            {
                int foo;
                int randomNum = Random.Range(0, 6);
                List<GameObject> filledPlayerFields = new();
                List<GameObject> filledEnemyFields = new();

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

    public IEnumerator DrawCard()
    {
        
        GameObject cardObject = Instantiate(cardPrefab, new Vector3() , Utils.QI);
        cardObject.SetActive(false);
        cardObject.transform.SetParent(canvas.transform);
        cardObjectList.Add(cardObject);
        CardData cardData = deckList[deckList.Count - 1];

        handList.Add(cardData);

        deckList.RemoveAt(deckList.Count - 1);
        

        CardAlignmentAlt();
        yield return new WaitForSeconds(0.2f);

        StartCoroutine(CreateMissile(hole, cardObjectList[cardObjectList.Count - 1]));
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

    public void SetMouseOnField(Field field)
    {mouseOnField = field;}

    public void ResetMouseOnField()
    {mouseOnField = null;}

    public void Notification(string message)
    {
        //  notificationPanel.Show(message);
    }

    public void SelectTarget(GameObject field)
    {
        missileTarget = field;
    }
    public void CardAlignmentAlt()
    {
        List<PRS> originCardPRSs = new List<PRS>();

        originCardPRSs = RoundAlignment(cardAreaBorderLeft, cardAreaBorderRight, cardObjectList.Count, 0.5f, Vector3.one * 2.3f);
        for(int i = 0; i < cardObjectList.Count; ++i)
        {
            var targetCard = cardObjectList[i];
            targetCard.GetComponent<Card>().originPRS = originCardPRSs[i];
            targetCard.transform.position = originCardPRSs[i].pos;
        }

    }

    //위치 선정, 패 정렬, 미사일 발사, 카드 POP 



    public void RoundAlignmentAlt()
    {

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
        List<PRS> originCardPRSs = new List<PRS>();

        originCardPRSs = RoundAlignment(cardAreaBorderLeft, cardAreaBorderRight, cardObjectList.Count, 0.5f, Vector3.one * 2.3f);
        for(int i = 0; i < cardObjectList.Count; ++i)
        {
            var targetCard = cardObjectList[i];
            targetCard.GetComponent<Card>().originPRS = originCardPRSs[i];
            targetCard.GetComponent<Card>().MoveTransform(
            targetCard.GetComponent<Card>().originPRS, true, 0.7f);
        }
    }

    public void TestFunction()
    {
        testField.GetComponent<Field>().AddCondition(EServentCondition.Void);
        testField.GetComponent<Field>().AddCondition(EServentCondition.Oblivion);

        testField.GetComponent<Field>().UpdateCondition();
    }

    public bool CheckSpellUsable(int spellNum)
    {
        switch(spellNum)
        {
            case 0: //엘리멘탈 부스트
            
            
            break;
        }


        return false;
    }

    
}
