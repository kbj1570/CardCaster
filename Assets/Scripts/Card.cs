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
    public GameObject CardHighlightBorder;
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

    public void Setup(CardData cardData, bool isFront)
    {
        this.cardData = cardData;
        this.isFront = isFront;
        nameTMP.text = this.cardData.GetCardName();
        if(cardData.GetCardType() == ECardType.Servent)
            forceTMP.text = this.cardData.GetForce().ToString();
        
        descriptionTMP.text = this.cardData.GetCardAbility();
        costTMP.text = this.cardData.GetCardCost().ToString();
        currentCost = this.cardData.GetCardCost();
        UpdateIsUsable();
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
        BattleManagerAlt.Inst.DeleteDragLine();

        if(BattleManagerAlt.Inst.CheckCardUsable(this.cardTargetType ,this.cardData.cardNum))
        {Destroy(this.gameObject);}
    }

    public void OnDrag(PointerEventData eventData)
    {
        // this.transform.position = eventData.delta;
        // this.MoveTransform(new PRS(Utils.MousePos, Utils.QI, this.originPRS.scale), false);
        // Dark Night, Black Sky, The Devils Cry
        
        BattleManagerAlt.Inst.DrawDragLine(this.transform.position);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {}
    public void OnPointerExit(PointerEventData eventData)
    {}
}
