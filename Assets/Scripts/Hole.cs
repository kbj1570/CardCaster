using UnityEngine;
using UnityEngine.EventSystems;

public class Hole : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		BattleManager.Inst.SetMouseOnField(EMouseOnArea.Hole);
	}
	public void OnPointerExit(PointerEventData eventData)
	{
		BattleManager.Inst.ResetMouseOnField();
	}
}