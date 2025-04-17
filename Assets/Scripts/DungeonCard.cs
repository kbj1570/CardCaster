using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;



public class DungeonCard : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{

    public Image energyFillImage; // 육각형 에너지 UI 이미지
    public float maxEnergy = 100f; // 최대 에너지
    private float currentEnergy = 15f; // 현재 에너지
    int energy;
    public TMP_Text nameTMP;
    public TMP_Text forceTMP;
    public TMP_Text descriptionTMP;
    public TMP_Text costTMP;
    public Sprite cardBack;
    public CardData cardData;
    public GameObject cardHighlightBorder;
    bool isUsable;
    int currentCost;
    public int cardOrder;
    public PRS originPRS;
    public Vector3 originPosition;
    public ECardType cardType;
    public bool locked = false;
    private Sequence currentSequence;

    void Awake()
    {
        originPRS.pos = this.transform.position;
        UpdateEnergyUI(); // 시작할 때 UI 초기화

        cardData = new RandomTeleporter();
    }


    public void AddEnergy(float amount)
    {
        float previousEnergy = currentEnergy;
        currentEnergy = Mathf.Min(currentEnergy + amount, maxEnergy); // 최대치를 넘지 않도록 제한

        UpdateEnergyUI(); // UI 업데이트 호출
    }

    public void ResetEnergy()
    {
        currentEnergy = 0;
        UpdateEnergyUI();
    }

    void UpdateEnergyUI()
    {
        // DOTween을 사용해 fillAmount를 부드럽게 변경
        float targetFill = currentEnergy / maxEnergy;
        energyFillImage.DOFillAmount(targetFill, 0.5f).SetEase(Ease.OutQuad);


        // 에너지가 꽉 차면 색 변화를 줘서 강조 효과 추가 (선택 사항)
        if (currentEnergy == maxEnergy)
        {
            energyFillImage.DOColor(Color.yellow, 0.2f).SetLoops(3, LoopType.Yoyo); // 색이 깜빡이는 효과
            isUsable = true;

        }
        else
        {
            energyFillImage.DOColor(Color.white, 0.2f); // 기본 색상 유지
        }
    }

    public void HideAndReveal(bool flag)
    {
        if (currentSequence != null && currentSequence.IsActive())
            currentSequence.Kill();
        
        if (flag)
        {
            // 숨기기: 더 빠르게 사라지도록 Ease.InBack 사용
            currentSequence = DOTween.Sequence()
            .Append(transform.DOMoveY(originPRS.pos.y - 330, 0.5f).SetEase(Ease.InBack));
        }
        else
        {
            // 나타나기: 목표 위치를 살짝 넘었다가 돌아오는 효과
            currentSequence = DOTween.Sequence()
            .Append(transform.DOMoveY(originPRS.pos.y + 30, 0.3f).SetEase(Ease.OutQuad)) // 살짝 위로 넘기기
            .Append(transform.DOMoveY(originPRS.pos.y, 0.2f).SetEase(Ease.OutBack)); // 부드럽게 착지
        }
    }

    public CardData GetCardData(){return cardData;}
    public bool GetIsUsable(){return isUsable;}

    public void SetCardOrder(int value)
    {this.cardOrder = value;}

    public int GetCardOrder()
    {return cardOrder;}

    public ECardType GetCardType()
    {return cardType;}

    public void UpdateCardCost(int cost)
    {
        currentCost = this.cardData.GetCardCost() - cost;
        if(currentCost < 0){currentCost = 0;}
        costTMP.text = currentCost.ToString();
    }

    public void SetLock(bool value)
    {this.locked = value;}

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
        if(locked)
        {return;}

        DungeonManager.Inst.CardBeginDrag(this.gameObject);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(locked)
        {return;}

        StartCoroutine(DungeonManager.Inst.CardEndDrag(this, DungeonManager.Inst.ReturnMouseOnNode()));
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(locked)
        {return;}

        this.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        this.transform.position = originPRS.pos;
        DungeonManager.Inst.CardOnDrag(this.gameObject);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (locked) return;

        if (currentSequence != null && currentSequence.IsActive())
            currentSequence.Kill();
        

        currentSequence = DOTween.Sequence()
            .Append(transform.DOScale(new Vector3(0.7f, 0.7f, 1), 0.13f).SetEase(Ease.InOutQuad))
            .Append(transform.DOMoveY(originPRS.pos.y + 130, 0.13f).SetEase(Ease.OutCirc));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (locked) return;

        if (currentSequence != null && currentSequence.IsActive())
            currentSequence.Kill();

        currentSequence = DOTween.Sequence()
            .Append(transform.DOScale(new Vector3(0.5f, 0.5f, 1), 0.07f).SetEase(Ease.InOutQuad))
            .Append(transform.DOMove(originPRS.pos, 0.07f).SetEase(Ease.OutCirc));
    }

    public void SetOriginPosition(Vector3 value)
    {originPosition = value;}
}
