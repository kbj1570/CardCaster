using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MissionManager : MonoBehaviour
{

    public List<MissionSO> acceptedMissionList;
    public List<MissionSO> allMissionList;
    public List<MissionSO> onBoardMissionList;
    public List<RewardSO> rewardList;
    public MissionSO selectedMission;
    public bool isOpened;
    public int currentPage;
    public static MissionManager Inst {get; private set;}
    public GameObject missionPaperPrefab;
    public GameObject detailedMissionWindow;
    public Transform missionPaperLocation_1;
    public Transform missionPaperLocation_2;
    public Transform missionPaperLocation_3;
    GameObject buttonObject_1 = null;
    GameObject buttonObject_2 = null;
    GameObject buttonObject_3 = null;
    public TMP_Text acceptButtonText;
    public TMP_Text acceptedMissionCount;

    void Awake() => Inst = this;

    void Start()
    {
        CreateBoard();
        currentPage = 0;
    }



    public void CreateBoard()
    {
        buttonObject_1 = Instantiate(missionPaperPrefab, missionPaperLocation_1.transform.position , Utils.QI);
        buttonObject_2 = Instantiate(missionPaperPrefab, missionPaperLocation_2.transform.position , Utils.QI);
        buttonObject_3 = Instantiate(missionPaperPrefab, missionPaperLocation_3.transform.position , Utils.QI);

        buttonObject_1.transform.SetParent(missionPaperLocation_1);
        buttonObject_2.transform.SetParent(missionPaperLocation_2);
        buttonObject_3.transform.SetParent(missionPaperLocation_3);
        //미션벽보 생성후 위치 지정

        UpdateBoard();
    }

    public void UpdateBoard()
    {
        onBoardMissionList[0] = allMissionList[0 + (3 * currentPage)];
        onBoardMissionList[1] = allMissionList[1 + (3 * currentPage)];
        onBoardMissionList[2] = allMissionList[2 + (3 * currentPage)];

        buttonObject_1.GetComponent<MissionPaper>().UpdateStatus(onBoardMissionList[0]);
        buttonObject_2.GetComponent<MissionPaper>().UpdateStatus(onBoardMissionList[1]);
        buttonObject_3.GetComponent<MissionPaper>().UpdateStatus(onBoardMissionList[2]);

        acceptedMissionCount.text = acceptedMissionList.Count.ToString() + " / " + 4;
        //미션벽보 오브젝트에 미션 내용 할당


    }

    public void ShowDetailedMission(int value)
    {
        detailedMissionWindow.GetComponent<DetailedMissionWindow>().SetMissionData(onBoardMissionList[value], value);
        detailedMissionWindow.GetComponent<DetailedMissionWindow>()
        .UpdateButtonText(acceptedMissionList.Contains(onBoardMissionList[value]));

        detailedMissionWindow.GetComponent<Window>().OnOff();


        //클릭한 미션 벽보 새부내용 할당 후 Window On
    }

    public void AcceptMission()
    {
        
        MissionSO mission = onBoardMissionList[detailedMissionWindow.GetComponent<DetailedMissionWindow>().GetMissionNum()];
        
        if(!acceptedMissionList.Contains(mission))
        {acceptedMissionList.Add(mission);}
        else{acceptedMissionList.Remove(mission);}

        detailedMissionWindow.GetComponent<DetailedMissionWindow>().UpdateButtonText(acceptedMissionList.Contains(mission));

        UpdateBoard();
    }

    public void ChangePage(bool value)
    {
        if(value)
        {currentPage++;}
        else{currentPage--;}
        UpdateBoard();
    }
}
