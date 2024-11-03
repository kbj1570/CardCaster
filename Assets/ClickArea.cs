
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickArea : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {BattleManagerAlt.Inst.CloseServentInfo();}
}
