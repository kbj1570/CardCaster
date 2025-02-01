using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectFrame : MonoBehaviour, IPointerClickHandler
{
    bool clicked;
    CardData cardData;
    
    public void OnPointerClick(PointerEventData eventData)
    {
        this.clicked = !clicked;

        if(clicked)
        {BattleManagerAlt.Inst.AddSelectedCards(cardData);}
        else
        {BattleManagerAlt.Inst.RemoveSelectedCards(cardData);}
    }

}
