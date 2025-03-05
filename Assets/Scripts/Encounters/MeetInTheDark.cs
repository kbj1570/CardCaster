public class MeetInTheDark : Encounter
{
    public MeetInTheDark()
    {
        encounterName = "어둠속의 대화";
        encounterNum = "0";
        encounterText = new()
        {{"칠흑같은 어둠 속에서 어떤 목소리가 들려온다. 이렇게 또 뵙는군요? 여행자님! 저번에 주셨던 물건은 유용하게 잘 썼답니다. 실례가 안된다면 부탁드리고 싶은 것이 하나 더 있습니다."},
        {"피를 너무 많이 흘려서 말이죠..."}};

        encounterSelect.Add("");
        encounterSelect.Add("");
        encounterSelect.Add("무시하고 지나간다");

        encounterRequire.Add(new RedPotion(), 1);

    }

}