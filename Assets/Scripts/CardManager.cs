// using System;
// using System.Collections;
// using System.Collections.Generic;
// using Unity.VisualScripting;
// using UnityEngine;
// using Random = UnityEngine.Random;


    
// public class CardManager : MonoBehaviour
// {
//     // Start is called before the first frame update

//     public static CardManager Inst {get; private set;}
//     void Awake() => Inst = this;

//     public List<CardData> cardData;
//     public GameObject monsterCardPrefab;
//     public GameObject spellCardPrefab;
//     public List<Card> myCards;
//     public List<Card> otherCards;
//     public Transform cardSpawnPoint;
//     public Transform myCardLeft;
//     public Transform myCardRight;
//     public Transform otherCardLeft;
//     public Transform otherCardRight;
//     public ECardState eCardState;
//     public ETarget eTarget;
//     public EMoveFrom eMoveFrom;
//     public List<CardData> selectedCards;
//     public List<CardData> cardDataBuffer;
//     public List<CardData> trashDataBuffer;
//     public List<CardData> cardListBuffer;
//     public Card selectCard;
//     Card mouseOnCard;
//     bool isMyCardDrag;
//     bool onMyCardArea;
    

//     public CardData PopItem()
//     {
//         if(cardDataBuffer.Count == 0)
//             SetupItemBuffer();
//         CardData item = cardDataBuffer[0];
//         cardDataBuffer.RemoveAt(0);
//         return item;
//     }

//     public int GetHandsCount()
//     {return myCards.Count;}

//     public List<CardData> GetCardList()
//     {return this.cardListBuffer;}

//     public void InActiveMyCards()
//     {
//         foreach(Card card in myCards)
//         {
//             card.gameObject.SetActive(false);
//         }
//     }

//     public void ActiveMyCards()
//     {
//         foreach(Card card in myCards)
//         {
//             card.gameObject.SetActive(true);
//         }
//     }

//     public void MoveSelectedCards()
//     {

//     }

//     public void SetTarget(ETarget value)
//     {
//         eTarget = value;
//     }

//     public void SetMoveFrom(EMoveFrom value)
//     {
//         eMoveFrom = value;
//     }

//     public void AddSelectedCard(CardData value)
//     {
//         selectedCards.Add(value);
//     }

//     void SetupItemBuffer()
//     {
//         cardDataBuffer = new List<CardData>(30);
//         for(int i = 0; i < cardData.Count; ++i)
//         {
//             CardData data = cardData[i];
//             cardDataBuffer.Add(data);
//         }

//         for(int i = 0; i < cardDataBuffer.Count; i++)
//         {
//             int rand = Random.Range(i, cardDataBuffer.Count);
//             CardData temp = cardDataBuffer[i];
//             cardDataBuffer[i] = cardDataBuffer[rand];
//             cardDataBuffer[rand] = temp;
//         }
//     }
//     void Start()
//     {
//         SetupItemBuffer();
//         TurnManager.OnAddCard += AddCard;
//     }

//     void OnDestroy()
//     {
//         TurnManager.OnAddCard -= AddCard;
//     }

//     void Update()
//     {
//         if(isMyCardDrag)
//             CardDrag();

//         DetectCardArea();
//         SetECardState();
//     }

//     void CardDrag()
//     {
//         if(!onMyCardArea)
//         {
//             selectCard.MoveTransform(new PRS(Utils.MousePos, Utils.QI, selectCard.originPRS.scale), false);
//         }
//     }

//     void SetECardState()
//     {
//         if(TurnManager.Inst.isLoading)
//             eCardState = ECardState.Nothing;
//         else if(!TurnManager.Inst.myTurn)
//             eCardState = ECardState.CanMouseOver;
//         else if(TurnManager.Inst.myTurn)
//             eCardState = ECardState.CanMouseDrag;
//     }

//     public void UpdateCardStatus(int monsterCost, int spellCost)
//     {
//         foreach(Card card in myCards)
//         {
//             card.UpdateCardCost(monsterCost, spellCost);
//             card.UpdateIsUsable();
//         }
//     }
//     void DetectCardArea()
//     {
//         RaycastHit2D[] hits = Physics2D.RaycastAll(Utils.MousePos, Vector3.forward);
//         int layer = LayerMask.NameToLayer("CardArea");
//         onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
//     }
    
//     void AddCard(bool isMine)
//     {
//         if(isMine)
//         {
//             CardData cardData = PopItem();
//             GameObject cardObject = null;
//             if(cardData.GetCardType() == ECardType.Monster)
//             {
//                 cardObject = Instantiate(monsterCardPrefab, cardSpawnPoint.position, Utils.QI);
//             }
//             else
//             {
//                 cardObject = Instantiate(spellCardPrefab, cardSpawnPoint.position, Utils.QI);
//             }
            
//             var card = cardObject.GetComponent<Card>();
//             card.Setup(cardData, isMine);
//             (isMine ? myCards : otherCards).Add(card);
//             SetOriginOrder(isMine);
//             CardAlignment(isMine);
//         }
//     }

//     public void SetOriginOrder(bool isMine)
//     {
//         int count = isMine ? myCards.Count : otherCards.Count;
//         for(int i = 0; i < count; ++i)
//         {
//             var targetCard = isMine ? myCards[i] : otherCards[i];
//             //targetCard?.GetComponent<Order>().SetOriginOrder(i);
//         }
//     }

//     public void CardAlignment(bool isMine)
//     {
//         List<PRS> originCardPRSs = new List<PRS>();

//         if(isMine)
//             originCardPRSs = RoundAlignment(myCardLeft, myCardRight, myCards.Count, 0.5f, Vector3.one * 4f);
//         else
//             originCardPRSs = RoundAlignment(otherCardLeft, otherCardRight, otherCards.Count, -0.5f, Vector3.one * 1.9f);
        
//         var targetCards = isMine ? myCards : otherCards;
//         for(int i = 0; i < targetCards.Count; ++i)
//         {
//             var targetCard = targetCards[i];
//             targetCard.originPRS = originCardPRSs[i];
//             targetCard.MoveTransform(targetCard.originPRS, true, 0.7f);
//         }
//     }

//     List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objectCount, float height, Vector3 scale)
//     {
//         float[] objLerps = new float[objectCount];
//         List<PRS> results = new List<PRS>(objectCount);

//         switch(objectCount)
//         {
//             case 1: objLerps = new float[] {0.5f}; break;
//             case 2: objLerps = new float[] {0.27f, 0.73f}; break;
//             case 3: objLerps = new float[] {0.1f, 0.5f, 0.9f}; break;
//             default:
//                 float interval = 1f/ (objectCount - 1);
//                 for(int i = 0; i < objectCount; ++i)
//                     objLerps[i] = interval * i;
//                 break;
//         }

//         for(int i = 0; i < objectCount; ++i)
//         {
//             var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
//             var targetRot = Quaternion.identity;
//             if(objectCount >= 4)
//             {
//                 float curve = Mathf.Sqrt(Mathf.Pow(height,2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
//                 curve = height >= 0 ? curve : - curve;
//                 targetPos.y += curve;
//                 targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
//             }
//             results.Add(new PRS(targetPos, targetRot, scale));
//         }

//         return results;
//     }
//     #region MyCard

//     public void CardMouseOver(Card card)
//     {
//         if(eCardState == ECardState.Nothing)
//         {
//             return;
//         }
//         mouseOnCard = card;
//         EnlargeCard(true, mouseOnCard);
//     }
//     public void CardMouseExit(Card card)
//     {
//         if(eCardState == ECardState.Nothing){return;}
//         EnlargeCard(false, mouseOnCard);
//         mouseOnCard = null;
//     }

//     public void CardMouseUp(Card card)
//     {
//         isMyCardDrag = false;
//         if(selectCard.GetCardData().GetCardType() == ECardType.Monster)
//         {

//             if(BattleManager.Inst.Sacrifice(selectCard.GetCardData()))
//             {
//                 myCards.Remove(selectCard);
//                 trashDataBuffer.Add(selectCard.GetCardData());
//                 Destroy(selectCard.gameObject);
//                 SetOriginOrder(true);
//                 CardAlignment(true);
//             }

//             if(!selectCard.GetIsUsable())
//             {return;}

//             if(BattleManager.Inst.Summon(selectCard.GetCardData()))
//             {
//                 myCards.Remove(selectCard);
//                 trashDataBuffer.Add(selectCard.GetCardData());
//                 Destroy(selectCard.gameObject);
//                 SetOriginOrder(true);
//                 CardAlignment(true);
//             }
            
//         }
//         else if(selectCard.GetCardData().GetCardType() == Ecard.Spell)
//         {
//             if(BattleManager.Inst.Sacrifice(selectCard.GetCardData()))
//             {
//                 myCards.Remove(selectCard);
//                 trashDataBuffer.Add(selectCard.GetCardData());
//                 Destroy(selectCard.gameObject);
//                 SetOriginOrder(true);
//                 CardAlignment(true);
//             }

//             if(!selectCard.GetIsUsable())
//             {return;}

//             if(BattleManager.Inst.ActivateSpell(selectCard.GetCardData()))
//             {
//                 myCards.Remove(selectCard);
//                 trashDataBuffer.Add(selectCard.GetCardData());
//                 Destroy(selectCard.gameObject);
//                 SetOriginOrder(true);
//                 CardAlignment(true);
//             }

//         }
//         // else if(selectCard.GetCardData().GetCardType() == ECardType.Soul)
//         // {
//         //     if(BattleManager.Inst.Sacrifice(selectCard.GetCardData()))
//         //     {
//         //         myCards.Remove(selectCard);
//         //         trashDataBuffer.Add(selectCard.GetCardData());
//         //         Destroy(selectCard.gameObject);
//         //         SetOriginOrder(true);
//         //         CardAlignment(true);
//         //     }

//         //     if(!selectCard.GetIsUsable())
//         //     {return;}

//         //     if(BattleManager.Inst.ActivateSpell(selectCard.GetCardData()))
//         //     {
//         //         myCards.Remove(selectCard);
//         //         trashDataBuffer.Add(selectCard.GetCardData());
//         //         Destroy(selectCard.gameObject);
//         //         SetOriginOrder(true);
//         //         CardAlignment(true);
//         //     }
//         // }
//         selectCard = null;

//         if(eCardState != ECardState.CanMouseDrag)
//             return;
//     }
//     public void CardMouseDown(Card card)
//     {
//         mouseOnCard = null;
//         if(eCardState != ECardState.CanMouseDrag)
//             return;
//         isMyCardDrag = true;
//         selectCard = card;
//     }

//     void EnlargeCard(bool isEnlarge, Card card)
//     {
//         if(isEnlarge)
//         {
//             Vector3 enlargePos = new Vector3(card.originPRS.pos.x, -8f, -10f);
//             card.MoveTransform(new PRS(enlargePos, Utils.QI, Vector3.one * 5f), false);
//         }
//         else
//         {
//             card.MoveTransform(card.originPRS, false);
//         }
//         //card.GetComponent<Order>().SetMostFrontOrder(isEnlarge);
//     }

    
//     #endregion

// }
