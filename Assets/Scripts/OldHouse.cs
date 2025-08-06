using UnityEngine;

public class OldHouse : MonoBehaviour
{



	void OnMouseDown()
	{
		CampsiteManager.Inst.PlayMapOpen();
		CampsiteManager.Inst.OpenMap();
	}
}
