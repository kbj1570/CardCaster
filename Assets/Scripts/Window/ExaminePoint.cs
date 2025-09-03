using UnityEngine;
using UnityEngine.EventSystems;

public class ExaminePoint : MonoBehaviour
{

	void OnMouseDown()
	{
		if (CampsiteManager.Inst.screenLocked)
			return;

		if (EventSystem.current.IsPointerOverGameObject())
			return;

		CampsiteManager.Inst.LockScreen(true);
		CampsiteManager.Inst.PlayMapOpen();
		CampsiteManager.Inst.OpenMap();
	}
}
