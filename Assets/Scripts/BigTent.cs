using UnityEngine;

public class BigTent : MonoBehaviour
{
	void OnMouseDown()
	{
		CampsiteManager.Inst.OpenStorage();
	}
}
