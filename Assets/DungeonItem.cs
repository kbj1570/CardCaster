using UnityEngine;
using UnityEngine.EventSystems;

public class DungeonItem : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int itemNum;

    public void OnPointerClick(PointerEventData eventData)
    {DungeonManager.Inst.SetSelectedItem(itemNum);}
    public void OnPointerEnter(PointerEventData eventData)
    {DungeonManager.Inst.ShowItemDescription(itemNum);}
    public void OnPointerExit(PointerEventData eventData)
    {DungeonManager.Inst.HideItemDescription();}
}
