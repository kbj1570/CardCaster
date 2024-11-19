using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;



public class Card : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text nameTMP;
    public TMP_Text forceTMP;
    public TMP_Text descriptionTMP;
    public TMP_Text costTMP;
    public Sprite cardBack;
    public CardData cardData;
    public GameObject cardHighlightBorder;
    bool isFront;
    bool isUsable;
    int currentCost;
    public PRS originPRS;

    public ECardTargetType cardTargetType;
    public ECardType cardType;

    public CardData GetCardData(){return cardData;}
    public bool GetIsUsable(){return isUsable;}

    void Update()
    {
    }

    public void UpdateCardCost(int monsterCost, int spellCost)
    {
        if(cardData.GetCardType() == ECardType.Servent)
        {
            currentCost = this.cardData.GetCardCost() - monsterCost;
            if(currentCost < 0){currentCost = 0;}
            costTMP.text = currentCost.ToString();
        }
        else
        {
            currentCost = this.cardData.GetCardCost() - spellCost;
            if(currentCost < 0){currentCost = 0;}
            costTMP.text = currentCost.ToString();
        }
    }
    public void UpdateIsUsable()
    {isUsable = (currentCost == 0);}

    public void Setup(CardData cardData)
    {
        this.cardData = cardData;
        nameTMP.text = this.cardData.GetCardName();
        // if(cardData.GetCardType() == ECardType.Servent)
        //     forceTMP.text = this.cardData.GetForce().ToString();
        
        // descriptionTMP.text = this.cardData.GetCardAbility();
        // costTMP.text = this.cardData.GetCardCost().ToString();
        // currentCost = this.cardData.GetCardCost();
        // UpdateIsUsable();
    }

    public void MoveTransform(PRS prs, bool useDotween, float dotweenTime = 0)
    {
        if(useDotween)
        {
            transform.DOMove(prs.pos, dotweenTime);
            transform.DORotateQuaternion(prs.rot, 1);
            transform.DOScale(prs.scale, 0.5f);
        }
        else
        {
            transform.position = prs.pos;
            transform.rotation = prs.rot;
            transform.localScale = prs.scale;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //BattleManager에게 이 카드를 놓았다는 신호를 보냄
        //신호를 받은 BattleManager는 카드를 사용하는지 아니면 코스트로 버리는지 등등을 판단하며 카드 오브젝트를 삭제함
        
        BattleManagerAlt.Inst.CardEndDrag(this.gameObject);

    }

    public void OnDrag(PointerEventData eventData)
    {
        // this.transform.position = eventData.delta;
        // this.MoveTransform(new PRS(Utils.MousePos, Utils.QI, this.originPRS.scale), false);
        // Dark Night, Black Sky, The Devils Cry
        BattleManagerAlt.Inst.CardOnDrag(this.gameObject);
        this.transform.localScale = new Vector3(1, 1, 1);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        this.cardHighlightBorder.SetActive(true);
        this.transform.localScale = new Vector3(1.4f, 1.4f, 1);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        this.cardHighlightBorder.SetActive(false);
        this.transform.localScale = new Vector3(1, 1, 1);
    }
}
