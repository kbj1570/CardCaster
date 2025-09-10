using UnityEngine;
using UnityEngine.EventSystems;

public class ExaminePoint : MonoBehaviour
{
	public int dialogueIndex;

	void OnMouseDown()
	{
		if (CampsiteManager.Inst.screenLocked)
			return;

		if (EventSystem.current.IsPointerOverGameObject())
			return;

		DialogueManager.Inst.SetLockTarget(CampsiteManager.Inst);
		DialogueManager.Inst.StartDialogue(dialogueIndex);
	}
}
