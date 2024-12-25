using System.Collections.Generic;

public class ElementalBoost : CardData
{
    List<PreRequisite> preRequisites;
    public ElementalBoost()
    {
        cardNum = 1;
        cardName = "엘리멘탈 부스트";
        cardCost = 1;
        cardType = ECardType.Spell;
        cardGuideDescription = "";
        cardAbility = "자신의 소환수들의 속성의 종류 수만큼 자신의 소환수들은 포스를 얻는다";
        cardTargetType = ECardTargetType.Selected;
        
        preRequisites = new();
        PreRequisite preRequisite = new();
        preRequisite.preRequisite = EPreRequisite.PlayerServentCount;
        preRequisite.count = 1;
        preRequisite.serventAttribute = EServentAttribute.None;

    }
}

public enum ETrigger
{
    None,
    Attack, // 공격시
    Block, // 방어시
    Hit // 피격시
}

public enum EPreRequisite
{
    None, // 무조건 사용 가능
    HandCount, // 패의 매수
    TrashCount, // 트래쉬의 매수
    DeckCount, // 덱의 매수
    HandCountOver, // 패의 매수 ~이상
    TrashCountOver,// 트래쉬의 매수 ~이상
    DeckCountOver,// 덱의 매수 ~이상
    PlayerHPCount,//나의 체력
    PlayerHPCountOver,//나의 체력 ~이상
    PlayerHPCountUnder,//나의 체력 ~이하
    HandCountUnder,// 패의 매수 ~이하
    TrashCountUnder, //트래쉬의 매수 ~이하
    DeckCountUnder, //덱의 매수 ~이하
    PlayerServentCount,//나의 소환수의 수
    PlayerServentCountUnder, //나의 소환수의 수 ~이하
    PlayerServentCountOver, //나의 소환수의 수 ~이상
    EnemyServentCount,//상대의 소환수의 수
    EnemyServentCountUnder, //상대의 소환수의 수 ~이하
    EnemyServentCountOver, //상대의 소환수의 수 ~이상
    EnemyHP, //상대의 체력
    EnemyHPOver, //상대의 체력 ~이상
    EnemyHPUnder, //상대의 체력 ~이하
    AllServentCount, // 소환수의 수
    AllServentCountOver, //소환수의 수 ~이상
    AllServentCountUnder //소환수의 수 ~이하
}

public struct AbilityPreRequisite
{
    public EPreRequisite preRequisite;
    public EServentAttribute serventAttribute;
    public int count;
    public string name;
}

public struct PreRequisite
{
    public EPreRequisite preRequisite;
    public EServentAttribute serventAttribute;
    public int count;
    public string name;
}
// Verb 소환된 내 소환수가 있을 때
// 무슨 속성?
// 몇 마리?

//Verb 소환된 상대 소환수가 있을 때
//무슨 속성?
// 몇 마리?

// 소환된 내 소환수가 몇마리 이상 있을 때
// 무슨 속성?
// 몇 마리?

// 내 패가 몇 장일 때
