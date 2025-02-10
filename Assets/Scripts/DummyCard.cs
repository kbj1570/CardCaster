using UnityEngine;
using TMPro;
using UnityEngine.UI;



public class DummyCard : MonoBehaviour
{
    public TMP_Text nameTMP;
    public TMP_Text forceTMP;
    public TMP_Text descriptionTMP;
    public TMP_Text costTMP;
    public Sprite cardBack;
    public CardData cardData;
    public GameObject cardHighlightBorder;

    public GridLayoutGroup forceAttribute;
    bool isFront;
    bool isUsable;
    int currentCost;
    public int cardOrder;
    public PRS originPRS;
    public Vector3 originPosition;
    public ECardType cardType;

    public Image fireElement;
    public Image waterElement;
    public Image earthElement;
    public Image windElement;
    public Image darknessElement;
    public Image lightElement;

    public bool locked = false;


    public CardData GetCardData(){return cardData;}
    public bool GetIsUsable(){return isUsable;}

    public void SetCardOrder(int value)
    {this.cardOrder = value;}

    public int GetCardOrder()
    {return cardOrder;}

    public ECardType GetCardType()
    {return cardType;}

    public int GetCurrentCost()
    {return currentCost;}
    public void UpdateIsUsable()
    {isUsable = (currentCost == 0);}

    public void Setup(CardData cardData)
    {
        this.cardData = cardData;
        nameTMP.text = this.cardData.GetCardName();
        // this.cardHighlightBorder.SetActive(true);
        cardType = cardData.GetCardType();
        if(cardType == ECardType.Servent)
            forceTMP.text = this.cardData.GetForce().ToString();
        
        // descriptionTMP.text = this.cardData.GetCardAbility();
        costTMP.text = this.cardData.GetCardCost().ToString();

        if(cardData.GetCardType() == ECardType.Servent)
        {
            Image image = null;
            switch(cardData.GetAttribute())
            {
                case EServentAttribute.Fire:
                image = fireElement;
                break;

                case EServentAttribute.Water:
                image = waterElement;
                break;

                case EServentAttribute.Earth:
                image = earthElement;
                break;

                case EServentAttribute.Dark:
                image = darknessElement;
                break;

                case EServentAttribute.Wind:
                image = windElement;
                break;

                case EServentAttribute.Light:
                image = lightElement;
                break;

            }
            
            for(int i = 0; i < cardData.GetForce(); ++i)
            {
                Image gameObject = Instantiate(image, forceAttribute.transform.position, Utils.QI);

                gameObject.transform.SetParent(forceAttribute.transform);

                gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
            }
        }
        // currentCost = this.cardData.GetCardCost();
        // UpdateIsUsable();
    }

    public void SetLock(bool value)
    {this.locked = value;}

    public void SetOriginPosition(Vector3 value)
    {originPosition = value;}
}
