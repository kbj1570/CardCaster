using System.Collections.Generic;
public class FunnyCookingTime : Encounter
{
    public FunnyCookingTime()
    {
        encounterName = "즐거운 요리시간";
        encounterNum = "1";
        encounterText = "냄비에는 정체불명의 수프가 담겨있고 주위에는 다양한 재료들이 보인다. 어떻게 할까?";

        
        firstSelection = new SelectionNode();
        firstSelection.SetRequireType(ERequireType.None);
        firstSelection.SetSelectionTitle("요리는 잘 모르지만 일단 재료를 넣어본다.");
        firstSelection.SetSelectionText(new List<string>()
        {{"조리대 옆의 선반에는 누군가가 좀 전에 손질한듯한 다양한 재료들이 놓여있었다. 어떤 걸 넣어볼까?"}});

        SelectionNode selection = new SelectionNode();
        selection.SetSelectionTitle("대파");
        selection.SetRequireType(ERequireType.None);
        firstSelection.SetFirstSelection(selection);

        secondSelection = new SelectionNode();
        secondSelection.SetRequireType(ERequireType.ECard);
        secondSelection.SetSelectionTitle("암흑 요리사에게 요리를 부탁한다.");
        secondSelection.SetRequireCard(new CookOfDarkness());
        secondSelection.SetSelectionText(new List<string>()
        {{"불안하긴 하지만.. 그래도 한번 맡겨볼까?"},
        {"이드는 카드에서 암흑 요리사를 불러냈다. 암흑 요리사는 주위를 둘러보더니 재료들을 손질하며 요리하기 시작했다."},
        {"요리를 성공적으로 마친 요리사를 다시 카드 안으로 돌려보냈다."},
        {"노란 스프를 획득했다."}});

        thirdSelection = new SelectionNode();
        thirdSelection.SetRequireType(ERequireType.None);
        thirdSelection.SetSelectionTitle("무시하고 지나간다.");
        thirdSelection.SetSelectionText(new List<string>()
        {{"뭐야. 그냥 가게? 냄비에서 흘러나오는 맛있는 냄새를 무시하고 이드는 애써 발걸음을 옮긴다."},
        {"괜히 이상한거 주워먹어서 탈나면 어쩌려고?"}});
    }

    public void CreateSelection()
    {
        int encounterNum = 0;
        switch(encounterNum)
        {
            case 0:
            


            break;
        }
    }

}