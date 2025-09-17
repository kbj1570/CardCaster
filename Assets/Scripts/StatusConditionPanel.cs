using TMPro;
using UnityEngine;

public class StatusConditionPanel : MonoBehaviour
{
    public TMP_Text statusConditionName;
	public TMP_Text statusConditionDesc;

	public void SetStatusCondition(EStatusCondition statusConditionType)
	{
		switch(statusConditionType)
		{
			case EStatusCondition.Confused:
				statusConditionName.text = "혼란";
				statusConditionDesc.text = "능력을 사용할 경우 50% 확률로 실패한다.";
				break;

			case EStatusCondition.Will:
				statusConditionName.text = "의지";
				statusConditionDesc.text = "치명적인 피해를 입으면 소멸하지 않고 포스가 1이 된다. 그 후 의지 상태는 해제된다.";
				break;
		}
	}
}
