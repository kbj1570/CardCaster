using System.Collections.Generic;

public class MeetInTheDark : Encounter
{
    public MeetInTheDark()
    {
        encounterName = "어둠속의 대화";
        encounterNum = "0";
        encounterText = new()
        {"칠흑같은 어둠 속에서 어떤 목소리가 들려온다. 저기....",
        "혹시 약을 갖고 계시면 좀 나눠주시겠어요?"};

        firstSelection = new();
        secondSelection = new();
        thirdSelection = new();

        firstSelection.SetSelectionTitle("물품을 준다. (붉은포션 1개)");
        secondSelection.SetSelectionTitle("왜 필요한건지 물어본다.");
        thirdSelection.SetSelectionTitle("무시하고 지나간다.");

        firstSelection.SetSelectionText(
        new(){
            "감사합니다! 복 받으실거예요!",
            "어둠이 걷히고 금방 밝아졌다. 밝아진 주위를 둘러보니 주변에는 아무도 없었다."
        });

        
    }

}