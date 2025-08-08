using UnityEngine;

public class OldHouse : MonoBehaviour
{



	void OnMouseDown()
	{
		if (CampsiteManager.Inst.screenLocked)
			return;


		CampsiteManager.Inst.PlayMapOpen();
		CampsiteManager.Inst.OpenMap();
	}
}
