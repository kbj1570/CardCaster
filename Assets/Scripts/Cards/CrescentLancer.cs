public class CrescentLancer : CardData
{
    public CrescentLancer()
    {
        cardNum = 0;
        cardName = "크레센트 랜서";
        cardCost = 1;
        cardType = ECardType.Servent;
        force = 3;
        cardGuideDescription = "누명을 쓰고 달에서 추방당했다는 전설 속의 기사, 언젠간 다시 달로 돌아갈 날을 기다리며 지상을 떠돌고 있다.";
        cardAbility = "공격 시 이 소환수의 포스가 상대 소환수의 포스보다 높다면 그 차이만큼 상대에게 대미지를 준다";
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
    }
}
