using UnityEngine;
using UnityEngine.EventSystems;

public class ExaminePoint : MonoBehaviour
{
	public int dialogueIndex;
	public EExaminePoint examinePoint;

	void OnMouseDown()
	{
		if (CampsiteManager.Inst.screenLocked)
			return;

		if (EventSystem.current.IsPointerOverGameObject())
			return;


		switch (examinePoint)
		{
			case EExaminePoint.None:
				{
					DialogueManager.Inst.SetLockTarget(CampsiteManager.Inst);
					DialogueManager.Inst.StartDialogue(dialogueIndex);
					break;
				}
			case EExaminePoint.NPC:
				{
					DialogueManager.Inst.SetLockTarget(CampsiteManager.Inst);
					DialogueManager.Inst.StartDialogue(dialogueIndex);
					break;
				}
			case EExaminePoint.Lygate:
				{

					if (PlayerData.saveData.lygateSeqNum == 0)
					{
						DialogueManager.Inst.SetLockTarget(CampsiteManager.Inst);
						DialogueManager.Inst.StartDialogue(0);
					}
					else if (PlayerData.saveData.lygateSeqNum == 1)
					{
						DialogueManager.Inst.SetLockTarget(CampsiteManager.Inst);
						DialogueManager.Inst.StartDialogue(0);
					}
					else if (PlayerData.saveData.lygateSeqNum == 2)
					{
						DialogueManager.Inst.SetLockTarget(CampsiteManager.Inst);
						DialogueManager.Inst.StartDialogue(0);
					}
					break;
				}
			case EExaminePoint.Merchant:
				{
					DialogueManager.Inst.SetLockTarget(CampsiteManager.Inst);
					DialogueManager.Inst.StartDialogue(dialogueIndex);
					break;
				}
			case EExaminePoint.Storage:
				{
					if (PlayerData.saveData.storageSeqNum == 0)
					{
						DialogueManager.Inst.SetLockTarget(CampsiteManager.Inst);
						DialogueManager.Inst.StartDialogue(2);
					}
					else
					{
						CampsiteManager.Inst.LockScreen(true);
						CampsiteManager.Inst.OpenStorage();
					}
					break;
				}
		}

	}
}

public enum EExaminePoint
{ None, NPC, Lygate, Tent, Merchant, Storage}
