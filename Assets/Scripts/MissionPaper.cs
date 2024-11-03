using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MissionPaper : MonoBehaviour
{
    public TMP_Text missionType_Above;
    public TMP_Text missionPlace;

    public void UpdateStatus(MissionSO value)
    {

        switch(value.GetMissionType())
        {
            case EMissionType.Subdue:
            missionType_Above.text = "토벌";
            break;

            case EMissionType.Rescue:
            missionType_Above.text = "구출";
            break;

            case EMissionType.Errand:
            missionType_Above.text = "보급";
            break;
        }

        switch(value.GetMissionPlace())
        {
            case EMissionPlace.Market:
            missionPlace.text = "시장";
            break;

            case EMissionPlace.GraveYard:
            missionPlace.text = "무덤";
            break;

            case EMissionPlace.Laboratory:
            missionPlace.text = "실험실";
            break;

            case EMissionPlace.Church:
            missionPlace.text = "교회";
            break;

            case EMissionPlace.Abyss:
            missionPlace.text = "혼돈의 너울";
            break;
        }

    }
}
