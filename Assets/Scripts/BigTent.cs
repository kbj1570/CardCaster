using UnityEngine;

public class BigTent : MonoBehaviour
{
	void OnMouseDown()
	{
		if (CampsiteManager.Inst.screenLocked)
			return;
		CampsiteManager.Inst.OpenStorage();
	}
}
