
using TMPro;
public class DetailedMissionWindow : Window
{
    public TMP_Text missionName;
    public TMP_Text missionDescription;
    public TMP_Text clientName;
    public int missionNum;

    public TMP_Text acceptButtonText;


    public void SetMissionData(MissionSO value, int missionNum)
    {
        missionName.text = value.GetMissionName();
        missionDescription.text = value.GetMissionDescription();
        clientName.text = value.GetClientName();
        this.missionNum = missionNum;
    }

    public int GetMissionNum()
    {return missionNum;}

    public void UpdateButtonText(bool value)
    {
        if(value)//갖고있다면
        {
            acceptButtonText.text = "Reject";
        }
        else
        {
            acceptButtonText.text = "Accept";
        }
    }
}