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
    public int cardOrder;
    public PRS originPRS;
    public Vector3 originPosition;

    public ECardTargetType cardTargetType;
    public ECardType cardType;

    public bool moving;
    private Sequence currentSequence;

    public CardData GetCardData(){return cardData;}
    public bool GetIsUsable(){return isUsable;}

    void Update()
    {
    }

    public void SetCardOrder(int value)
    {this.cardOrder = value;}

    public int GetCardOrder()
    {return cardOrder;}

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
        this.cardHighlightBorder.SetActive(true);
        cardType = cardData.GetCardType();
        if(cardType == ECardType.Servent)
            forceTMP.text = this.cardData.GetForce().ToString();
        
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
        
        BattleManagerAlt.Inst.CardEndDrag(this);

    }

    public void OnDrag(PointerEventData eventData)
    {
        // this.transform.position = eventData.delta;
        // this.MoveTransform(new PRS(Utils.MousePos, Utils.QI, this.originPRS.scale), false);
        // Dark Night, Black Sky, The Devils Cry

        
        if (currentSequence != null && currentSequence.IsActive())
        {currentSequence.Kill();}
                
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(1f, 1f, 1), 0.07f).SetEase(Ease.InOutQuad));
        //.Append(transform.DOMove(originPRS.pos, 0.07f).SetEase(Ease.OutCirc));

        this.transform.position = originPRS.pos;
        currentSequence = sequence;
        BattleManagerAlt.Inst.CardOnDrag(this.gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentSequence != null && currentSequence.IsActive())
        {currentSequence.Kill();}

        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(1.6f, 1.6f, 1), 0.13f).SetEase(Ease.InOutQuad))
        .Append(transform.DOMoveY(originPRS.pos.y + 70, 0.13f).SetEase(Ease.OutCirc));
        currentSequence = sequence;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (currentSequence != null && currentSequence.IsActive())
        {currentSequence.Kill();}
                
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DOScale(new Vector3(1f, 1f, 1), 0.07f).SetEase(Ease.InOutQuad))
        .Append(transform.DOMove(originPRS.pos, 0.07f).SetEase(Ease.OutCirc));
        currentSequence = sequence;
    }

    public void SetOriginPosition(Vector3 value)
    {originPosition = value;}
}
