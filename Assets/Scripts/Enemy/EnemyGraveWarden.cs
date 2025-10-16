using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 샘플 적: 무덤지기 (Grave Warden)
/// - 기본 스펙: HP/토큰/골드/덱/보상은 생성자에서 주입 가능(오버로드 제공)
/// - 고유 능력(EffectExecute): 필드 위 '적 소환수' 전원에게 +1 힘 부여
/// </summary>
public class EnemyGraveWarden : Enemy
{
    public EnemyGraveWarden()
    {
        enemyName   = "Grave Warden";
        enemyNum    = "EN_001";
        enemyHealth = 30;
        actionToken = 2;
        enemyGold   = 30;

        enemyRewards = new Dictionary<ItemData, int>(); // { { someItem, 10 }, ... } 식으로 외부에서 채워도 됨
        rewards      = new List<ItemData>();
        serventDeck  = new List<EnemyServentCardData>(); // 외부에서 deck 채워도 됨

        dialogueIndex = 0;
    }

    public EnemyGraveWarden(
        List<EnemyServentCardData> deck,
        Dictionary<ItemData, int> rewardWeights,
        int baseGold = 30,
        int hp = 30,
        int tokens = 2,
        string name = "Grave Warden",
        string num   = "EN_001",
        int dialogueIdx = 0)
    {
        enemyName   = name;
        enemyNum    = num;
        enemyHealth = hp;
        actionToken = tokens;

        enemyGold   = baseGold;
        enemyRewards = rewardWeights ?? new Dictionary<ItemData, int>();
        rewards      = new List<ItemData>();
        serventDeck  = deck ?? new List<EnemyServentCardData>();

        dialogueIndex = dialogueIdx;

    }



    public IEnumerator EffectExecute(BattleManager bm)
    {
        // 연출/로그
        bm.AlertMessage($"{enemyName}이(가) 종을 울렸다! (적 소환수 전원 +1 힘)");

        // 적 소환수 목록 가져와서 버프 적용
        List<Servent> enemyServents = bm.GetServents(EServentType.Enemy);
        if (enemyServents != null && enemyServents.Count > 0)
        {
            foreach (var s in enemyServents)
            {
                if (s != null)
                    s.GainForce(1);
            }
        }

        yield return new WaitForSeconds(0.3f);
    }
}