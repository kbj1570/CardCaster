using System.Collections;
using System.Collections.Generic;

public class CrescentLancer : ServentCardData
{
    public CrescentLancer()
    {
        cardNum = "101";
        cardName = "크레센트 랜서";
        cardCost = 1;
        cardType = ECardType.Servent;
        serventType = EServentType.Player;
        force = 3;
        cardStoryDesc = "누명을 쓰고 달에서 추방당했다는 전설 속의 기사, 언젠간 다시 달로 돌아갈 날을 기다리며 지상을 떠돌고 있다.";
        cardDesc = "공격 시 이 소환수의 포스가 \r\n 상대 소환수의 포스보다 높다면 \r\n 그 차이만큼 상대에게 대미지를 준다";

		strengthChallengeSuccessCommentary = new List<string>
		{
			"크레센트 랜서가 밀자 천천히 움직이기 시작했다.",
			"크레센트 랜서가 일을 마치자 다시 카드 안으로 돌아갔다.",
			"얼굴이 헬멧에 가려져 제대로 보이지는 않았지만 도움이 되어 매우 기뻐하는듯하다."
		};
		strengthChallengeFailCommentary = new List<string>
		{
			"크레센트 랜서가 아무리 힘을 써도 꿈쩍도 하지 않는다.",
			"크레센트 랜서는 다시 카드 안으로 돌아갔다.",
			"아무래도 자존심이 상한듯하다.",
			"나중에 위로해줘야겠다..."
		};
		intelligenceChallengeSuccessCommentary = new List<string>
		{
			"크레센트 랜서는 한참을 멍하니 패널을 바라보더니, 갑자기 버튼을 정확히 눌러 순서대로 해제했다.",
			"본능적인 감각인지, 달빛 아래서 익힌 기사의 예감인지… 어쨌든 장애물은 순식간에 풀려버렸다.",
			"자신도 놀란 듯 헬멧 안에서 숨죽인 웃음을 흘리며 다시 카드 안으로 돌아갔다.",
			"“이게… 된 건가?”",
			"기묘하게도 그 순간, 그의 전설 속 기사다운 품격이 잠깐 스쳐 지나갔다."
		};
		intelligenceChallengeFailCommentary = new List<string>
		{
			"소환된 크레센트 랜서는 눈앞의 수식에 당황한듯 머리를 긁적이더니, 천천히 버튼을 누르기 시작했다.",
			"자세히보니 그냥 대각선을 따라 순서대로 누르는것 같다.",
			"패널이 붉은색으로 깜빡이며, 그의 낮은 지능을 완강하게 거부했다.",
			"크레센트 랜서는 분한건지 벽을 주먹으로 몇 번 치더니, 다시 카드 안으로 돌아갔다.",
			"방금 나를 원망하는듯한 그 눈빛이 잊혀지지 않는다....",
			"괜히 불러냈나?"
		};


		penetrate = true;
        serventSize = EServentSize.Small;
        cardTargetType = ECardTargetType.Select;
        serventAttribute = EServentAttribute.Light;
    }

	public override IEnumerator SummonEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator AttackEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override IEnumerator DefendEffectExecute(BattleManager bm)
	{
		yield return null;
	}
	public override bool IsAbilityUsable(BattleManager bm)
	{
		return true;
	}
}
