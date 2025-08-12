using UnityEngine;
using UnityEngine.EventSystems;

public class BigTent : MonoBehaviour
{
	void OnMouseDown()
	{
		if (CampsiteManager.Inst.screenLocked)
			return;

		if (EventSystem.current.IsPointerOverGameObject())
		{
			return;
		}

		CampsiteManager.Inst.LockScreen(true);
		CampsiteManager.Inst.OpenStorage();
	}
}
