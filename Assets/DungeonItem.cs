using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int itemNum;
    public int itemOrder;

    public void OnPointerClick(PointerEventData eventData)
    {DungeonManager.Inst.SelectUsingItem(itemNum, itemOrder);}
    public void OnPointerEnter(PointerEventData eventData)
    {DungeonManager.Inst.ShowItemDescription(itemNum);}
    public void OnPointerExit(PointerEventData eventData)
    {DungeonManager.Inst.HideItemDescription();}
}
