using UnityEngine;
using UnityEngine.EventSystems;

public class StorageArea : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public EMouseOnArea mouseOnArea;
	public void OnPointerEnter(PointerEventData eventData)
	{
		StorageWindow.Inst.SetMouseOnArea(mouseOnArea);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		StorageWindow.Inst.ResetMouseOnArea();
	}
}
