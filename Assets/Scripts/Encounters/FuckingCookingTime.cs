using System.Collections.Generic;
public class FuckingCookingTime : Encounter
{
    public FuckingCookingTime()
    {
        encounterName = "즐거운 요리시간";
        encounterNum = "1";
        encounterText = new()
        {{"냄비에는 정체불명의 수프가 담겨있고 주위에는 다양한 재료들이 보인다. 어떻게 할까?"}};

        
        firstSelection = new SelectionNode();
        firstSelection.SetRequireType(ERequireType.None);
        firstSelection.SetSelectionTitle("재료를 넣어본다.");
        firstSelection.SetSelectionText(new List<string>()
        {{"조리대 옆의 선반에는 누군가가 좀 전에 손질한듯한 다양한 재료들이 놓여있었다. 어떤걸 넣어볼까?"}});

        SelectionNode selection = new SelectionNode();
        selection.SetSelectionTitle("대파");
        selection.SetRequireType(ERequireType.None);

        firstSelection.SetFirstSelection(selection);



        secondSelection = new SelectionNode();
        secondSelection.SetRequireType(ERequireType.None);
        secondSelection.SetSelectionTitle("한입 맛본다.");
        secondSelection.SetSelectionText(new List<string>()
        {{"이드는 옆에 있는 스푼을 들고 냄비에 넣어 수프를 한입 맛보았다."},
        {"아무맛도 안 나는데..?"},
        {"이드는 발걸음을 옮긴다."}});

        thirdSelection = new SelectionNode();
        thirdSelection.SetRequireType(ERequireType.None);
        thirdSelection.SetSelectionTitle("무시하고 지나간다.");
        thirdSelection.SetSelectionText(new List<string>()
        {{"뭐야. 그냥 가게? 냄비에서 흘러나오는 맛있는 냄새를 무시하고 이드는 애써 발걸음을 옮긴다."},
        {"괜히 이상한거 주워먹어서 탈나면 어쩌려고?"}});
    }

}