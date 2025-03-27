using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectFrame : MonoBehaviour, IPointerClickHandler
{
    bool clicked;
    CardData cardData;


    public UnityEngine.UI.Image image;

    public void SetCardData(CardData cardData){this.cardData = cardData;}

    public void OnPointerClick(PointerEventData eventData)
    {
        Color color = Color.black;
        color.a = 0;
        

        if(clicked)
        {
            
            BattleManager.Inst.RemoveSelectedCards(cardData);
            color.a = 0;
            this.clicked = !clicked;
        }
        else
        {
            if(BattleManager.Inst.AddSelectedCards(cardData))
            {
                color = Color.red;
                color.a = 0.32f;
                this.clicked = !clicked;
            }
        }
        this.image.color = color;
    }

}
