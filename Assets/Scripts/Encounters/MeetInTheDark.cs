public class MeetInTheDark : Encounter
{
    public MeetInTheDark()
    {
        encounterName = "어둠속의 대화(2)";
        encounterNum = "0";
        encounterText = "칠흑같은 어둠 속에서 어떤 목소리가 들려온다. 이렇게 또 뵙는군요! 여행자님! 저번에 주셨던 물건은 유용하게 잘 썼답니다! 그런데 피를 너무 많이 흘려서 말이죠...";

        encounterSelect.Add("");
        encounterSelect.Add("");
        encounterSelect.Add("무시하고 지나간다");

        encounterRequire.Add(new RedPotion(), 1);

    }

}